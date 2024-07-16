using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FriendsAndDebt.Authorization.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FriendsAndDebt.FAD;

public class Friend : AggregateRoot<long>
{
    public const int MaxIntroduceLength = 64;

    [StringLength(MaxIntroduceLength)]
    public string? Introduce { get; set; }

    public bool IsApprove { get; set; }

    public long OwnerId { get; set; }

    public long UserId { get; set; }

    public User Owner { get; set; }

    public User User { get; set; }

    public void Approve()
    {
        IsApprove = true;
        DomainEvents.Add(new FriendApprovedEvent(this.Id));
    }

}

public class Board : FullAuditedAggregateRoot<long>
{

    public const int MaxNameLength = 128;

    public const int MaxColorLength = 16;

    [StringLength(MaxColorLength)]
    public string? Color { get; set; }

    [StringLength(MaxNameLength)]
    public string Name { get; set; }

    public long OwnerId { get; set; }

    public User Owner { get; set; }

    public ICollection<User> Members { get; set; }

    public ICollection<Card> Cards { get; set; }
}


public class Card : FullAuditedAggregateRoot<long>
{

    public const int MaxTitleLength = 128;

    public const int MaxDescriptionLength = 256;

    [StringLength(MaxTitleLength)]
    public string Title { get; set; }

    [StringLength(MaxDescriptionLength)]
    public string Description { get; set; }

    public decimal Amount { get; set; }

    public long OwnerId { get; set; }

    public User Owner { get; }

    public Board Board { get; set; }

    public ICollection<Debt> Debts { get; set; }
}

public class Debt : FullAuditedAggregateRoot<long>
{
    public const int MaxDescriptionLength = 128;

    [StringLength(MaxDescriptionLength)]
    public string Description { get; set; }

    public decimal SponsorAmount { get; set; }

    public decimal Amount { get; set; }

    //Con nợ
    public User Debtor { get; set; }

    //chủ nợ
    public User Creditor { get; set; }

    //Người bảo trợ
    public User? Sponsor { get; set; }
}