using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehaviour : MonoBehaviour
{
    public GameObject glass, spider, car;
    private Vector3 original_glass_position, original_spider_position, original_car_position;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        original_glass_position = glass.transform.position;
        original_spider_position = spider.transform.position;
        original_car_position = car.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateCar(InputAction.CallbackContext context)
    {
        print("activated car");
        if(context.performed)
        {
            if (car.activeSelf == false)
            {
                spider.transform.position = original_spider_position;
                car.transform.position = original_car_position;
                glass.transform.position = original_glass_position;
                spider.SetActive(false);
                car.SetActive(true);
            }
            else
            {
                glass.transform.position = original_glass_position;
                spider.SetActive(true);
                car.SetActive(false);
            }
        }
    }

    public void resetGlass(InputAction.CallbackContext context)
    {
        print("glass reset");
        if (context.performed)
        {
            glass.transform.position = original_glass_position;
        }
    }
}
