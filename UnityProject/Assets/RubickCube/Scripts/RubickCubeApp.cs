using UnityEngine;
using Uag.AI.Common.MVC;
using System;

namespace Uag.AI.RubickCube
{
    public class RubickCubeApp : AppBase
    {
        public static new RubickCubeApp instance { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            instance = this;

            RubickMatrix cube = new RubickMatrix();
            Debug.Log("Initial State");
            Debug.Log(cube);
            cube.Shuffle();
            Debug.Log(cube);
            cube.Revert();
            Debug.Log(cube);
        }

        protected override ControllerBase CreateController()
        {
            return new RubickCubeController();
        }

        protected override ModelBase CreateModel()
        {
            return new RubickCubeModel();
        }

        protected override ViewBase CreateView()
        {
            return FindObjectOfType<RubickCubeView>();
        }
    }
}
