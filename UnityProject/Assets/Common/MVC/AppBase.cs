using UnityEngine;

namespace Uag.AI.Common.MVC
{
    public abstract class AppBase : MonoBehaviour
    {
        public static AppBase instance { get; protected set; }

        public ModelBase model { get; private set; }
        public ControllerBase controller { get; private set; }
        public ViewBase view { get; private set; }

        protected virtual void Awake()
        {
            instance = this;

            model = CreateModel();
            controller = CreateController();
            view = CreateView();

            controller.AddGeneralHandler(model.HandleInput);
            view.SubscribeHandlers(model);

            controller.Init();
            model.Init();
            view.Init();
        }

        protected virtual void Start()
        {
            controller.OnStartApp();
        }

        protected abstract ModelBase CreateModel();
        protected abstract ControllerBase CreateController();
        protected abstract ViewBase CreateView();

    }
}
