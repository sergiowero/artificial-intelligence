using System;
using UnityEngine;
using Uag.AI.Common.Events;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeView : ViewBase
    {
        public RubickCubeObjectController cubeObject;

        public override void SubscribeHandlers(ModelBase _model)
        {
            _model.AddEventHandler(RubickCubeOutputEvents.State, OnRubickStateChanged);
            _model.AddEventHandler(RubickCubeOutputEvents.Resolved, OnRubickResolved);
        }

        public void ActionShuffle()
        {
            RubickCubeApp.instance.controller.SendShuffle();
        }

        public void ActionResolve()
        {
            RubickCubeApp.instance.controller.SendResolve();
        }

        private void OnRubickStateChanged(IEvent _event)
        {
            RubickCubeStateOutputEvent stateEvent = (RubickCubeStateOutputEvent)_event;
            Debug.LogFormat("State received \n{0}", stateEvent.rubickCube.Print());
            cubeObject.SetState(stateEvent.rubickCube.moveStack);
        }

        private void OnRubickResolved(IEvent _event)
        {
            RubickCubeResolvedOutputEvent resolvedEvent = (RubickCubeResolvedOutputEvent)_event;
            Debug.LogFormat("Resolved <b>{0}</b> in <b>{3}</b> - Its = <b>{1}</b> \n{2}", resolvedEvent.success, resolvedEvent.iterations, resolvedEvent.rubickCube.Print(), resolvedEvent.rubickCube.stackSize);
            cubeObject.SetSolution(resolvedEvent.rubickCube.moveStack);
        }
    }
}
