using UnityEngine;

public class BatteryItemController : AbilityItemController
{
    protected override void AddAbility(GameObject other)
    {
        var flashlightController = other.gameObject.AddComponent<FlashlightAbilityController>();
        var flashlight = other.transform.Find("Flashlight").gameObject;
        flashlightController.SetFlashlightGameObject(flashlight);
    }
}
