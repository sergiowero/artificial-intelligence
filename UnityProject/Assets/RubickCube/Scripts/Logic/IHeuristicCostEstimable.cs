using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uag.AI.RubickCube
{
    interface IHeuristicCostEstimable
    {
        int HeuristicCostEstimate();
    }
}
