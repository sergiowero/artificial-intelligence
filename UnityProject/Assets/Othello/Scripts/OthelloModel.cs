using System;
using Uag.AI.Common.MVC;
using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloModel : ModelBase
    {
        public override Type GetEventEnumType()
        {
            return typeof(OthelloOutputEvents);
        }

        public override void HandleInput(IEvent _event)
        {
            
        }
    }
}
