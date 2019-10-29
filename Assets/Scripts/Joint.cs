using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum eJointType
    {
        JOINT_1,
        JOINT_2,
        JOINT_3,
        JOINT_4,
        JOINT_5,
        JOINT_6
    }

    public enum eJointAxis
    {
        JOINT_AXIS_X,
        JOINT_AXIS_Y,
        JOINT_AXIS_Z
    }

    [System.Serializable]
    public class Joint
    {
        public eJointType jointType = eJointType.JOINT_1;
        public Transform targetTransform;

        public Action<eJointType, float> OnAngleChanged;

        public eJointAxis jointAxis;
        private float currentAngle = 0f;

        public void InitJoint()
        {
            this.currentAngle = GetCurrentAngle();
        }

        public float GetCurrentAngle()
        {
            float angle = 0f;

            switch(jointAxis)
            {
                case eJointAxis.JOINT_AXIS_X:
                    angle = targetTransform.localEulerAngles.x;
                    break;

                case eJointAxis.JOINT_AXIS_Y:
                    angle = targetTransform.localEulerAngles.y;
                    break;

                case eJointAxis.JOINT_AXIS_Z:
                    angle = targetTransform.localEulerAngles.z;
                    break;
            }

            angle = (angle > 180f) ? angle - 360f : angle;

            return angle;
        }

        public void UpdateJoint()
        {
            float angleNow = GetCurrentAngle();

            if(Mathf.Abs(currentAngle - angleNow) > 0.1f)
            {
                currentAngle = angleNow;

                Debug.Log("Angle Changed for " + jointType + " " + angleNow);

                OnAngleChanged.Invoke(jointType, currentAngle);
            }
        }
    }
}