using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Justin P Barnett used https://www.youtube.com/watch?v=RwGIyRy-Lss&list=WL&index=3

public class PhysicsHand : MonoBehaviour
{
    //Physics Movement
    [SerializeField] GameObject followObject;
    [SerializeField] float followSpeed = 30f;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    Vector3 velocity;
    Vector3 angularVelocity;
    Transform _followTarget;
    Rigidbody _body;

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
        _followTarget = followObject.transform;
        //Physics Movement
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;
        
        //Teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

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
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
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
        _body.velocity = velocity;


        //Rotation
        _body.angularVelocity = angularVelocity;

        //var forwardWithOffset = Quaternion.Euler(rotationOffset) * _followTarget.up;
        //var crossProduct = Vector3.Cross(forwardWithOffset, _body.rotation * Vector3.up);

        //_body.angularVelocity = -crossProduct * rotateSpeed;
    }
}
