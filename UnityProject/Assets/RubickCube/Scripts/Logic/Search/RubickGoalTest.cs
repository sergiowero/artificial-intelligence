using aima.search.framework;

namespace Uag.AI.RubickCube
{
    public class RubickGoalTest : GoalTest
    {
        public bool isGoalState(object state)
        {
            RubickColorMatrix node = (RubickColorMatrix)state;
            return node.IsResolved();
        }
    }
}
