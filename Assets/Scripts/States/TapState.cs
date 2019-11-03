using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace iCook
{
    public struct TapInfo
    {
        public int tapCount;
        public Action OnTapComplete;
    }

    public class TapState : State
    {
        private int tapCount;
        public Action OnTapComplete;
        public GameObject targetTapObject;

        public TapState(System.Object payload):base(payload)
        {
            TapInfo tapInfo = (TapInfo)payload;
            tapCount = tapInfo.tapCount;
            OnTapComplete = tapInfo.OnTapComplete;

            targetTapObject = new GameObject("Target Tap Object");
        }

        public override void EnterState()
        {
            Transform clawTip = iCookVisualizer.Instance.clawTip;

            targetTapObject.transform.position = clawTip.position;
            targetTapObject.transform.rotation = clawTip.rotation;

            iCookVisualizer.Instance.SetHandTarget(targetTapObject.transform);

            targetTapObject.transform.DOKill();
            targetTapObject.transform.DOBlendableLocalMoveBy(new Vector3(0f, -0.5f, 0f), 1f).SetLoops(tapCount, LoopType.Yoyo)
                .OnStepComplete(StepComplete).OnComplete(StateComplete);
        }

        void StateComplete()
        {
            OnStateComplete();
        }

        void StepComplete()
        {
            OnTapComplete();
        }

        public override void ExitState()
        {
            GameObject.Destroy(targetTapObject);
        }

        public override void UpdateState()
        {
            
        }
    }
}
