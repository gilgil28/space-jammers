using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButtonController : MonoBehaviour
{
    [SerializeField] GameObject mPlayer;
    [SerializeField] GameObject mBats;
    
    private bool _hasReacted;
    

    // Update is called once per frame
    private void Update()
    {
        if (_hasReacted || FlashlightAbilityController.Instance == null)
        {
            return;
        }
       
        if (FlashlightAbilityController.Instance.IsActive())
        {
            var distance = Vector3.Distance(mPlayer.transform.position, transform.position);
            if (distance < 1)
            {
                mBats.SetActive(true);
                _hasReacted = true;
            }
        }
    }
}
