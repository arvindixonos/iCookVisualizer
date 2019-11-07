using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class OpenClawState : State
    {
        public OpenClawState(System.Object payload = null) : base(payload)
        {
           
        }

        public override void EnterState()
        {
            iCookHand.OnClawStateChanged += ClawStateChanged;
            iCookHand.Instance.OpenClaw();
        }

        public void ClawStateChanged(eClawState clawState)
        {
            if(clawState == eClawState.CLAW_OPEN)
            {
                RackSlot rackSlot = (RackSlot)payload;
                rackSlot.DetchFromClaw();
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