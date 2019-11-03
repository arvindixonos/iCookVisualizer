using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomIKSolver : MonoBehaviour {

    //public string _name; // this is just a label to tell what solver this is if you have multiple on the same game object
    //[Space(25)]
    public string label = "Enter Limb Name";

    public List<CustomIKJoint >Joints; // list of joints that are part of the calculation for IK

    public CustomIKJoint Ankle; // foot end effector location
    public Transform Target; // where the IK is pointing at

    public int Itterations = 3; // how many times the IK is calculated per frame

    public bool Visualise;

    private void LateUpdate()
    {
        SolveIK(Target.position);
    }

    public void SolveIK(Vector3 target)
    {
        for (int k = 0; k < Itterations; k++)
        {
            for (int i = 0; i < Joints.Count; i++)
            {
                Vector3 p = Ankle.transform.position;
                Vector3 j = Joints[i].transform.position;
                Quaternion r = Joints[i].transform.rotation;

                Quaternion rotTarget = Quaternion.FromToRotation(p - j, target - j) * r;

                Joints[i].transform.rotation = Quaternion.Slerp(Joints[i].transform.rotation, rotTarget, (float)(i + 1) / (float)Joints.Count);
            }
        }
    }
}
