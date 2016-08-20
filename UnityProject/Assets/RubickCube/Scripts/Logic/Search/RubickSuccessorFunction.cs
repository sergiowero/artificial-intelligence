using System.Collections;
using aima.search.framework;

namespace Uag.AI.RubickCube
{
    public class RubickSuccessorFunction : SuccessorFunction
    {
        public ArrayList getSuccessors(object state)
        {
            RubickColorMatrix node = (RubickColorMatrix)state;
            ArrayList successors = new ArrayList();
            RubickMovementTypes lastMove = node.stackSize > 0 ? node.moveStack[node.moveStack.Count - 1] : RubickMovementTypes.None;
            if (lastMove != RubickMovementTypes.None)
            {
                lastMove = (RubickMovementTypes)(-((int)lastMove));
            }

            var moves = System.Enum.GetValues(typeof(RubickMovementTypes));
            for (int i = 0; i < moves.Length; i++)
            {
                RubickMovementTypes val = (RubickMovementTypes)moves.GetValue(i);
                if (val != RubickMovementTypes.None && val != lastMove)
                {
                    RubickColorMatrix clone = (RubickColorMatrix)node.Clone();
                    clone.Transform(val);
                    successors.Add(new Successor(val.ToString(), clone));
                }
            }

            return successors;
        }
    }
}
