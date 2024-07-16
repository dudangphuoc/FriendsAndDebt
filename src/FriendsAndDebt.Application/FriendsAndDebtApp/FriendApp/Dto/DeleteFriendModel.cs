using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using FriendsAndDebt.FAD;
using System.ComponentModel.DataAnnotations;

namespace FriendsAndDebt.FriendsAndDebtApp.FriendApp.Dto;
public class DeleteFriendModel : EntityDto<long>
{
}

public class GetFriendModel : EntityDto<long>
{
}

public class UpdateFriendModel : IEntityDto<long>
{
    public long Id { get; set; }

    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }
}

public class CreateFriendModel
{
    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }

    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string Owner { get; set; }
    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string User { get; set; }
}

public class GetAllFriendModel : PagedAndSortedResultRequestDto
{

}

public class FriendModel : EntityDto<long>
{
    [StringLength(Friend.MaxIntroduceLength)]
    public string Introduce { get; set; }
}