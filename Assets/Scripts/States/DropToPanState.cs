using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class DropToPanState : State
    {
        private bool dropped = false;

        public DropToPanState(System.Object payload = null) :base(payload)
        {
            dropped = false;
        }

        public override void EnterState()
        {
            iCookVisualizer.Instance.OnClawRotated += DropComplete;

            iCookVisualizer.Instance.RotateClaw(90f);
        }

        void DropComplete(float clawAngle)
        {
            if(dropped)
            {
                OnStateComplete();

                return;
            }

            dropped = true;

            iCookVisualizer.Instance.RotateClaw(-90f);
        }

        public override void ExitState()
        {
            iCookVisualizer.Instance.OnClawRotated -= DropComplete;
        }

        public override void UpdateState()
        {

        }
    }
}

