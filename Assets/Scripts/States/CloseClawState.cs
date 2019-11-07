using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class CloseClawState : State
    {
        public CloseClawState(System.Object payload = null) : base(payload)
        {

        }

        public override void EnterState()
        {
            iCookHand.OnClawStateChanged += ClawStateChanged;
            iCookHand.Instance.CloseClaw();
        }

        public void ClawStateChanged(eClawState clawState)
        {
            if (clawState == eClawState.CLAW_CLOSED)
            {
                if(payload != null)
                {
                    RackSlot rackSlot = (RackSlot)payload;
                    rackSlot.AttachToClaw();
                }

                OnStateComplete();
            }
        }

        public override void ExitState()
        {
            iCookHand.OnClawStateChanged -= ClawStateChanged;
        }

        public override void UpdateState()
        {

        }
    }
}