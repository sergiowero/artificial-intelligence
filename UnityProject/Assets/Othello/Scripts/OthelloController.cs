using UnityEngine;
using System;
using Uag.AI.Common.MVC;

namespace Uag.AI.Othello
{
    public class OthelloController : ControllerBase
    {
        public override Type GetEventEnumType()
        {
            return typeof(OthelloInputEvents);
        }
    }
}
