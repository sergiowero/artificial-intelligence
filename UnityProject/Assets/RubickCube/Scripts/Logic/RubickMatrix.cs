using System.Collections.Generic;
using System.Text;
using System;

namespace Uag.AI.RubickCube
{
    public class RubickMatrix : ICloneable, IEquatable<RubickMatrix>, IHeuristicCostEstimable
    {
        public const int ELEMS = 8;
        public const int WIDTH = 4;
        public const int HEIGHT = 4;

        // The initial state of the cube, in this state the cube is reolved
        public static readonly int[] identity =
        {
            0, 4, 5 ,1,
            4, 0, 1, 5,
            7, 3, 2, 6,
            3, 7, 6, 2,
        };

        private static int[][] s_resolvedState;

        static RubickMatrix()
        {
            s_resolvedState = new int[ELEMS][];

            for (int i = 4; i < 12; i++)
            {
                int elem = identity[i];
                s_resolvedState[elem] = GetRelatives(i, identity);
            }
        }

        public int stackSize { get { return m_moveStack.Count; } }
        public List<RubickMovementTypes> moveStack { get { return m_moveStack; } }

        private int[] m_matrix;

        private List<RubickMovementTypes> m_moveStack;

        public RubickMatrix()
        {
            m_matrix = new int[WIDTH * HEIGHT];
            m_moveStack = new List<RubickMovementTypes>();
            Reset();
        }

        public void Reset()
        {
            m_moveStack.Clear();
            for (int i = 0; i < m_matrix.Length; i++)
            {
                m_matrix[i] = identity[i];
            }
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
        }

        public void Apply()
        {
            m_moveStack.Clear();
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
            for (int i = 4; i < 12; i++)
            {
                var my = GetRelatives(i, m_matrix);
                var resolved = GetSpectedRelatives(m_matrix[i]);
                for (int j = 0; j < my.Length; j++)
                {
                    if (my[j] != resolved[j])
                        return false;
                }
            }
            return true;
        }

        public int HeuristicCostEstimate()
        {
            int estimate = 0;
            for (int i = 4; i < 12; i++)
            {
                var my = GetRelatives(i);
                var target = RubickMatrix.GetSpectedRelatives(GetValue(i));
                for (int j = 0; j < my.Length; j++)
                {
                    if (my[j] != target[j])
                        estimate++;
                }
            }
            return estimate;
        }

        public int GetValue(int _idx)
        {
            return GetValue(m_matrix, _idx);
        }

        public int[] GetRelatives(int _idx)
        {
            return GetRelatives(_idx, m_matrix);
        }

        public int[] GetRelativesByElem(int _elem)
        {
            for (int i = 4; i < 12; i++)
            {
                if (m_matrix[i] == _elem)
                {
                    return GetRelatives(i, m_matrix);
                }
            }
            return null;
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
                m_moveStack.Add(_move);
            }
            switch (_move)
            {
                case RubickMovementTypes.TopTurnRight:
                    TransformRow(0, -1);
                    TransformRow(1, 1);
                    break;
                case RubickMovementTypes.TopTurnLeft:
                    TransformRow(0, 1);
                    TransformRow(1, -1);
                    break;
                case RubickMovementTypes.BottomTurnRight:
                    TransformRow(2, 1);
                    TransformRow(3, -1);
                    break;
                case RubickMovementTypes.BottomTurnLeft:
                    TransformRow(2, -1);
                    TransformRow(3, 1);
                    break;
                case RubickMovementTypes.LeftTurnUp:
                    TransformColumn(0, 1);
                    TransformColumn(1, -1);
                    break;
                case RubickMovementTypes.LeftTurnDown:
                    TransformColumn(0, -1);
                    TransformColumn(1, 1);
                    break;
                case RubickMovementTypes.RightTurnUp:
                    TransformColumn(2, -1);
                    TransformColumn(3, 1);
                    break;
                case RubickMovementTypes.RightTurnDown:
                    TransformColumn(2, 1);
                    TransformColumn(3, -1);
                    break;
            }
        }

        private void TransformRow(int _row, int _dir)
        {
            int start = _row * WIDTH;

            int[] values = new int[WIDTH];
            System.Array.Copy(m_matrix, start, values, 0, WIDTH);

            for (int i = 0; i < WIDTH; i++)
            {
                int newIndex = ToIndex(i + _dir, _row);
                m_matrix[newIndex] = values[i];
            }
        }

        private void TransformColumn(int _col, int _dir)
        {
            int[] values = new int[HEIGHT];
            for (int i = 0; i < HEIGHT; i++)
            {
                values[i] = m_matrix[ToIndex(_col, i)];
            }

            for (int i = 0; i < HEIGHT; i++)
            {
                int newIndex = ToIndex(_col, i + _dir);
                m_matrix[newIndex] = values[i];
            }
        }

        public bool Equals(RubickMatrix other)
        {
            for (int i = 4; i < 12; i++)
            {
                //var my = GetRelatives(i, m_matrix);
                //var theirs = other.GetRelativesByElem(m_matrix[i]);
                //for (int j = 0; j < my.Length; j++)
                //{
                //    if (my[j] != theirs[j])
                //        return false;
                //}
                if (m_matrix[i] != other.m_matrix[i])
                    return false;
            }
            return true;
        }

        public string Print()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < m_matrix.Length; i += 4)
            {
                str.AppendFormat("{0},{1},{2},{3}\n", m_matrix[i], m_matrix[i + 1], m_matrix[i + 2], m_matrix[i + 3]);
            }
            //str.AppendFormat("\n - Stack size {0}", m_moveStack.Count);
            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals((RubickMatrix)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RubickMatrix a, RubickMatrix b)
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

        public static bool operator !=(RubickMatrix a, RubickMatrix b)
        {
            return !(a == b);
        }

        public static int ToIndex(int _x, int _y)
        {
            _x = GetIndex(_x, WIDTH);
            _y = GetIndex(_y, HEIGHT);

            return _x + _y * WIDTH;
        }

        public static void ToCoordinates(int _idx, out int _x, out int _y)
        {
            _x = _idx % WIDTH;
            _y = _idx / WIDTH;
        }

        public static int GetValue(int[] _array, int _index)
        {
            _index = GetIndex(_index, _array.Length);

            return _array[_index];
        }

        public static int GetIndex(int _index, int _length)
        {
            if (_index < 0)
            {
                _index = _index % _length;
                _index = _length + _index;
            }
            if (_index >= _length)
            {
                _index = _index % WIDTH;
            }

            return _index;
        }

        private static int[] GetRelatives(int _idx, int[] _matrix)
        {
            int[] res = new int[4];
            int x, y;
            ToCoordinates(_idx, out x, out y);
            res[0] = _matrix[ToIndex(x, y - 1)];
            res[1] = _matrix[ToIndex(x + 1, y)];
            res[2] = _matrix[ToIndex(x, y + 1)];
            res[3] = _matrix[ToIndex(x - 1, y)];
            return res;
        }

        public static int[] GetSpectedRelatives(int _elem)
        {
            int[] res = new int[4];
            s_resolvedState[_elem].CopyTo(res, 0);
            return res;
        }

        public object Clone()
        {
            RubickMatrix other = new RubickMatrix();
            this.m_matrix.CopyTo(other.m_matrix, 0);
            other.m_moveStack.AddRange(this.m_moveStack);
            return other;
        }
    }
}
