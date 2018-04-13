using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IK_FABRIK_UNITY : MonoBehaviour
{
    //public const int MAX_ITERATIONS = 100;

    //public GameObject tentacleArm;
    //public Rigidbody[] tentacleChilds;

    public float maxAngleRotation = 40;

    [Space]
    public float minSwingClamp = 0.9f;
    public float maxSwingClamp = 1.1f;
    public float minTwistClamp = 0.9f;
    public float maxTwistClamp = 1.1f;
    public Cloth cloth;
    [Space]
    public Transform[] joints;
    public Transform[] originalJoints;
    public Vector3[] originalPositions;
    public Quaternion[] originalRotations;
    public Transform target;
    Transform originalTarget;
    //public int numIterations;

    private Vector3[] copy;
    private float[] distances;
    private bool done;

    public GameObject dusk;
    public PlayerController duskController;
    float tresholdCondition = 0.1f;
    public GameObject meshObject;
    public PunchContact punchContact;

    void Start()
    {
        //tentacleChilds = tentacleArm.GetComponentsInChildren<Rigidbody>();

        distances = new float[joints.Length - 1];
        copy = new Vector3[joints.Length];
        //originalPositions = new Vector3[joints.Length];
        //originalRotations = new Quaternion[joints.Length];
        //originalJoints = joints;
        //for(int i = 0; i < originalJoints.Length; i++) {
        //    //originalPositions[i] = originalJoints[i].position;
        //    originalJoints[i].position = new Vector3(joints[i].localPosition.x, joints[i].localPosition.y, joints[i].localPosition.z);
        //    originalRotations[i] = joints[i].localRotation;
        //}
        //originalTarget.position = new Vector3(target.localPosition.x, target.localPosition.y, target.localPosition.z);

    }

    //public void ResetPositions() {
    //    target.localPosition = originalTarget.position;
    //    for (int i = 0; i < originalJoints.Length; i++) {
    //        joints[i].localPosition = new Vector3(originalJoints[i].position.x, originalJoints[i].position.y, originalJoints[i].position.z);
    //        joints[i].localRotation = originalJoints[i].rotation;
    //    }

    //}

        void Update() {
        if (duskController == null) {
            duskController = dusk.GetComponent<PlayerController>();
        }

        joints[0].position = dusk.transform.position + new Vector3(0, 0.4f, 0);


        //if (!duskController.moving&&duskController.grounded) {
        // cloth.enabled = false;
        // Copy the joints positions to work with
        //TODO
        copy[0] = joints[0].position;

            for (int i = 0; i < joints.Length - 1; i++) {
                copy[i + 1] = joints[i + 1].position;
                distances[i] = (copy[i + 1] - copy[i]).magnitude;
            }
            // CALCULATE ALSO THE DISTANCE BETWEEN JOINTS

            //done = TODO
            done = (copy[copy.Length - 1] - target.position).magnitude < tresholdCondition;

            if (!done) {
                float targetRootDist = Vector3.Distance(copy[0], target.position);

                // Update joint positions
                if (targetRootDist > distances.Sum()) {
                    // The target is unreachable
                    for (int i = 0; i < copy.Length - 1; i++) {
                        float r = (target.position - copy[i]).magnitude;
                        float lambda = distances[i] / r;

                        copy[i + 1] = (1 - lambda) * copy[i] + (lambda * target.position);
                    }

                } else {
                    Vector3 b = copy[0];

                    // The target is reachable
                    //while (TODO)

                    float difference = (copy[copy.Length - 1] - target.position).magnitude;

                    while (difference > tresholdCondition) // treshold = tolerance
                    {
                        // numIterations++;

                        // STAGE 1: FORWARD REACHING
                        //TODO
                        copy[copy.Length - 1] = target.position;

                        for (int i = copy.Length - 2; i > 0; i--) {
                            float r = (copy[i + 1] - copy[i]).magnitude;
                            float lambda = distances[i] / r;

                            copy[i] = (1 - lambda) * copy[i + 1] + lambda * copy[i];



                        }

                        // STAGE 2: BACKWARD REACHING
                        //TODO
                        copy[0] = b;

                        for (int i = 0; i < copy.Length - 1; i++) {
                            float r = (copy[i + 1] - copy[i]).magnitude;
                            float lambda = distances[i] / r;

                            copy[i + 1] = (1 - lambda) * copy[i] + lambda * copy[i + 1];
                        }

                        difference = (copy[copy.Length - 1] - target.position).magnitude;
                    }
                }

                // Update original joint rotations
                for (int i = 0; i <= joints.Length - 2; i++) {
                    // float originalAngle = joints[i].rotation.w;

                    //TODO
                    // Rotation
                    Vector3 vectorA = joints[i + 1].position - joints[i].position;
                    Vector3 vectorB = copy[i + 1] - copy[i];

                    // float angle = Mathf.Acos(Vector3.Dot(vectorA.normalized, vectorB.normalized)) * Mathf.Rad2Deg;
                    float cosA = (Vector3.Dot(vectorA.normalized, vectorB.normalized));
                    float sinA = Vector3.Cross(vectorA.normalized, vectorB.normalized).magnitude;

                    // Atan = Cos | Atan2 = denominador y...
                    float angle = Mathf.Atan2(sinA, cosA) * Mathf.Rad2Deg;

                    Vector3 axis = Vector3.Cross(vectorA, vectorB).normalized;

                    joints[i].rotation = Quaternion.AngleAxis(angle, axis) * joints[i].rotation;


                    //1. decompose twist and swing
                    Quaternion twist = getTwist(joints[i], joints[i + 1]);
                    Quaternion swing = getSwing(joints[i], joints[i + 1]);

                    //test A: make sure that recomposing twist and swing gives the original rotation

                    float angleTest = Quaternion.Angle(joints[i + 1].rotation, joints[i].rotation);

                    if (Mathf.Abs(angleTest) > maxAngleRotation) {
                        // float clampedTwist = Mathf.Clamp(twist.w, minTwistClamp, maxTwistClamp);
                        // twist = new Quaternion(twist.x, twist.y, twist.z, clampedTwist);

                        joints[i + 1].rotation = joints[i].rotation;
                    }

                    //Debug.Log("Before: " + twist);

                    //2. clamp twist (or even cancel)
                    // float clampedTwist = Mathf.Clamp(twist.w, minTwistClamp, maxTwistClamp);
                    // twist = new Quaternion(twist.x, twist.y, twist.z, clampedTwist);

                    //Debug.Log("After: " + twist);

                    //3. clamp swing
                    // float clampedSwing = Mathf.Clamp(swing.w, minSwingClamp, maxSwingClamp);
                    // swing = new Quaternion(swing.x, swing.y, swing.z, clampedSwing);



                    //4. recompose rotation from (clamped) twist and (optionally clamped) swing
                    // joints[i].rotation = twist * swing;


                    //if(getTwist(joints[i-1], joints[i]).w )

                    //float clampedRotation = Mathf.Clamp(joints[i].localRotation.w, 0.9f, 1.1f);

                    //joints[i].localRotation = new Quaternion(joints[i].localRotation.x, joints[i].localRotation.y, joints[i].localRotation.z, clampedRotation);

                    joints[i + 1].position = copy[i + 1];

                }
                // Debug.Log(getTwist(joints[20], joints[21]));
                // Debug.Log(joints[20].localRotation.w);

            }
        //} else {
        //    cloth.enabled = true;


        //}
    }

    Quaternion getTwist(Transform _parent, Transform _selfJoint)
    {
        Quaternion localRotation = Quaternion.Inverse(_parent.rotation) * _selfJoint.rotation;
        // this is the same than 
        //Quaternion localRotation = transform.localRotation;

        Quaternion twist = new Quaternion(localRotation.x, localRotation.y * 0, localRotation.z * 0, localRotation.w);

        float twistModulus = Mathf.Sqrt(Mathf.Pow(twist.x, 2) + Mathf.Pow(twist.y, 2) + Mathf.Pow(twist.z, 2) + Mathf.Pow(twist.w, 2));
        twist = new Quaternion(twist.x / twistModulus, twist.y / twistModulus, twist.z / twistModulus, twist.w / twistModulus);

        return twist;
    }

    Quaternion getSwing(Transform _parent, Transform _selfJoint)
    {
        Quaternion swing = new Quaternion();
        Quaternion localRotation = Quaternion.Inverse(_parent.rotation) * _selfJoint.rotation;
        Quaternion twistConjugated = new Quaternion(getTwist(_parent, _selfJoint).x, getTwist(_parent, _selfJoint).y, getTwist(_parent, _selfJoint).z, getTwist(_parent, _selfJoint).w);

        twistConjugated = Quaternion.Inverse(twistConjugated);

        swing = localRotation * twistConjugated;

        return swing;
    }

    //void LateUpdate()
    //{
    //    for (int i = 0; i < joints.Length - 2; i++)
    //    {
    //        joints[i].localRotation = getSwing(joints[i + 1], joints[i]);
    //    }
    //}
}
