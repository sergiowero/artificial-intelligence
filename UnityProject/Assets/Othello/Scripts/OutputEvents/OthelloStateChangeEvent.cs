using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloStateChangeEvent : IEvent
    {
        public int eventType { get { return (int)OthelloOutputEvents.StateChange; } }
    }
}
