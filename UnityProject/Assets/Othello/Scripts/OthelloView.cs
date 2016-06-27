using Uag.AI.Common.MVC;
using Uag.AI.Common.Events;

namespace Uag.AI.Othello
{
    public class OthelloView : ViewBase
    {
        public override void SubscribeHandlers(ModelBase _model)
        {
            _model.AddEventHandler(OthelloOutputEvents.Initial, OnInit);
            _model.AddEventHandler(OthelloOutputEvents.StateChange, OnStateChange);
            _model.AddEventHandler(OthelloOutputEvents.TokensChange, OnTokensChange);
            _model.AddEventHandler(OthelloOutputEvents.Ready, OnReady);
            _model.AddEventHandler(OthelloOutputEvents.Finish, OnFinish);
        }

        public void OnInit(IEvent _event)
        {

        }

        public void OnStateChange(IEvent _event)
        {

        }

        public void OnTokensChange(IEvent _event)
        {

        }

        public void OnReady(IEvent _event)
        {

        }

        public void OnFinish(IEvent _event)
        {

        }
    }
}
