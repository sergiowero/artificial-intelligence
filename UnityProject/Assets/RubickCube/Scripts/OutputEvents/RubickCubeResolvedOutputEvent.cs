using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeResolvedOutputEvent : IEvent
    {
        public RubickColorMatrix rubickCube { get; private set; }
        public int iterations { get; private set; }
        public bool success { get; private set; }

        public RubickCubeResolvedOutputEvent(RubickColorMatrix _cube, int _iterations)
        {
            rubickCube = _cube;
            iterations = _iterations;
            success = _cube != null && _cube.IsResolved();
        }

        public int eventType { get { return (int)RubickCubeOutputEvents.Resolved; } }
    }
}
