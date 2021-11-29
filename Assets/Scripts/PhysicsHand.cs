using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

//Code by Justin P Barnett used https://www.youtube.com/watch?v=RwGIyRy-Lss&list=WL&index=3

public class PhysicsHand : MonoBehaviour
{
    //Physics Movement
    [SerializeField] ActionBasedController controller;
    [SerializeField] float followSpeed = 30f;
    [SerializeField] float rotateSpeed = 100f;

    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    
    Vector3 velocity;
    Vector3 angularVelocity;
    
    Transform followTarget;
    Rigidbody body;

    //Contact point
    [SerializeField] Transform palm;
    [SerializeField] float reachDistance = 0.1f, jointDistance = 0.05f;
    public LayerMask grabbableLayer;

    private bool isGrabbing;
    private GameObject heldObject;
    private Transform grabPoint;
    private FixedJoint joint1, joint2;



    Quaternion Modulate360(Quaternion q)
    {
        return Quaternion.Euler(q.eulerAngles.x + (q.eulerAngles.x < 0f ? 360f : 0f),
            q.eulerAngles.y + (q.eulerAngles.y < 0f ? 360f : 0f),
            q.eulerAngles.z + (q.eulerAngles.z < 0f ? 360f : 0f));
    }

    Vector3 Modulate360(Vector3 v)
    {
        return new Vector3(v.x + (v.x < 0.0f ? 360f : 0f), v.y + (v.y < 0.0f ? 360f : 0f), v.z + (v.z < 0.0f ? 360f : 0f));
    }

    void Start()
    {
        followTarget = controller.gameObject.transform;
        //Physics Movement
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20f;
        body.maxAngularVelocity = 20f;

        //Input Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;




        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;
    }

    void Update()
    {
        ReadFollow();
    }

    void FixedUpdate()
    {

        PhysicsMove();
    }

    public void Enable(GameObject followObject)
    {
      
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void ReadFollow()
    {
        //Position;
        var positionWithOffset = followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //Rotation
        var rotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f)
        {
            angle -= 360f;
        }
        angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
    }

    private void PhysicsMove()
    {

        //Position
        body.velocity = velocity;


        //Rotation
        body.angularVelocity = angularVelocity;

        //var forwardWithOffset = Quaternion.Euler(rotationOffset) * _followTarget.up;
        //var crossProduct = Vector3.Cross(forwardWithOffset, _body.rotation * Vector3.up);

        //_body.angularVelocity = -crossProduct * rotateSpeed;
    }

    private void Grab(InputAction.CallbackContext context)
    {
        if (isGrabbing || heldObject) return;
        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, (int)grabbableLayer);
        if (grabbableColliders.Length < 1) return;

        var objectToGrab = grabbableColliders[0].transform.gameObject;
        var objectBody = objectToGrab.GetComponent<Rigidbody>();
        
        if (objectBody != null)
        {
            heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent<Rigidbody>();
            if (objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }

        StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
    }

    private IEnumerator GrabObject(Collider collider, Rigidbody objectBody)
    {
        isGrabbing = true;

        //create a grab point
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = heldObject.transform;

        //move hand to grab point
        followTarget = grabPoint;

        //wait for hand to reach grab point
        while(grabPoint != null && Vector3.Distance(grabPoint.position, palm.position) > jointDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //freeze hand and object motion
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        objectBody.velocity = Vector3.zero;
        objectBody.angularVelocity = Vector3.zero;

        objectBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        objectBody.interpolation = RigidbodyInterpolation.Interpolate;

        //attach joints
        joint1 = gameObject.AddComponent<FixedJoint>();
        joint1.connectedBody = objectBody;
        joint1.breakForce = float.PositiveInfinity;
        joint1.breakTorque = float.PositiveInfinity;

        joint1.connectedMassScale = 1;
        joint1.massScale = 1;
        joint1.enableCollision = false;
        joint1.enablePreprocessing = false;

        joint2 = heldObject.AddComponent<FixedJoint>();
        joint2.connectedBody = body;
        joint2.breakForce = float.PositiveInfinity;
        joint2.breakTorque = float.PositiveInfinity;
             
        joint2.connectedMassScale = 1;
        joint2.massScale = 1;
        joint2.enableCollision = false;
        joint2.enablePreprocessing = false;


        //reset follow target
        followTarget = controller.gameObject.transform;
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (joint1 != null)
            Destroy(joint1);

        if (joint2 != null)
            Destroy(joint2);

        if (grabPoint != null)
            Destroy(grabPoint.gameObject);

        if(heldObject != null)
        {
            var objectBody = heldObject.GetComponent<Rigidbody>();
            objectBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            objectBody.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
        }

        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }
}