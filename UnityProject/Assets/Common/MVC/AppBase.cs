using UnityEngine;

namespace Uag.AI.Common.MVC
{
    public abstract class AppBase : MonoBehaviour
    {
        public static AppBase instance { get;  protected set; }

        public ModelBase Model { get; private set; }
        public ControllerBase Controller { get; private set; }
        public ViewBase View { get; private set; }

        protected virtual void Awake()
        {
            instance = this;

            Model = CreateModel();
            Controller = CreateController();
            View = CreateView();

            Controller.AddGeneralHandler(Model.HandleInput);
            View.SubscribeHandlers(Model);
        }

        protected abstract ModelBase CreateModel();
        protected abstract ControllerBase CreateController();
        protected abstract ViewBase CreateView();

    }
}
