using System;
using Uag.AI.Common.Events;

namespace Uag.AI.Common.MVC
{
    public abstract class ControllerBase : EventDispatcher
    {
        public ControllerBase()
        {
            Initialize(GetEventEnumType());
        }

        public virtual void Init()
        {

        }
        public abstract void OnStartApp();


        public abstract Type GetEventEnumType();

    }
}
