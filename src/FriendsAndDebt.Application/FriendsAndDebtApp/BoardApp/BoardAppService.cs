using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using FriendsAndDebt.Authorization.Users;
using FriendsAndDebt.FAD;
using FriendsAndDebt.Users.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendsAndDebt.FriendsAndDebtApp.BoardApp;

public class DeleteBoardModel : EntityDto<long>
{
}

public class GetBoardModel : EntityDto<long>
{
}

[AutoMap(typeof(Board))]
public class UpdateBoardModel : CreateBoardModel, IEntityDto<long>
{
    public long Id { get; set; }

    [StringLength(Board.MaxNameLength)]
    public string Name { get; set; }
}

[AutoMap(typeof(Board))]

public class CreateBoardModel
{
    [StringLength(Board.MaxColorLength)]
    public string? Color { get; set; }

    [StringLength(Board.MaxNameLength)]
    public string Name { get; set; }

    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string Owner { get; set; }
}

public class GetAllBoardModel : PagedAndSortedResultRequestDto
{

}

[AutoMap(typeof(Board))]
public class BoardModel : EntityDto<long>
{
    [StringLength(Board.MaxColorLength)]
    public string? Color { get; set; }

    [StringLength(Board.MaxNameLength)]
    public string Name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UserDto Owner { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<UserDto> Members { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<CardDto> Cards { get; set; }
}

public class CardDto : EntityDto<long>
{
    [StringLength(Card.MaxTitleLength)]
    public string Title { get; set; }

    [StringLength(Card.MaxDescriptionLength)]
    public string Description { get; set; }

    public decimal Amount { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UserDto Owner { get; }
}

[AutoMap(typeof(Board))]
public class BoardAddMemberModel : EntityDto<long>
{
    public string[] UserNames { get; set; }
}

[AutoMap(typeof(Card))]
public class BoardAddCardModel
{
    public long BoardId { get; set; }

    [StringLength(Card.MaxTitleLength)]
    public string Title { get; set; }

    [StringLength(Card.MaxDescriptionLength)]
    public string Description { get; set; }

    public long OwnerId { get; set; }

    public decimal Amount { get; set; }
}

public class DebtDto : EntityDto<long>
{
    [StringLength(Debt.MaxDescriptionLength)]
    public string Description { get; set; }

    public decimal SponsorAmount { get; set; }

    public decimal Amount { get; set; }

    public UserDto Debtor { get; set; }

    public UserDto Creditor { get; set; }

    public UserDto? Sponsor { get; set; }
}

public class SponsorModel : EntityDto<long>
{
    public long SponsorId { get; set; }

    public decimal SponsorAmount { get; set; }
}

public interface IProjectAppService : IAsyncCrudAppService<BoardModel, long, GetAllBoardModel, CreateBoardModel, UpdateBoardModel, GetBoardModel, DeleteBoardModel>
{
    Task<BoardModel> AddCardsAsync(BoardAddCardModel input);
    Task<BoardModel> AddMembersAsync(BoardAddMemberModel input);
}

public class BoardAppService(IRepository<Board, long> repository, UserStore userStore, IRepository<Card, long> cardRepository)
    : AsyncCrudAppService<Board, BoardModel, long, GetAllBoardModel, CreateBoardModel, UpdateBoardModel, GetBoardModel, DeleteBoardModel>(repository), IProjectAppService
{
    protected override Task<Board> GetEntityByIdAsync(long id)
    {
        var entity = Repository.GetAllIncluding(x => x.Owner, x => x.Members, x => x.Cards)
            .Include(x => x.Cards).ThenInclude(x => x.Debts).FirstOrDefault(x => x.Id == id);

        return Task.FromResult(entity);
    }

    public async Task<BoardModel> AddMembersAsync(BoardAddMemberModel input)
    {
        var entity = Repository.Get(input.Id);
        foreach (string item in input.UserNames)
        {
            var user = await userStore.GetUserFromDatabaseAsync(item);
            entity.Members.AddIfNotContains(user);
        }

        Repository.Update(entity);
        UnitOfWorkManager.Current.SaveChanges();

        return ObjectMapper.Map<BoardModel>(entity);
    }

    public async Task<BoardModel> AddCardsAsync(BoardAddCardModel input)
    {
        var entity = Repository.Get(input.BoardId);
        var cardEntity = ObjectMapper.Map<Card>(input);
        cardEntity.Board = entity;
        cardRepository.Insert(cardEntity);
        UnitOfWorkManager.Current.SaveChanges();

        return ObjectMapper.Map<BoardModel>(entity);
    }
}

