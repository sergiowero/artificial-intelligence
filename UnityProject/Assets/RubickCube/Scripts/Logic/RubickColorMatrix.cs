using System.Collections.Generic;
using System.Text;
using System;

namespace Uag.AI.RubickCube
{
    public class RubickColorMatrix : ICloneable, IEquatable<RubickColorMatrix>, IHeuristicCostEstimable
    {
        private static HashSet<int> s_hash = new HashSet<int>();

        private List<RubickColorPiece> m_pieces;
        public List<RubickColorPiece> pieces { get { return m_pieces; } }

        private List<RubickMovementTypes> m_moveStack;
        public int stackSize { get { return m_moveStack.Count; } }
        public List<RubickMovementTypes> moveStack { get { return m_moveStack; } }
        //public RubickMatrix m_matrix;

        public RubickColorMatrix()
        {
            m_pieces = new List<RubickColorPiece>();
            m_moveStack = new List<RubickMovementTypes>();
            // Add 8 pieces
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            m_pieces.Add(new RubickColorPiece());
            //m_matrix = new RubickMatrix();
            Reset();
        }

        public void Reset()
        {
            //m_matrix.Reset();
            m_moveStack.Clear();
            m_pieces[0].Setup(0, RubickColor.Yellow, RubickOrientation.Front, RubickColor.Orange, RubickOrientation.Up, RubickColor.Blue, RubickOrientation.Left);
            m_pieces[1].Setup(1, RubickColor.Yellow, RubickOrientation.Front, RubickColor.Orange, RubickOrientation.Up, RubickColor.Green, RubickOrientation.Right);
            m_pieces[2].Setup(2, RubickColor.Yellow, RubickOrientation.Front, RubickColor.Red, RubickOrientation.Down, RubickColor.Green, RubickOrientation.Right);
            m_pieces[3].Setup(3, RubickColor.Yellow, RubickOrientation.Front, RubickColor.Red, RubickOrientation.Down, RubickColor.Blue, RubickOrientation.Left);
            m_pieces[4].Setup(4, RubickColor.White, RubickOrientation.Back, RubickColor.Orange, RubickOrientation.Up, RubickColor.Blue, RubickOrientation.Left);
            m_pieces[5].Setup(5, RubickColor.White, RubickOrientation.Back, RubickColor.Orange, RubickOrientation.Up, RubickColor.Green, RubickOrientation.Right);
            m_pieces[6].Setup(6, RubickColor.White, RubickOrientation.Back, RubickColor.Red, RubickOrientation.Down, RubickColor.Green, RubickOrientation.Right);
            m_pieces[7].Setup(7, RubickColor.White, RubickOrientation.Back, RubickColor.Red, RubickOrientation.Down, RubickColor.Blue, RubickOrientation.Left);
        }

        public void Revert()
        {
            while (m_moveStack.Count > 0)
            {
                var move = m_moveStack[m_moveStack.Count - 1];
                m_moveStack.RemoveAt(m_moveStack.Count - 1);
                move = (RubickMovementTypes)(-((int)move)); // get the reverse move
                Transform(move, false);
            }
            //m_matrix.Revert();
        }

        public void Apply()
        {
            m_moveStack.Clear();
            //m_matrix.Apply();
        }

        public void Shuffle(int _steps = 100)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            while (_steps > 0)
            {
                int num = rand.Next(4) + 1;
                num = rand.NextDouble() > 0.5 ? num : -num;
                RubickMovementTypes move = (RubickMovementTypes)num;
                Transform(move);
                _steps--;
            }
        }

        public bool IsResolved()
        {
            return HeuristicCostEstimate() == 0;
        }

        public int HeuristicCostEstimate()
        {
            int estimate = 0;

            for (int i = 0; i < 6; i++)
            {
                s_hash.Clear();
                for (int p = 0; p < pieces.Count; p++)
                {
                    int curr = pieces[p].data[i];
                    if (curr != -1)
                    {
                        s_hash.Add(curr);
                    }
                }
                estimate += s_hash.Count - 1;
            }

            return estimate;
        }

        public void Transform(params RubickMovementTypes[] _moves)
        {
            for (int i = 0; i < _moves.Length; i++)
            {
                Transform(_moves[i]);
            }
        }

        private void Transform(RubickMovementTypes _move, bool _save = true)
        {
            if (_save)
            {
                //UnityEngine.Debug.LogFormat("Move <b>{0}</b>", _move);
                m_moveStack.Add(_move);
            }
            //m_matrix.Transform(_move);
            m_pieces.ForEach(p => p.Transform(_move));
            m_pieces.Sort((p1, p2) =>
            {
                if (p1.state > p2.state)
                    return 1;
                else if (p2.state > p1.state)
                    return -1;
                else return 0;
            });
        }

        public bool Equals(RubickColorMatrix other)
        {
            //return System.Object.ReferenceEquals(this, other);

            //If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            for (int i = 0; i < m_pieces.Count; i++)
            {
                for (int d = 0; d < 6; d++)
                {
                    if (this.m_pieces[i].data[d] != other.m_pieces[i].data[d])
                        return false;
                }
            }

            return true;
        }

        public string Print()
        {
            StringBuilder str = new StringBuilder();
            str.Append("---------------------------|\n");
            for (int i = 0; i < m_pieces.Count; i++)
            {
                str.AppendFormat(" <b>{0}</b>:<color=green>{7}</color> | {1} | {2} | {3} | {4} | {5} | {6} |\n",
                    m_pieces[i].state,
                    m_pieces[i].data[0] == -1 ? "-" : m_pieces[i].data[0].ToString(),
                    m_pieces[i].data[1] == -1 ? "-" : m_pieces[i].data[1].ToString(),
                    m_pieces[i].data[2] == -1 ? "-" : m_pieces[i].data[2].ToString(),
                    m_pieces[i].data[3] == -1 ? "-" : m_pieces[i].data[3].ToString(),
                    m_pieces[i].data[4] == -1 ? "-" : m_pieces[i].data[4].ToString(),
                    m_pieces[i].data[5] == -1 ? "-" : m_pieces[i].data[5].ToString(),
                    m_pieces[i].pieceNum);
            }
            str.Append("---------------------------|\n");
            str.AppendFormat(" - Stack size {0}\n", m_moveStack.Count);
            str.Append("---------------------------|\n");
            //str.Append("\n Matrix -----------------|\n");
            //str.Append(m_matrix.Print());
            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals((RubickColorMatrix)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RubickColorMatrix a, RubickColorMatrix b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(RubickColorMatrix a, RubickColorMatrix b)
        {
            return !(a == b);
        }

        public object Clone()
        {
            RubickColorMatrix other = new RubickColorMatrix();
            other.m_pieces.Clear();
            this.m_pieces.ForEach(p => other.m_pieces.Add((RubickColorPiece)p.Clone()));
            other.m_moveStack.AddRange(this.m_moveStack);
            //other.m_matrix = (RubickMatrix)this.m_matrix.Clone();
            return other;
        }
    }
}
