using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeStateOutputEvent : IEvent
    {
        public RubickColorMatrix rubickCube { get; private set; }

        public RubickCubeStateOutputEvent(RubickColorMatrix _cube)
        {
            rubickCube = _cube;
        }

        public int eventType { get { return (int)RubickCubeOutputEvents.State; } }
    }
}
