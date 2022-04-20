using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehaviour : MonoBehaviour
{
    public GameObject glass, spider, car, stick;
    private Vector3 original_glass_position, original_spider_position, original_car_position, original_stick_position;
    private Quaternion original_glass_rotation, original_spider_rotation, original_car_rotation, original_stick_rotation;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateCar(InputAction.CallbackContext context)
    {
        //print("activated car");
        if(context.performed)
        {
            if (car.activeSelf == false)
            {
                spider.transform.position = original_spider_position;
                spider.transform.rotation = new Quaternion(original_spider_rotation.x, 0.0f, original_spider_rotation.z, 0.0f);
                car.transform.position = original_car_position;
                car.transform.rotation = new Quaternion(original_car_rotation.x, 0.0f, original_car_rotation.z, 0.0f);

                glass.transform.position = original_glass_position;
                glass.transform.rotation = original_glass_rotation;

                spider.SetActive(false);
                car.SetActive(true);
            }
            else
            {
                glass.transform.position = original_glass_position;
                glass.transform.rotation = original_glass_rotation;

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

            stick.transform.position = original_stick_position;
            stick.transform.rotation = original_stick_rotation;
        }
    }
}
