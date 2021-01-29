using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        transform.DOScaleX(5, 1).OnComplete(()=> {
            Debug.Log("test");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
