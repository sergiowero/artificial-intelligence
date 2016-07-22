using Uag.AI.Common.Events;

namespace Uag.AI.RubickCube
{
    public class RubickCubeShuffleInputEvent : IEvent
    {
        public int steps { get; private set; }

        public RubickCubeShuffleInputEvent(int _steps)
        {
            steps = _steps;
        }

        public int eventType { get { return (int)RubickCubeInputEvents.Shuffle; } }
    }
}
