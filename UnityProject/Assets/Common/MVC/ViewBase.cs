using System;
using UnityEngine;

namespace Uag.AI.Common.MVC
{
    public abstract class ViewBase : MonoBehaviour
    {
        public abstract void SubscribeHandlers(ModelBase _model);
    }
}
