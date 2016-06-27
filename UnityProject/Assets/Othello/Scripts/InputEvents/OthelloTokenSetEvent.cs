using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloTokenSetEvent : IEvent
    {
        public int eventType { get { return (int)OthelloInputEvents.TokenSet; } }
    }
}
