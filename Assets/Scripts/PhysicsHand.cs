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
    Transform _followTarget;
    Rigidbody _body;

    void Start()
    {
        //Physics Movement
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
    }

    void Update()
    {

        PhysicsMove();
    }

    public void Enable(GameObject followObject)
    {
        _followTarget = followObject.transform;
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void PhysicsMove()
    {
        //Position
        var positionWithOffset = _followTarget.position + positionOffset;
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);


        //Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
    }
}
