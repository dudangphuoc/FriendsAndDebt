using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using FriendsAndDebt.FAD;
using System.ComponentModel.DataAnnotations;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;

public class DeleteFriendModel : EntityDto<long>
{
}

public class GetFriendModel : EntityDto<long>
{
}

[AutoMap(typeof(Friend))]
public class UpdateFriendModel : IEntityDto<long>
{
    public long Id { get; set; }

    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }
}

[AutoMap(typeof(Friend))]
public class CreateFriendModel
{
    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }

    public long UserId { get; set; }
}

public class GetAllFriendModel : PagedResultRequestDto
{
    public FriendFilterType RequestType { get; set; }
}

[AutoMap(typeof(Friend))]
public class FriendModel : EntityDto<long>
{
    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }

    public string OwnerName { get; set; }

    public string FriendName { get; set; }
}