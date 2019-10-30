using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioIK;


namespace iCook
{
    public class Claw 
    {
        public enum eClawState
        {
            CLAW_CLOSED,
            CLAW_OPEN,
            CLAW_CLOSING,
            CLAW_OPENING
        }

        public eClawState currentClawState = eClawState.CLAW_CLOSED;

        private BioSegment rightClawSegment;
        private BioJoint rightClawJoint;
        private BioSegment leftClawSegment;
        private BioJoint leftClawJoint;

        public Claw(BioIK.BioIK bioIK, string rightClawName, string leftClawName)
        {
            rightClawSegment = bioIK.FindSegment(rightClawName);
            rightClawJoint = rightClawSegment.Joint;

            leftClawSegment = bioIK.FindSegment(leftClawName);
            leftClawJoint = leftClawSegment.Joint;
        }

        public void SetClawAngle(float angle)
        {
            angle = Mathf.Clamp(angle, 0f, 50f);

            rightClawJoint.Y.SetTargetValue(angle);
            leftClawJoint.Y.SetTargetValue(-angle);
        }
    }
}