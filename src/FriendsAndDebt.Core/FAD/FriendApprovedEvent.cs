using Abp.Events.Bus;
using System;

namespace FriendsAndDebt.FAD;
public class FriendApprovedEvent : IEventData
{
    public long FriendId { get; set; }

    public FriendApprovedEvent(long friend)
    {
        FriendId = friend;
    }
    public DateTime EventTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public object EventSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}