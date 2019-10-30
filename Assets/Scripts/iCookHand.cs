using System;
using UnityEngine;
using BioIK;

namespace iCook
{
    public class iCookHand : Singleton<iCookHand>
    {
        private BioIK.BioIK bioIK;
        private int numJoints = 0;
        public Joint[] joints;

        public Action<eJointType, float> OnAngleChanged;

        public string rightClawName = "";
        public string leftClawName = "";
        private Claw claw;
        public float clawTestAngle = 0f;

        public BioIK.BioIK GetHand()
        {
            return bioIK;
        }

        private void OnEnable()
        {
            bioIK = GetComponent<BioIK.BioIK>();
        }

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

            claw = new Claw(bioIK, rightClawName, leftClawName);
        }

        public void JointAngleChanged(eJointType jointType, float currentAngle)
        {
            OnAngleChanged?.Invoke(jointType, currentAngle);
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

            claw.SetClawAngle(clawTestAngle);
        }
    }
}

