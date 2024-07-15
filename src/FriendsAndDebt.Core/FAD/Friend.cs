using Abp.Domain.Entities;
using FriendsAndDebt.Authorization.Users;
using System.Collections.Generic;
namespace FriendsAndDebt.FAD;

public class Friend : AggregateRoot<long>
{
    public User Owner { get; set; }

    public User User { get; set; }

    public bool IsApprove { get; set; }
}

public class Board : AggregateRoot<long>
{
    public string BoardCode { get; set; }

    public string Name { get; set; }

    public string Color { get; set; }

    public User Owner { get; set; }

    public ICollection<User> Members { get; set; }

    public ICollection<Card> Cards { get; set; }
}


public class Card : AggregateRoot<long>
{
    public string CardCode { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public User Owner { get; }

    public Board Board { get; }

    public decimal Amount { get; set; }

    public ICollection<Debt> Debts { get; set; }
}

public class Debt : AggregateRoot<long>
{
    public User Debtor { get; set; }

    public User Creditor { get; set; }

    public User? Sponsor { get; set; }

    public decimal SponsorAmount { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }
}