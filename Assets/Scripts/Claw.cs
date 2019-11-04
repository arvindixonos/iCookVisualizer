using System;
using UnityEngine;
using DG.Tweening;


namespace iCook
{
    public enum eClawState
    {
        CLAW_CLOSED,
        CLAW_OPEN,
        CLAW_CLOSING,
        CLAW_OPENING
    }

    public class Claw 
    {
        public eClawState currentClawState = eClawState.CLAW_CLOSED;

        public Action<eClawState> OnClawStateChanged;

        private Transform leftClaw;
        private Transform rightClaw;

        public Claw(Transform leftClaw, Transform rightClaw)
        {
            this.leftClaw = leftClaw;
            this.rightClaw = rightClaw;
        }

        public void OpenClaw()
        {
            SetClawState(eClawState.CLAW_OPENING);

            leftClaw.DOKill();
            leftClaw.DOLocalRotate(new Vector3(0f, 0f, -50f), 1f).OnComplete(OpenClawComplete);

            rightClaw.DOKill();
            rightClaw.DOLocalRotate(new Vector3(0f, 0f, 50f), 1f);
        }

        void OpenClawComplete()
        {
            SetClawState(eClawState.CLAW_OPEN);
        }

        public void CloseClaw()
        {
            SetClawState(eClawState.CLAW_CLOSING);

            leftClaw.DOKill();
            leftClaw.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f).OnComplete(CloseClawComplete);

            rightClaw.DOKill();
            rightClaw.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f);
        }

        void CloseClawComplete()
        {
            SetClawState(eClawState.CLAW_CLOSED);
        }

        public void SetClawState(eClawState clawState)
        {
            currentClawState = clawState;

            if(OnClawStateChanged != null)
            {
                OnClawStateChanged(currentClawState);
            }
        }
    }
}