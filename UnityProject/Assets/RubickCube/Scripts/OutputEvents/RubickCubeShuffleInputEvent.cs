using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeStateOutputEvent : IEvent
    {
        public RubickMatrix rubickCube { get; private set; }

        public RubickCubeStateOutputEvent(RubickMatrix _cube)
        {
            rubickCube = _cube;
        }

        public int eventType { get { return (int)RubickCubeOutputEvents.State; } }
    }
}
