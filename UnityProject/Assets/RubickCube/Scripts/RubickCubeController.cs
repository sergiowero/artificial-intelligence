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

        public override void OnStartApp()
        {
            SendStart();
        }

        public void SendStart()
        {
            DispatchEvent(new RubickCubeStartInputEvent());
        }

        public void SendShuffle()
        {
            DispatchEvent(new RubickCubeShuffleInputEvent(RubickCubeApp.instance.shuffleSteps));
        }

        public void SendResolve()
        {
            DispatchEvent(new RubickCubeResolveInputEvent());
        }
    }
}
