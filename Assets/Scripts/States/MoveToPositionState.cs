using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class MoveToPositionState : State
    {
        private Transform targetPosition = null;

        public MoveToPositionState(System.Object payload):base(payload)
        {

        }

        public override void EnterState()
        {
            targetPosition = iCookVisualizer.Instance.GetTargetPosition(ePositionType.POSITION_IDLE) as Transform;
            iCookVisualizer.Instance.SetHandTarget(targetPosition);
        }

        public override void ExitState()
        {
           
        }

        public override void UpdateState()
        {
            float magnitude = Vector3.Magnitude(targetPosition.position - iCookVisualizer.Instance.clawTip.position);

            if(magnitude < 0.01f)
            {
                OnStateComplete();
            }
        }
    }
}
