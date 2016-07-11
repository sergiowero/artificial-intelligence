using System;
using Uag.AI.Common.Events;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeModel : ModelBase
    {
        public override Type GetEventEnumType()
        {
            return typeof(RubickCubeOutputEvents);
        }

        public override void HandleInput(IEvent _event)
        {
            
        }
    }
}
