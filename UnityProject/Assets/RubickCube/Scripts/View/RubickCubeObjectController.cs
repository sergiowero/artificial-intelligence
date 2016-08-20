using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Uag.AI.RubickCube
{

    public class RubickCubeObjectController : MonoBehaviour
    {
        public Transform centerPivot;

        public Transform[] pieces;

        [HideInInspector]
        public List<RubickMovementTypes> movementQueue;

        private Transform[] m_initialPieces;
        private Vector3[] m_initialPositions;
        private Quaternion[] m_initialRotations;

        private float m_elapsedTime = 0;

        // Use this for initialization
        void Start()
        {

            m_initialPositions = new Vector3[pieces.Length];
            m_initialRotations = new Quaternion[pieces.Length];
            m_initialPieces = new Transform[pieces.Length];
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].SetParent(null);
                m_initialPieces[i] = pieces[i];
                m_initialPositions[i] = pieces[i].localPosition;
                m_initialRotations[i] = pieces[i].localRotation;
            }
        }

        public void SetState(List<RubickMovementTypes> _moves)
        {
            ResetCube();
            movementQueue.Clear();
            _moves.ForEach(m => ApplyMove(m));
        }

        public void SetSolution(List<RubickMovementTypes> _moves)
        {
            movementQueue.Clear();
            movementQueue.AddRange(_moves);
        }

        #region Test Movements

        [ContextMenu("TopTurnRight")]
        public void TopTurnRight()
        {
            ApplyMove(RubickMovementTypes.TopTurnRight);
        }

        [ContextMenu("TopTurnLeft")]
        public void TopTurnLeft()
        {
            ApplyMove(RubickMovementTypes.TopTurnLeft);
        }

        [ContextMenu("LeftTurnDown")]
        public void LeftTurnDown()
        {
            ApplyMove(RubickMovementTypes.LeftTurnDown);
        }
        #endregion

        public void ApplyMove(RubickMovementTypes _move, bool animated = false)
        {
            Transform t = null;
            switch (_move)
            {
                case RubickMovementTypes.TopTurnRight:
                    if (!animated)
                        TurnRight(pieces[0], pieces[1], pieces[4], pieces[5]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(0, -90, 0), pieces[0], pieces[1], pieces[4], pieces[5]));
                    t = pieces[0];
                    pieces[0] = pieces[4];
                    pieces[4] = pieces[5];
                    pieces[5] = pieces[1];
                    pieces[1] = t;
                    break;
                case RubickMovementTypes.TopTurnLeft:
                    if (!animated)
                        TurnLeft(pieces[0], pieces[1], pieces[4], pieces[5]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(0, 90, 0), pieces[0], pieces[1], pieces[4], pieces[5]));
                    t = pieces[0];
                    pieces[0] = pieces[1];
                    pieces[1] = pieces[5];
                    pieces[5] = pieces[4];
                    pieces[4] = t;
                    break;
                case RubickMovementTypes.BottomTurnRight:
                    if (!animated)
                        TurnRight(pieces[3], pieces[2], pieces[7], pieces[6]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(0, -90, 0), pieces[3], pieces[2], pieces[7], pieces[6]));
                    t = pieces[3];
                    pieces[3] = pieces[7];
                    pieces[7] = pieces[6];
                    pieces[6] = pieces[2];
                    pieces[2] = t;
                    break;
                case RubickMovementTypes.BottomTurnLeft:
                    if (!animated)
                        TurnLeft(pieces[3], pieces[2], pieces[7], pieces[6]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(0, 90, 0), pieces[3], pieces[2], pieces[7], pieces[6]));
                    t = pieces[3];
                    pieces[3] = pieces[2];
                    pieces[2] = pieces[6];
                    pieces[6] = pieces[7];
                    pieces[7] = t;
                    break;
                case RubickMovementTypes.LeftTurnUp:
                    if (!animated)
                        TurnUp(pieces[0], pieces[3], pieces[7], pieces[4]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(90, 0, 0), pieces[0], pieces[3], pieces[7], pieces[4]));
                    t = pieces[0];
                    pieces[0] = pieces[3];
                    pieces[3] = pieces[7];
                    pieces[7] = pieces[4];
                    pieces[4] = t;
                    break;
                case RubickMovementTypes.LeftTurnDown:
                    if (!animated)
                        TurnDown(pieces[0], pieces[3], pieces[7], pieces[4]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(-90, 0, 0), pieces[0], pieces[3], pieces[7], pieces[4]));
                    t = pieces[0];
                    pieces[0] = pieces[4];
                    pieces[4] = pieces[7];
                    pieces[7] = pieces[3];
                    pieces[3] = t;
                    break;
                case RubickMovementTypes.RightTurnUp:
                    if (!animated)
                        TurnUp(pieces[1], pieces[2], pieces[6], pieces[5]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(90, 0, 0), pieces[1], pieces[2], pieces[6], pieces[5]));
                    t = pieces[1];
                    pieces[1] = pieces[2];
                    pieces[2] = pieces[6];
                    pieces[6] = pieces[5];
                    pieces[5] = t;
                    break;
                case RubickMovementTypes.RightTurnDown:
                    if (!animated)
                        TurnDown(pieces[1], pieces[2], pieces[6], pieces[5]);
                    else
                        StartCoroutine(TurnAnimated(new Vector3(-90, 0, 0), pieces[1], pieces[2], pieces[6], pieces[5]));
                    t = pieces[1];
                    pieces[1] = pieces[5];
                    pieces[5] = pieces[6];
                    pieces[6] = pieces[2];
                    pieces[2] = t;
                    break;
                default:
                    break;
            }
        }

        public void TurnRight(params Transform[] pieces)
        {
            centerPivot.localRotation = Quaternion.identity;
            Attach(pieces);
            centerPivot.Rotate(0, -90, 0);
            Dettach(pieces);
            //TurnAnimated(new Vector3(0, -90, 0), pieces);
        }

        public IEnumerator TurnAnimated(Vector3 rotation, params Transform[] pieces)
        {
            float duration = 0.25f;
            centerPivot.localRotation = Quaternion.identity;
            Attach(pieces);
            Quaternion start = centerPivot.rotation;
            centerPivot.Rotate(rotation);
            Quaternion end = centerPivot.rotation;
            centerPivot.rotation = start;
            float elapsedTime = 0.0f;
            while (elapsedTime <= duration)
            {
                centerPivot.rotation = Quaternion.Lerp(start, end, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            centerPivot.rotation = end;
            Dettach(pieces);
            yield return null;
        }

        public void TurnLeft(params Transform[] pieces)
        {
            centerPivot.localRotation = Quaternion.identity;
            Attach(pieces);
            centerPivot.Rotate(0, 90, 0);
            Dettach(pieces);
        }

        public void TurnUp(params Transform[] pieces)
        {
            centerPivot.localRotation = Quaternion.identity;
            Attach(pieces);
            centerPivot.Rotate(90, 0, 0);
            Dettach(pieces);
        }

        public void TurnDown(params Transform[] pieces)
        {
            centerPivot.localRotation = Quaternion.identity;
            Attach(pieces);
            centerPivot.Rotate(-90, 0, 0);
            Dettach(pieces);
        }

        private void Attach(params Transform[] pieces)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].SetParent(centerPivot);
            }
        }

        private void Dettach(params Transform[] pieces)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].SetParent(null);
            }
        }

        [ContextMenu("ResetCube")]
        public void ResetCube()
        {
            m_elapsedTime = 0f;
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i] = m_initialPieces[i];
                pieces[i].localPosition = m_initialPositions[i];
                pieces[i].localRotation = m_initialRotations[i];
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (movementQueue.Count > 0)
            {
                m_elapsedTime += Time.deltaTime;
                if (m_elapsedTime > 0.3f)
                {
                    m_elapsedTime -= 0.3f;
                    var m = movementQueue[0];
                    //Debug.Log(m);
                    ApplyMove(m, true);
                    movementQueue.RemoveAt(0);
                }
            }
        }

        private void AnimatemMovement(RubickMovementTypes _move)
        {

        }
    }

}
