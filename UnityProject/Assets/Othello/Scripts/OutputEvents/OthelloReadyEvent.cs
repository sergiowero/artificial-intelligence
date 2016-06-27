using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloReadyEvent : IEvent
    {
        public int eventType { get { return (int)OthelloOutputEvents.Ready; } }
    }
}
