using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] Transform look_at;
    [Header("MovementParameters")]
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;

    float maxMoveSpeedSqr;
    Rigidbody rb;
    Vector2 movementInputVector;

    void Start()
    {
        maxMoveSpeedSqr = maxMoveSpeed * maxMoveSpeed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        velocity += Quaternion.Euler(0f, look_at.rotation.eulerAngles.y, 0f) * new Vector3(movementInputVector.x, 0.0f, movementInputVector.y);
        if (new Vector2(velocity.x, velocity.z).sqrMagnitude > maxMoveSpeedSqr)
        {
            velocity = new Vector3(velocity.x, 0f, velocity.z).normalized * maxMoveSpeed + new Vector3(0f, velocity.y);
        }
        rb.velocity = velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInputVector = context.ReadValue<Vector2>();
    }
}
