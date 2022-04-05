using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Behaviour : MonoBehaviour
{
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;

    float maxMoveSpeedSqr;
    [System.NonSerialized] public Rigidbody rb;
    Vector2 movementInputVector;

    private bool has_input_move;
    Vector3 velocity;
    public float drag = 0.1f;

    public ParticleSystem pickup_effect;

    //public Player2Behaviour player2_behaviour;

    // Start is called before the first frame update
    void Start()
    {
        maxMoveSpeedSqr = maxMoveSpeed * maxMoveSpeed;
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {

        velocity = rb.velocity;
        Move(velocity);

        //max speed
        if (new Vector2(velocity.x, velocity.z).sqrMagnitude > maxMoveSpeedSqr)
        {
            velocity = new Vector3(velocity.x, 0f, velocity.z).normalized * maxMoveSpeed + new Vector3(0f, velocity.y);
        }
        rb.velocity = velocity;

        //if (has_input_move == false)
        //{
        //    rb.velocity = rb.velocity * decceleration * Time.deltaTime;
        //}
    }
    private void FixedUpdate()
    {

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {            
            //drag
            velocity.x *= 1.0f - drag; // reduce x component...
            velocity.z *= 1.0f - drag; // and z component each cycle
            rb.velocity = velocity;
        }
    }

    public void Move(Vector3 velocity_1)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity_1 += new Vector3(0.0f, 0.0f, 1.0f) * acceleration * Time.deltaTime;
            velocity = velocity_1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity_1 += new Vector3(0.0f, 0.0f, -1.0f) * acceleration * Time.deltaTime;
            velocity = velocity_1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity_1 += new Vector3(-1.0f, 0.0f, 0.0f) * acceleration * Time.deltaTime;
            velocity = velocity_1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity_1 += new Vector3(1.0f, 0.0f, 0.0f) * acceleration * Time.deltaTime;
            velocity = velocity_1;
        }
    }

   /* private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ammo"))
        {
            InstantiateAndPlay(pickup_effect, other.gameObject.transform.position, Quaternion.identity, 2.0f);
            Destroy(other.gameObject);

            //player2_behaviour.addAmmo();
            print("picked up ammo");
        }
    }*/

   /* public void InstantiateAndPlay(ParticleSystem particle_system,Vector3 position, Quaternion rotation, float destroyDelay)
    {
        ParticleSystem ps = Instantiate(particle_system, position, rotation) as ParticleSystem;
        ps.Play();
        Destroy(ps.gameObject, destroyDelay);
    }*/

}
