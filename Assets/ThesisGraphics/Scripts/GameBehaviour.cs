using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class GameBehaviour : MonoBehaviour
{
    public GameObject glass, spider, car, stick;
    private Rigidbody glass_rb, stick_rb;
    private Vector3 original_glass_position, original_spider_position, original_car_position, original_stick_position;
    private Quaternion original_glass_rotation, original_spider_rotation, original_car_rotation, original_stick_rotation;

    private NavMeshAgent spider_nav_agent;

    //for releasing everything in hand
    [SerializeField] PhysicsHand left_hand, right_hand;
    PhysicsHand[] hands;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        original_glass_position = glass.transform.position;
        original_glass_rotation = glass.transform.rotation;

        original_spider_position = spider.transform.position;
        original_spider_rotation = spider.transform.rotation;

        original_car_position = car.transform.position;
        original_car_rotation = car.transform.rotation;

        original_stick_position = stick.transform.position;
        original_stick_rotation = stick.transform.rotation;

        spider_nav_agent = spider.GetComponent<NavMeshAgent>();

        glass_rb = glass.GetComponent<Rigidbody>();
        stick_rb = stick.GetComponent<Rigidbody>();


        hands = new PhysicsHand[2] { left_hand, right_hand };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateCar(InputAction.CallbackContext context)
    {
        //print("activated car");
        if (context.performed)
        {
            if (car.activeSelf == false)
            {
                spider.transform.position = original_spider_position;
                spider.transform.rotation = new Quaternion(original_spider_rotation.x, 0.0f, original_spider_rotation.z, 0.0f);
                car.transform.position = original_car_position;
                car.transform.rotation = new Quaternion(original_car_rotation.x, 0.0f, original_car_rotation.z, 0.0f);

                glass.transform.position = original_glass_position;
                glass.transform.rotation = original_glass_rotation;
                stick.transform.position = original_stick_position;
                stick.transform.rotation = original_stick_rotation;
                glass_rb.velocity = Vector3.zero;
                stick_rb.velocity = Vector3.zero;

                spider.SetActive(false);
                car.SetActive(true);
            }
            else
            {
                glass.transform.position = original_glass_position;
                glass.transform.rotation = original_glass_rotation;
                stick.transform.position = original_stick_position;
                stick.transform.rotation = original_stick_rotation;
                glass_rb.velocity = Vector3.zero;
                stick_rb.velocity = Vector3.zero;

                spider.SetActive(true);
                car.SetActive(false);
            }
        }
    }

    public void resetGlass(InputAction.CallbackContext context)
    {
        //print("glass reset");
        if (context.performed)
        {
            glass.transform.position = original_glass_position;
            glass.transform.rotation = original_glass_rotation;
            glass_rb.velocity = Vector3.zero;

            stick.transform.position = original_stick_position;
            stick.transform.rotation = original_stick_rotation;
            stick_rb.velocity = Vector3.zero;
        }
    }

    public void turn_off_on_nav_agent(InputAction.CallbackContext context)
    {
        //print("glass reset");
        if (context.performed)
        {
            spider_nav_agent.enabled = !spider_nav_agent.enabled;
        }
    }

    public void drop_all(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            foreach (PhysicsHand hand in hands)
            {
                if (hand.heldObject != null)
                {
                    hand.ReleaseObject();
                    print("drop");
                }
            }
        }
    }
}
