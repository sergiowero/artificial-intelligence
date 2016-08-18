using System.Collections.Generic;
using System.Text;
using System;

namespace Uag.AI.RubickCube
{
    public enum RubickColor
    {
        None = -1,
        Yellow = 0,
        Orange,
        White,
        Red,
        Blue,
        Green
    }

    public enum RubickOrientation
    {
        None = -1,
        Front = 0,
        Up,
        Back,
        Down,
        Left,
        Right
    }

    public class RubickColorPiece : ICloneable
    {

        // State trasition table
        // State | Events
        //-------|------------------------------------------------------------------------------------------------------------------------| 
        //       | TopTurnRight | TopTurnLeft | BottomTurnRight | BottomTurnLeft | LeftTurnUp | LeftTurnDown | RightTurnUp | RightTurnDown|  
        //   0   |      1       |      4      |        0        |       0        |     4      |      3       |      0      |      0       |
        //   1   |      5       |      0      |        1        |       1        |     1      |      1       |      5      |      2       |
        //   2   |      2       |      2      |        6        |       3        |     2      |      2       |      1      |      6       |
        //   3   |      3       |      3      |        2        |       7        |     0      |      7       |      3      |      3       |
        //   4   |      0       |      5      |        4        |       4        |     7      |      0       |      4      |      4       |
        //   5   |      4       |      1      |        5        |       5        |     5      |      5       |      6      |      1       |
        //   6   |      6       |      6      |        7        |       2        |     6      |      6       |      2      |      5       |
        //   7   |      7       |      7      |        3        |       6        |     3      |      4       |      7      |      7       |
        // -------------------------------------------------------------------------------------------------------------------------------|
        private static readonly int[,] s_stateTrasitionTable =
        {
            {1,4,0,0,4,3,0,0},
            {5,0,1,1,1,1,5,2},
            {2,2,6,3,2,2,1,6},
            {3,3,2,7,0,7,3,3},
            {0,5,4,4,7,0,4,4},
            {4,1,5,5,5,5,6,1},
            {6,6,7,2,6,6,2,5},
            {7,7,3,6,3,4,7,7},
        };

        public int state { get; private set; } // position of the piece in the cube (8 options)

        private const int FRONT = 0;
        private const int UP = 1;
        private const int BACK = 2;
        private const int DOWN = 3;
        private const int LEFT = 4;
        private const int RIGHT = 5;
        private const int LENGTH = 6;
        // data sctructrure
        // front, up, back, down, left, right
        // [ -1,   -1,   -1,   -1,    -1,   -1  ]
        public int[] data { get; private set; }
        private int[] m_dataAux;

        public RubickColorPiece()
        {
            data = new int[] { -1, -1, -1, -1, -1, -1 };
            m_dataAux = new int[] { -1, -1, -1, -1, -1, -1 };
            state = -1;
        }

        public void Setup(int _initialState, RubickColor _col1, RubickOrientation _pos1, RubickColor _col2, RubickOrientation _pos2, RubickColor _col3, RubickOrientation _pos3)
        {
            // set state
            state = _initialState;

            // reset data
            ClearData();
            ClearAux();

            // assign data
            data[(int)_pos1] = (int)_col1;
            data[(int)_pos2] = (int)_col2;
            data[(int)_pos3] = (int)_col3;
        }

        private void ClearData()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = -1;
            }
        }

        private void ClearAux()
        {
            for (int i = 0; i < m_dataAux.Length; i++)
            {
                m_dataAux[i] = -1;
            }
        }

        public void Transform(RubickMovementTypes _moveType)
        {
            int eventIndex = GetEventIndex(_moveType);

            int oldState = state;
            state = s_stateTrasitionTable[state, eventIndex];

            if (oldState == state)
                return;

            ClearAux();

            if (oldState == 0)
            {
                if (_moveType == RubickMovementTypes.TopTurnRight)
                {
                    m_dataAux[FRONT] = data[LEFT];
                    m_dataAux[RIGHT] = data[FRONT];
                    m_dataAux[UP] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnDown)
                {
                    m_dataAux[LEFT] = data[LEFT];
                    m_dataAux[DOWN] = data[FRONT];
                    m_dataAux[FRONT] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.TopTurnLeft)
                {
                    m_dataAux[BACK] = data[LEFT];
                    m_dataAux[LEFT] = data[FRONT];
                    m_dataAux[UP] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnUp)
                {
                    m_dataAux[LEFT] = data[LEFT];
                    m_dataAux[UP] = data[FRONT];
                    m_dataAux[BACK] = data[UP];
                }
            }
            else if (oldState == 1)
            {
                if (_moveType == RubickMovementTypes.TopTurnRight)
                {
                    m_dataAux[BACK] = data[RIGHT];
                    m_dataAux[RIGHT] = data[FRONT];
                    m_dataAux[UP] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.TopTurnLeft)
                {
                    m_dataAux[FRONT] = data[RIGHT];
                    m_dataAux[LEFT] = data[FRONT];
                    m_dataAux[UP] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.RightTurnUp)
                {
                    m_dataAux[RIGHT] = data[RIGHT];
                    m_dataAux[UP] = data[FRONT];
                    m_dataAux[BACK] = data[UP];
                }
                else if (_moveType == RubickMovementTypes.RightTurnDown)
                {
                    m_dataAux[RIGHT] = data[RIGHT];
                    m_dataAux[DOWN] = data[FRONT];
                    m_dataAux[FRONT] = data[UP];
                }
            }
            else if (oldState == 2)
            {
                if (_moveType == RubickMovementTypes.BottomTurnLeft)
                {
                    m_dataAux[FRONT] = data[RIGHT];
                    m_dataAux[LEFT] = data[FRONT];
                    m_dataAux[DOWN] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.BottomTurnRight)
                {
                    m_dataAux[BACK] = data[RIGHT];
                    m_dataAux[RIGHT] = data[FRONT];
                    m_dataAux[DOWN] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.RightTurnUp)
                {
                    m_dataAux[RIGHT] = data[RIGHT];
                    m_dataAux[UP] = data[FRONT];
                    m_dataAux[FRONT] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.RightTurnDown)
                {
                    m_dataAux[RIGHT] = data[RIGHT];
                    m_dataAux[DOWN] = data[FRONT];
                    m_dataAux[BACK] = data[DOWN];
                }
            }
            else if (oldState == 3)
            {
                if (_moveType == RubickMovementTypes.BottomTurnLeft)
                {
                    m_dataAux[LEFT] = data[FRONT];
                    m_dataAux[BACK] = data[LEFT];
                    m_dataAux[DOWN] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.BottomTurnRight)
                {
                    m_dataAux[RIGHT] = data[FRONT];
                    m_dataAux[FRONT] = data[LEFT];
                    m_dataAux[DOWN] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnUp)
                {
                    m_dataAux[UP] = data[FRONT];
                    m_dataAux[LEFT] = data[LEFT];
                    m_dataAux[FRONT] = data[DOWN];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnDown)
                {
                    m_dataAux[DOWN] = data[FRONT];
                    m_dataAux[LEFT] = data[LEFT];
                    m_dataAux[BACK] = data[DOWN];
                }
            }
            else if (oldState == 4)
            {
                if (_moveType == RubickMovementTypes.TopTurnLeft)
                {
                    m_dataAux[UP] = data[UP];
                    m_dataAux[RIGHT] = data[BACK];
                    m_dataAux[BACK] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.TopTurnRight)
                {
                    m_dataAux[UP] = data[UP];
                    m_dataAux[LEFT] = data[BACK];
                    m_dataAux[FRONT] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnUp)
                {
                    m_dataAux[BACK] = data[UP];
                    m_dataAux[DOWN] = data[BACK];
                    m_dataAux[LEFT] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnDown)
                {
                    m_dataAux[FRONT] = data[UP];
                    m_dataAux[UP] = data[BACK];
                    m_dataAux[LEFT] = data[LEFT];
                }
            }
            else if (oldState == 5)
            {
                if (_moveType == RubickMovementTypes.TopTurnLeft)
                {
                    m_dataAux[UP] = data[UP];
                    m_dataAux[RIGHT] = data[BACK];
                    m_dataAux[FRONT] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.TopTurnRight)
                {
                    m_dataAux[UP] = data[UP];
                    m_dataAux[LEFT] = data[BACK];
                    m_dataAux[BACK] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.RightTurnUp)
                {
                    m_dataAux[BACK] = data[UP];
                    m_dataAux[DOWN] = data[BACK];
                    m_dataAux[RIGHT] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.RightTurnDown)
                {
                    m_dataAux[FRONT] = data[UP];
                    m_dataAux[UP] = data[BACK];
                    m_dataAux[RIGHT] = data[RIGHT];
                }
            }
            else if (oldState == 6)
            {
                if (_moveType == RubickMovementTypes.BottomTurnLeft)
                {
                    m_dataAux[DOWN] = data[DOWN];
                    m_dataAux[RIGHT] = data[BACK];
                    m_dataAux[FRONT] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.BottomTurnRight)
                {
                    m_dataAux[DOWN] = data[DOWN];
                    m_dataAux[LEFT] = data[BACK];
                    m_dataAux[BACK] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.RightTurnUp)
                {
                    m_dataAux[FRONT] = data[DOWN];
                    m_dataAux[DOWN] = data[BACK];
                    m_dataAux[RIGHT] = data[RIGHT];
                }
                else if (_moveType == RubickMovementTypes.RightTurnDown)
                {
                    m_dataAux[BACK] = data[DOWN];
                    m_dataAux[UP] = data[BACK];
                    m_dataAux[RIGHT] = data[RIGHT];
                }
            }
            else if (oldState == 7)
            {
                if (_moveType == RubickMovementTypes.BottomTurnLeft)
                {
                    m_dataAux[DOWN] = data[DOWN];
                    m_dataAux[RIGHT] = data[BACK];
                    m_dataAux[BACK] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.BottomTurnRight)
                {
                    m_dataAux[DOWN] = data[DOWN];
                    m_dataAux[LEFT] = data[BACK];
                    m_dataAux[FRONT] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnUp)
                {
                    m_dataAux[FRONT] = data[DOWN];
                    m_dataAux[DOWN] = data[BACK];
                    m_dataAux[LEFT] = data[LEFT];
                }
                else if (_moveType == RubickMovementTypes.LeftTurnDown)
                {
                    m_dataAux[BACK] = data[DOWN];
                    m_dataAux[UP] = data[BACK];
                    m_dataAux[LEFT] = data[LEFT];
                }
            }

            Array.Copy(m_dataAux, data, LENGTH);
        }

        public object Clone()
        {
            RubickColorPiece newPiece = new RubickColorPiece();
            newPiece.state = this.state;
            Array.Copy(this.data, newPiece.data, LENGTH);
            return newPiece;
        }

        public static int GetEventIndex(RubickMovementTypes _moveType)
        {
            switch (_moveType)
            {
                case RubickMovementTypes.TopTurnRight:
                    return 0;
                case RubickMovementTypes.TopTurnLeft:
                    return 1;
                case RubickMovementTypes.BottomTurnRight:
                    return 2;
                case RubickMovementTypes.BottomTurnLeft:
                    return 3;
                case RubickMovementTypes.LeftTurnUp:
                    return 4;
                case RubickMovementTypes.LeftTurnDown:
                    return 5;
                case RubickMovementTypes.RightTurnUp:
                    return 6;
                case RubickMovementTypes.RightTurnDown:
                    return 7;
            }
            return -1;
        }
    }
}
