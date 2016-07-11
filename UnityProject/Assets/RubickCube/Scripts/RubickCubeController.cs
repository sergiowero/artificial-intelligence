using System;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeController : ControllerBase
    {
        public override Type GetEventEnumType()
        {
            return typeof(RubickCubeInputEvents);
        }
    }
}
