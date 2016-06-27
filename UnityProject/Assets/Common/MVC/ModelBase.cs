using System;
using Uag.AI.Common.Events;

namespace Uag.AI.Common.MVC
{
    public abstract class ModelBase : EventDispatcher
    {
        public ModelBase()
        {
            Initialize(GetEventEnumType());
        }

        public abstract Type GetEventEnumType();

        public abstract void HandleInput(IEvent _event);


    }
}
