using Uag.AI.Common.MVC;

namespace Uag.AI.Othello
{
    public class OthelloApp : AppBase
    {
        public static new OthelloApp instance { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        protected override ControllerBase CreateController()
        {
            return new OthelloController();
        }

        protected override ModelBase CreateModel()
        {
            return new OthelloModel();
        }

        protected override ViewBase CreateView()
        {
            return FindObjectOfType<OthelloView>();
        }
    }
}
