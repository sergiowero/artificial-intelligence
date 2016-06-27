using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloTokesChange : IEvent
    {
        public int eventType { get { return (int)OthelloOutputEvents.TokensChange; } }
    }
}
