using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeResolvedOutputEvent : IEvent
    {
        public RubickMatrix rubickCube { get; private set; }
        public int iterations { get; private set; }
        public bool success { get; private set; }

        public RubickCubeResolvedOutputEvent(RubickMatrix _cube, int _iterations)
        {
            rubickCube = _cube;
            iterations = _iterations;
            success = _cube != null && _cube.IsResolved();
        }

        public int eventType { get { return (int)RubickCubeOutputEvents.Resolved; } }
    }
}
