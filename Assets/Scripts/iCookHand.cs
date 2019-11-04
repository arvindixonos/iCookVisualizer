using System;
using UnityEngine;

namespace iCook
{
    public class iCookHand : Singleton<iCookHand>
    {
        private int numJoints = 0;
        public Joint[] joints;

        public Action<eJointType, float> OnAngleChanged;

        public static Action<eClawState> OnClawStateChanged;

        public Transform leftClaw;
        public Transform rightClaw;
        private Claw claw;

        public void Start()
        {
            InitializeHand();
        }

        public void InitializeHand()
        {
            numJoints = joints.Length;

            for (int i = 0; i < numJoints; i++)
            {
                joints[i].InitJoint();
                joints[i].OnAngleChanged += JointAngleChanged;
            }

            claw = new Claw(leftClaw, rightClaw);
            claw.OnClawStateChanged += ClawStateChanged;
        }

        public void JointAngleChanged(eJointType jointType, float currentAngle)
        {
            OnAngleChanged?.Invoke(jointType, currentAngle);
        }

        public void OpenClaw()
        {
            claw.OpenClaw();
        }

        public void CloseClaw()
        {
            claw.CloseClaw();
        }

        public void ClawStateChanged(eClawState clawState)
        {
            OnClawStateChanged?.Invoke(clawState);
        }

        public void UpdateHand()
        {
            for (int i = 0; i < numJoints; i++)
            {
                if(joints[i] != null)
                {
                    joints[i].UpdateJoint();
                }
            }
        }
    }
}

