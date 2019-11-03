using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace iCook
{
    public class MoveToPositionState : State
    {
        private Transform targetPosition = null;
        private GameObject moverObject = null;

        public MoveToPositionState(System.Object payload):base(payload)
        {
            targetPosition = payload as Transform;
        }

        public override void EnterState()
        {
            moverObject = new GameObject("Mover");
            moverObject.transform.position = iCookVisualizer.Instance.clawTip.position;

            moverObject.transform.DOKill();
            moverObject.transform.DOMove(targetPosition.position, 2f);

            iCookVisualizer.Instance.SetHandTarget(moverObject.transform);
        }

        public override void ExitState()
        {
            GameObject.Destroy(moverObject);
        }

        public override void UpdateState()
        {
            float magnitude = Vector3.Magnitude(targetPosition.position - iCookVisualizer.Instance.clawTip.position);

            if(magnitude < 0.1f)
            {
                OnStateComplete();
            }
        }
    }
}
