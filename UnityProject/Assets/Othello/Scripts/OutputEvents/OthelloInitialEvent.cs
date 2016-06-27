using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloInitialEvent : IEvent
    {
        public int eventType { get { return (int)OthelloOutputEvents.Initial; } }
    }
}
