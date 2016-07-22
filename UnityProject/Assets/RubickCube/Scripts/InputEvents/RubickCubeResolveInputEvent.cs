using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeResolveInputEvent : IEvent
    {
        public int eventType { get { return (int)RubickCubeInputEvents.Resolve; } }
    }
}
