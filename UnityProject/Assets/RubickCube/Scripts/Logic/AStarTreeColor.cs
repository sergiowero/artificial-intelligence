using System.Collections.Generic;

namespace Uag.AI.RubickCube
{
    public class AStarTreeColor
    {
        public RubickColorMatrix result { get; private set; }
        public int iterations { get; private set; }

        private List<RubickColorMatrix> closedSet = new List<RubickColorMatrix>();
        private List<RubickColorMatrix> openSet = new List<RubickColorMatrix>();
        private Dictionary<RubickColorMatrix, int> gScore = new Dictionary<RubickColorMatrix, int>();
        private Dictionary<RubickColorMatrix, int> fScore = new Dictionary<RubickColorMatrix, int>();

        private int GetGScore(RubickColorMatrix _node)
        {
            int val = int.MaxValue;
            gScore.TryGetValue(_node, out val);
            return val;
        }

        private int GetFScore(RubickColorMatrix _node)
        {
            int val = int.MaxValue;
            fScore.TryGetValue(_node, out val);
            return val;
        }

        private int HeuristicCostEstimate(RubickColorMatrix _node)
        {
            return _node.HeuristicCostEstimate();
        }

        private int DistanceBetween(RubickColorMatrix _a, RubickColorMatrix _b)
        {
            int dist = _a.stackSize - _b.stackSize;

            return System.Math.Abs(dist);
        }

        private RubickColorMatrix FindLowestFScore()
        {
            RubickColorMatrix elem = null;
            int min = int.MaxValue;

            foreach (var item in openSet)
            {
                int fscore = GetFScore(item);
                if (elem == null || fscore < min)
                {
                    elem = item;
                    min = fscore;
                }
            }

            return elem;
        }

        private List<RubickColorMatrix> GenerateNeighbors(RubickColorMatrix _node)
        {
            List<RubickColorMatrix> neighbors = new List<RubickColorMatrix>();

            RubickMovementTypes lastMove = _node.stackSize > 0 ? _node.moveStack[_node.moveStack.Count - 1] : RubickMovementTypes.None;
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
                    RubickColorMatrix clone = (RubickColorMatrix)_node.Clone();
                    clone.Transform(val);
                    neighbors.Add(clone);
                }
            }

            return neighbors;
        }

        private List<RubickColorMatrix> GetBestNeighbors(RubickColorMatrix _node)
        {
            List<RubickColorMatrix> neighbors = new List<RubickColorMatrix>();
            int min = int.MaxValue;
            RubickColorMatrix minNeightbor = null;

            var moves = System.Enum.GetValues(typeof(RubickMovementTypes));
            for (int i = 0; i < moves.Length; i++)
            {
                RubickMovementTypes val = (RubickMovementTypes)moves.GetValue(i);
                if (val != RubickMovementTypes.None)
                {
                    RubickColorMatrix clone = (RubickColorMatrix)_node.Clone();
                    clone.Transform(val);
                    int cost = clone.HeuristicCostEstimate();
                    if (cost < min)
                    {
                        min = cost;
                        minNeightbor = clone;
                    }
                }
            }

            if(min == _node.HeuristicCostEstimate())
            {
                UnityEngine.Debug.Log("Duh");
            }
            neighbors.Add(minNeightbor);
            return neighbors;
        }

        public void Search(RubickColorMatrix _start)
        {
            result = null;
            closedSet.Clear();
            openSet.Clear();
            openSet.Add(_start);

            gScore.Clear();
            gScore[_start] = 0;

            fScore.Clear();
            fScore[_start] = HeuristicCostEstimate(_start);
            iterations = 0;
            RubickColorMatrix current = null;
            int attepts = 2000;
            while (openSet.Count > 0 && attepts > 0)
            {
                attepts--;
                iterations++;

                if(attepts == 0)
                {
                    UnityEngine.Debug.Log("Hola");
                }
                current = FindLowestFScore();
                if (current.IsResolved())
                {
                    result = current;
                    return;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                IList<RubickColorMatrix> neighbors = GenerateNeighbors(current);

                foreach (var nb in neighbors)
                {
                    RubickColorMatrix neighbor = nb;
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int tentativeGScore = GetGScore(current) + DistanceBetween(current, neighbor);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else
                    {
                        neighbor = openSet.Find(n => n == neighbor);
                        if (tentativeGScore >= GetGScore(neighbor))
                        {
                            continue;
                        }
                    }

                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor);
                }
            }
        }
    }
}

