using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] Transform look_at;
    [Header("MovementParameters")]
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;

    float maxMoveSpeedSqr;
    [System.NonSerialized] public Rigidbody rb;
    public GameObject body;
    Vector2 movementInputVector;
    private bool has_input_move;

    //jumping
    [System.NonSerialized] bool is_grounded = true;
    [System.NonSerialized] bool readyToJump = true;
    float jumpForce = 5.0f;
    float jumpCooldown = 0.1f;
    private bool has_input_jump;
    private bool can_update_grounded = true;

    public BoxCollider gnome_collider;
    [System.NonSerialized] public float gnome_scale_factor;
    [System.NonSerialized] public float gnome_height, gnome_lenght, gnome_width;



    //being grabbed
    [System.NonSerialized] public bool is_grabbed;
    private bool rotation_reset;
    [SerializeField] int breakFreeCount;
    [SerializeField] float breakFreeWindow;
    //List<float> breakAttempts = new List<float>();
    int breakAttempts = 0;
    [SerializeField] PhysicsHand left_hand, right_hand;
    PhysicsHand[] hands;

    [System.NonSerialized] public bool is_thrown = false;

    //animation
    public Animator animator;
    private float gnome_forward_velocity, gnome_horizontal_velocity;

    //particles
    [SerializeField] ParticleSystem ps_struggle, ps_escape;

    void Start()
    {
        maxMoveSpeedSqr = maxMoveSpeed * maxMoveSpeed;
        rb = GetComponent<Rigidbody>();

        hands = new PhysicsHand[2] { left_hand, right_hand };
    }

    private void Awake()
    {
        gnome_scale_factor = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        gnome_height = gnome_collider.size.y * gnome_scale_factor;
        gnome_lenght = gnome_collider.size.z * gnome_scale_factor;
        gnome_width = gnome_collider.size.x * gnome_scale_factor;
    }

    // Update is called once per frame
    private void Update()
    {
        if (can_update_grounded)
        {
            checkGrounded();
        }
    }

    void FixedUpdate()
    {
        if(is_thrown)
        {
            is_thrown = !is_grounded;
            return;
        }

        Vector3 velocity = rb.velocity;
        velocity += Quaternion.Euler(0f, look_at.rotation.eulerAngles.y, 0.0f) * new Vector3(movementInputVector.x, 0.0f, movementInputVector.y) * acceleration * Time.deltaTime;

        //max speed
        if (new Vector2(velocity.x, velocity.z).sqrMagnitude > maxMoveSpeedSqr)
        {
            velocity = new Vector3(velocity.x, 0f, velocity.z).normalized * maxMoveSpeed + new Vector3(0f, velocity.y);
        }
        rb.velocity = velocity;
        if(!is_grabbed)
        {
            body.transform.rotation = Quaternion.Euler(0f, look_at.rotation.eulerAngles.y, 0.0f);
        }

        //animator stuff
        gnome_forward_velocity = Vector3.Dot(rb.velocity, body.transform.forward / maxMoveSpeed);
        animator.SetFloat("Velocity", gnome_forward_velocity);

        gnome_horizontal_velocity = Vector3.Dot(rb.velocity, body.transform.right / maxMoveSpeed);
        animator.SetFloat("SideVelocity", gnome_horizontal_velocity);
        if (gnome_forward_velocity < 0.001 && gnome_forward_velocity > -0.001f)
        {
            animator.SetFloat("Velocity", 0);
        }
        if (gnome_horizontal_velocity < 0.001 && gnome_horizontal_velocity > -0.001f)
        {
            animator.SetFloat("SideVelocity", 0);
        }

        //decceleration
        if (has_input_move == false && has_input_jump == false && is_grounded)
        {
            rb.velocity = rb.velocity * decceleration * Time.deltaTime;
        }

        //slow fall

        if (rb.velocity.y < -1.4f)
        {
            rb.velocity = new Vector3(rb.velocity.x, -1.4f, rb.velocity.z);
        }


        //if (is_grounded && rb.velocity.y < 0)
        //{
        //    animator.SetBool("Grounded", true);
        //}
        //else if (!is_grounded /*&& rb.velocity.y > 0*/)
        //{
        //    animator.SetBool("Grounded", false);
        //}

        //if (look_at.rotation.z != 0.0f)
        //{
        //    look_at.rotation = Quaternion.Euler(look_at.rotation.x, look_at.rotation.y, 0.0f);
        //}


        if(is_grabbed)
        {
            animator.SetBool("isGrabbed", true);
            rotation_reset = false;
            if (rb.constraints != RigidbodyConstraints.None)
            {
                rb.constraints = RigidbodyConstraints.None;
            }
        }
        else if (!is_grabbed && !rotation_reset)
        {
            resetRotation();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInputVector = context.ReadValue<Vector2>();
        has_input_move = context.performed;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        has_input_jump = context.performed;
        //clicked jump
        if (context.performed)
        {
            if (is_grabbed)
            {
                breakAttempts++;
                ps_struggle.Play();
                //breakAttempts.Add(Time.time);
                //int timesToRemove = 0;
                //foreach (float time in breakAttempts)
                //{
                //    if (time < Time.time - breakFreeWindow)
                //        timesToRemove++;
                //    else
                //        break;
                //}
                //breakAttempts.RemoveRange(0, timesToRemove);
                if (breakAttempts >= breakFreeCount)
                    foreach (PhysicsHand hand in hands)
                    {
                        if (hand.heldObject == gameObject)
                        {
                            hand.ReleaseObject();
                            ps_escape.Play();
                            breakAttempts = 0;
                        }
                    }
            }
            else if (is_grounded && readyToJump)
            {
                readyToJump = false;

                //Add jump forces

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                //rb.velocity += Vector3.up * jumpForce;
                rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);

                //animate
                animator.Play("Jump");

                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else if (!is_grounded)
            {
                if (readyToJump)
                {
                    readyToJump = false;

                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    //rb.velocity += Vector3.up * jumpForce;
                    rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);

                    animator.Play("Double Jump");
                }
            }
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void checkGrounded()
    {
        Collider[] colliders = new Collider[2];

        int layer_mask = ~((1 << gameObject.layer));

        Vector3 sphere_position = transform.position - new Vector3(0, gnome_height * 0.05f, 0);
        float sphere_radius = gnome_lenght / 2 /*- gnome_lenght * 0.05f*/;
        Physics.OverlapSphereNonAlloc(sphere_position, sphere_radius, colliders, layer_mask, QueryTriggerInteraction.Ignore);
        //Debug.Log(colliders.Length);
        if (colliders[0] != null)
        {
            readyToJump = true;
            is_grounded = true;
            animator.SetBool("Grounded", true);
        }
        else
        {
            is_grounded = false;
            can_update_grounded = false;
            animator.SetBool("Grounded", false);
            Invoke("setBoolBack", 0.05f);
        }
    }

    private void setBoolBack()
    {
        can_update_grounded = true;
    }

    public void resetRotation()
    {
        rb.rotation = Quaternion.Euler(0, rb.rotation.y, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rotation_reset = true;
        animator.SetBool("isGrabbed", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 sphere_position = transform.position - new Vector3(0, gnome_height * 0.05f, 0);
        float sphere_radius = gnome_lenght / 2 /*- gnome_lenght * 0.05f*/;
        Gizmos.DrawSphere(sphere_position, sphere_radius);
    }



    public void reloadScene(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SceneManager.LoadScene(0);
        }
    }
}