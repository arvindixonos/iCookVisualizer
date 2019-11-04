using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace iCook
{
    public class MoveToPositionState : State
    {
        private Transform targetTransform = null;
        private GameObject moverObject = null;
        private Vector3 targetPosition = Vector3.zero;

        public MoveToPositionState(System.Object payload):base(payload)
        {
            targetTransform = payload as Transform;
            targetPosition = targetTransform.position;
        }

        public override void EnterState()
        {
            moverObject = new GameObject();
            moverObject.transform.position = iCookVisualizer.Instance.clawTip.position;

            moverObject.transform.DOKill();
            moverObject.transform.DOMove(targetPosition, 2f).OnComplete(StateComplete);

            iCookVisualizer.Instance.SetHandTarget(moverObject.transform);
        }

        void StateComplete()
        {
            OnStateComplete();
        }

        public override void ExitState()
        {
            GameObject.Destroy(moverObject);
        }

        public override void UpdateState()
        {
         
        }
    }
}
