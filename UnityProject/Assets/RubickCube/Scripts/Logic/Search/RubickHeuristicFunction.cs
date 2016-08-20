using aima.search.framework;
using aima.basic;
using System;

namespace Uag.AI.RubickCube
{
    public class RubickHeuristicFunction : HeuristicFunction
    {
        public int getHeuristicValue(object state)
        {
            RubickColorMatrix node = (RubickColorMatrix)state;
            return node.HeuristicCostEstimate();
        }
    }
}
