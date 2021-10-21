using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour/*, AxisState.IInputAxisProvider*/
{
    public float maxSensitivity = 1000.0f;

    public Cinemachine.AxisState yAxis;
    public Cinemachine.AxisState xAxis;
    public Transform camera_look_at_transform;

    public Cinemachine.CinemachineVirtualCamera headCamera;
    private CinemachineComponentBase head_component_base;

    //public Cinemachine.CinemachineVirtualCamera aimCamera;

    private float head_camera_max_distance = 8.0f;
    private float head_camera_min_distance = 0.0f;

    public Slider sensetivity_slider;

    private Vector2 mouse_input;
    private Vector3 axis_look;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //sensetivity_slider.value = PlayerPrefs.GetFloat("SensitivityValue", xAxis.m_MaxSpeed / maxSensitivity);

        xAxis.SetInputAxisProvider(0, GetComponent<CinemachineInputProvider>());
        yAxis.SetInputAxisProvider(1, GetComponent<CinemachineInputProvider>());

        xAxis.m_MaxSpeed = maxSensitivity;
        yAxis.m_MaxSpeed = maxSensitivity;

        head_component_base = headCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        camera_look_at_transform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0.0f);

        //(head_component_base as Cinemachine3rdPersonFollow).CameraDistance -= Input.mouseScrollDelta.y;
        (head_component_base as Cinemachine3rdPersonFollow).CameraDistance = Mathf.Clamp((head_component_base as Cinemachine3rdPersonFollow).CameraDistance, head_camera_min_distance, head_camera_max_distance);
    }

    //public void updateLook(InputAction.CallbackContext context)
    //{
    //    mouse_input = context.ReadValue<Vector2>();
    //}

    //public float GetAxisValue(int axis)
    //{
    //    switch (axis)
    //    {
    //        case 0:
    //            return mouse_input.x;
    //        case 1:
    //            return mouse_input.y;
    //        default:
    //            break;
    //    }
    //    return 0;
    //}

    public void SetSensitivity()
    {
        //PlayerPrefs.SetFloat("SensitivityValue", sensetivity_slider.value);

        //xAxis.m_MaxSpeed = maxSensitivity * sensetivity_slider.value;
        //yAxis.m_MaxSpeed = maxSensitivity * sensetivity_slider.value;
    }
}

