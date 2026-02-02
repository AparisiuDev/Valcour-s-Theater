using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BarrraOxygen : MonoBehaviour
{
    public CanvasGroup miCanvasGroup;
    public MaskController maskController;


    void Start()
    {
        miCanvasGroup.alpha = 0f;
        miCanvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (maskController.IsMaskDown())
        {
            miCanvasGroup.alpha = 1f;
        }
        else
        {
            miCanvasGroup.alpha = 0f;
        }
    }
}
