using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeStartInputEvent : IEvent
    {
        public int eventType { get { return (int)RubickCubeInputEvents.Start; } }
    }
}
