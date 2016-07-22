using System;
using Uag.AI.Common.Events;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeModel : ModelBase
    {
        public RubickMatrix rubickCube { get; private set; }
        public AStarTree aStarSearchTree { get; private set; }

        public RubickCubeModel() : base()
        {
            rubickCube = new RubickMatrix();
            aStarSearchTree = new AStarTree();
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
            rubickCube.Shuffle(_shuffleEvent.steps);
            rubickCube.Apply();
            SendState();
        }

        public void Resolve()
        {
            aStarSearchTree.Search(rubickCube);
            if(aStarSearchTree.result != null)
            {
                rubickCube = aStarSearchTree.result;
            }
            SendState();
        }

        private void SendState()
        {
            DispatchEvent(new RubickCubeStateOutputEvent((RubickMatrix)rubickCube.Clone()));
        }
    }
}
