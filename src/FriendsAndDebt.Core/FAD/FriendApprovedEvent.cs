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
    public DateTime EventTime { get; set; }
    public object EventSource { get; set; }
}