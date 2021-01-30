using UnityEngine;

public class BatteryItemController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log("Found battery!");
            var flashlightController = other.gameObject.AddComponent<FlashlightAbilityController>();
            var flashlight = other.transform.Find("Flashlight").gameObject;
            flashlightController.SetFlashlightGameObject(flashlight);
            Destroy(gameObject);
        }
    }
}
