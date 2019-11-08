using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using DG.Tweening;

namespace iCook
{
    public class StirState : State
    {
        private Spline stirSpline;
        private GameObject moverObject = null;
        private SplineFollower splineFollower = null;
        private eStirPattern stirPattern;

        public StirState(System.Object payload = null) : base(payload)
        {
            stirPattern = StirManager.Instance.GetStirPattern((string)payload);

            stirSpline = StirManager.Instance.GetStirSpline(stirPattern);

            splineFollower = new SplineFollower();
            splineFollower.percentage = 0f;

            moverObject = new GameObject("Spline Mover");

            splineFollower.target = moverObject.transform;

            stirSpline.followers = new SplineFollower[] { splineFollower };
        }

        public override void EnterState()
        {
            iCookVisualizer.Instance.SetHandTarget(moverObject.transform);

            DOTween.To(() => splineFollower.percentage, x => splineFollower.percentage = x, 1f, 3f).SetEase(Ease.Linear).SetLoops(5, LoopType.Restart).OnComplete(StirComplete);
        }

        public void StirComplete()
        {
            OnStateComplete();
        }

        public override void ExitState()
        {
            stirSpline.followers = null;
            GameObject.Destroy(moverObject);
        }

        public override void UpdateState()
        {
            
        }
    }
}
