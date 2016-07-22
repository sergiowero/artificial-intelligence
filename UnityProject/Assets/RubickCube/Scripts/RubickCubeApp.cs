using UnityEngine;
using Uag.AI.Common.MVC;
using System;

namespace Uag.AI.RubickCube
{
    public class RubickCubeApp : AppBase
    {
        public static new RubickCubeApp instance { get; protected set; }

        public new RubickCubeController controller
        {
            get
            {
                return (RubickCubeController)base.controller;
            }
        }

        [Header("Config")]
        public int shuffleSteps = 100;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
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
