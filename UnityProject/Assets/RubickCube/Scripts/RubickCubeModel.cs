using System;
using Uag.AI.Common.Events;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeModel : ModelBase
    {
        public RubickColorMatrix rubickCube { get; private set; }
        public AStarTreeColor aStarSearchTree { get; private set; }

        public RubickCubeModel() : base()
        {
            rubickCube = new RubickColorMatrix();
            aStarSearchTree = new AStarTreeColor();
            //UnityEngine.Debug.Log(rubickCube.Print());
        }

        public override Type GetEventEnumType()
        {
            return typeof(RubickCubeOutputEvents);
        }

        public override void Init()
        {

        }

        public override void HandleInput(IEvent _event)
        {
            RubickCubeInputEvents evtType = (RubickCubeInputEvents)_event.eventType;
            switch (evtType)
            {
                case RubickCubeInputEvents.Start:
                    SendState();
                    break;
                case RubickCubeInputEvents.Shuffle:
                    Shuffle((RubickCubeShuffleInputEvent)_event);
                    break;
                case RubickCubeInputEvents.Resolve:
                    Resolve();
                    break;
                default:
                    break;
            }
        }

        public void Shuffle(RubickCubeShuffleInputEvent _shuffleEvent)
        {
            rubickCube.Reset();
            rubickCube.Shuffle(_shuffleEvent.steps);
            SendState();
        }

        public void Resolve()
        {
            rubickCube.Apply();
            aStarSearchTree.Search(rubickCube);
            int iterations = -1;
            if (aStarSearchTree.result != null)
            {
                rubickCube = aStarSearchTree.result;
                iterations = aStarSearchTree.iterations;
            }
            DispatchEvent(new RubickCubeResolvedOutputEvent((RubickColorMatrix)rubickCube.Clone(), iterations));
        }

        private void SendState()
        {
            DispatchEvent(new RubickCubeStateOutputEvent((RubickColorMatrix)rubickCube.Clone()));
        }
    }
}
