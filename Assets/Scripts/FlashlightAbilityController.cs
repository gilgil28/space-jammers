using System;
using System.Diagnostics;
using UnityEngine;

public class FlashlightAbilityController : MonoBehaviour
{
    private GameObject _flashlight;

    public static FlashlightAbilityController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public bool IsActive()
    {
        return _flashlight.activeSelf;
    }

}