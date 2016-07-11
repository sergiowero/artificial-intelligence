using System.Collections.Generic;

namespace Uag.AI.RubickCube
{
    public class RubickMatrix
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

        private int[] m_matrix;

        private int[][] m_resolvedState;

        private Stack<RubickMovementTypes> m_moveStack;

        public RubickMatrix()
        {
            m_matrix = new int[WIDTH * HEIGHT];
            Reset();
        }

        public void Reset()
        {
            m_moveStack.Clear();
            for (int i = 0; i < m_matrix.Length; i++)
            {
                m_matrix[i] = identity[i];
            }
            SetupResolveState();
        }

        public void Revert()
        {
            while (m_moveStack.Count > 0)
            {
                var move = m_moveStack.Pop();

            }
        }

        public void SaveState()
        {
            m_moveStack.Clear();
        }

        private void SetupResolveState()
        {
            m_resolvedState = new int[ELEMS][];

            for (int i = 4; i < 12; i++)
            {
                int elem = identity[i];
                m_resolvedState[elem] = GetRelatives(i);
            }
        }

        public int[] GetRelatives(int _idx)
        {
            int[] res = new int[4];
            int x, y;
            ToCoordinates(_idx, out x, out y);
            res[0] = identity[ToIndex(x, y - 1)];
            res[1] = identity[ToIndex(x + 1, y)];
            res[2] = identity[ToIndex(x, y + 1)];
            res[3] = identity[ToIndex(x - 1, y)];
            return res;
        }

        public int[] GetSpectedRelatives(int _elem)
        {
            int[] res = new int[4];
            m_resolvedState[_elem].CopyTo(res, 0);
            return res;
        }

        public void Transform(params RubickMovementTypes[] _moves)
        {
            for (int i = 0; i < _moves.Length; i++)
            {
                Transform(_moves[i]);
            }
        }

        private void Transform(RubickMovementTypes _move)
        {
            m_moveStack.Push(_move);
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
            int start = _col;

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
    }
}
