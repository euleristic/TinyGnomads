using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    [Range(0, 1)]
    public float intensity;
    public float duration;

    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor) //Not sure why interactorObject causes issues. Maybe the script must be attached as a component to a reference object.
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
            controller.SendHapticImpulse(intensity, duration);
    }
}
public class HapticInteractable : MonoBehaviour
{

    public Haptic hapticOnActivated;

    // Start is called before the first frame update
    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
    }

    //Check Valem's tutorial on haptics, we need to add decide what events in the prototype causes haptic feedback, and set it up here in the start function.
}
