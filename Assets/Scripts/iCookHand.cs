using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class iCookHand : Singleton<MonoBehaviour>
    {
        private int numJoints = 0;
        public Joint[] joints;

        public Action<eJointType, float> OnAngleChanged;

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
        }
    }
}

