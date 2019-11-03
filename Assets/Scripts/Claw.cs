using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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

        private Transform leftClaw;
        private Transform rightClaw;

        public Claw(Transform leftClaw, Transform rightClaw)
        {
            this.leftClaw = leftClaw;
            this.rightClaw = rightClaw;
        }

        public void OpenClaw()
        {
            currentClawState = eClawState.CLAW_OPENING;

            leftClaw.DOKill();
            leftClaw.DOLocalRotate(new Vector3(0f, 0f, 50f), 1f).OnComplete(OpenClawComplete);

            rightClaw.DOKill();
            rightClaw.DOLocalRotate(new Vector3(0f, 0f, -50f), 1f);
        }

        void OpenClawComplete()
        {
            currentClawState = eClawState.CLAW_OPEN;
        }

        public void CloseClaw()
        {
            currentClawState = eClawState.CLAW_CLOSING;

            leftClaw.DOKill();
            leftClaw.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f).OnComplete(CloseClawComplete);

            rightClaw.DOKill();
            rightClaw.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f);
        }

        void CloseClawComplete()
        {
            currentClawState = eClawState.CLAW_CLOSED;
        }
    }
}