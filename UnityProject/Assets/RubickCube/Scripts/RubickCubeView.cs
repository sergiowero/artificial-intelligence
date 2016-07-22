using System;
using UnityEngine;
using Uag.AI.Common.Events;
using Uag.AI.Common.MVC;

namespace Uag.AI.RubickCube
{
    public class RubickCubeView : ViewBase
    {
        public override void SubscribeHandlers(ModelBase _model)
        {
            _model.AddEventHandler(RubickCubeOutputEvents.State, OnRubickStateChanged);
        }

        private void OnRubickStateChanged(IEvent _event)
        {
            RubickCubeStateOutputEvent stateEvent = (RubickCubeStateOutputEvent)_event;
            Debug.LogFormat("Resolved <b>{0}</b> \n{1}", stateEvent.rubickCube.IsResolved(), stateEvent.rubickCube);
                
        }
    }
}
