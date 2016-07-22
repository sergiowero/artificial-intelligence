using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uag.AI.RubickCube
{
    public class AStarTree
    {
        public RubickMatrix result { get; private set; }

        private List<RubickMatrix> closedSet = new List<RubickMatrix>();
        private List<RubickMatrix> openSet = new List<RubickMatrix>();
        private Dictionary<RubickMatrix, int> gScore = new Dictionary<RubickMatrix, int>();
        private Dictionary<RubickMatrix, int> fScore = new Dictionary<RubickMatrix, int>();

        public AStarTree()
        {
        }

        private int GetGScore(RubickMatrix _node)
        {
            int val = int.MaxValue;
            gScore.TryGetValue(_node, out val);
            return val;
        }

        private int GetFScore(RubickMatrix _node)
        {
            int val = int.MaxValue;
            fScore.TryGetValue(_node, out val);
            return val;
        }

        private int HeuristicCostEstimate(RubickMatrix _node)
        {
            int estimate = 0;
            for (int i = 4; i < 12; i++)
            {
                var my = _node.GetRelatives(i);
                var target = RubickMatrix.GetSpectedRelatives(_node.GetValue(i));
                for (int j = 0; j < my.Length; j++)
                {
                    if (my[j] != target[j])
                        estimate ++;
                }
            }
            return estimate;
        }

        private int DistanceBetween(RubickMatrix _a, RubickMatrix _b)
        {
            int dist = _a.stackSize - _b.stackSize;
            
            return System.Math.Abs(dist) * 8;
        }

        private RubickMatrix FindLowestFScore()
        {
            RubickMatrix elem = null;
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

        private List<RubickMatrix> GenerateNeighbors(RubickMatrix _node)
        {
            List<RubickMatrix> neighbors = new List<RubickMatrix>();

            var moves = System.Enum.GetValues(typeof(RubickMovementTypes));
            for (int i = 0; i < moves.Length; i++)
            {
                RubickMovementTypes val = (RubickMovementTypes)moves.GetValue(i);
                if (val != RubickMovementTypes.None)
                {
                    RubickMatrix clone = (RubickMatrix)_node.Clone();
                    clone.Transform(val);
                    neighbors.Add(clone);
                }
            }

            return neighbors;
        }

        public void Search(RubickMatrix _start)
        {
            result = null;
            closedSet.Clear();
            openSet.Clear();
            openSet.Add(_start);

            gScore.Clear();
            gScore[_start] = 0;

            fScore.Clear();
            fScore[_start] = HeuristicCostEstimate(_start);
            int iterations = 0;
            RubickMatrix current = null;
            while (openSet.Count > 0)
            {
                iterations++;
                current = FindLowestFScore();
                if (current.IsResolved())
                {
                    Debug.Log(iterations);
                    result = current;
                    return;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                IList<RubickMatrix> neighbors = GenerateNeighbors(current);

                foreach (var neighbor in neighbors)
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int tentativeGScore = GetGScore(current) + DistanceBetween(current, neighbor);
                    if(!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if(tentativeGScore >= GetGScore(neighbor))
                    {
                        continue;
                    }

                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor);
                }
            }
        }
    }
}

