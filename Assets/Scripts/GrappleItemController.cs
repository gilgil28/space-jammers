using UnityEngine;

public class GrappleItemController : AbilityItemController
{
    protected override void AddAbility(GameObject other)
    {
        var grappleController = other.GetComponent<GrappleLook>();
        grappleController.enabled = true;
    }
}