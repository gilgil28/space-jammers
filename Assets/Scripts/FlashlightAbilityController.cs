using UnityEngine;

public class FlashlightAbilityController : MonoBehaviour
{
    private GameObject _flashlight;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            _flashlight.SetActive(!_flashlight.activeSelf);
        }
    }

    public void SetFlashlightGameObject(GameObject flashlight)
    {
        _flashlight = flashlight;
    }
}