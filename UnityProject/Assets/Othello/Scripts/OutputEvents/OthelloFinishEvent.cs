using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloFinishEvent : IEvent
    {
        public int eventType { get { return (int)OthelloOutputEvents.Finish; } }
    }
}
