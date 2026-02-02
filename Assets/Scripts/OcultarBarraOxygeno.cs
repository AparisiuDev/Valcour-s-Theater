using UnityEngine;

public class OcultarBarraOxygeno : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MaskController maskController;
    public CanvasGroup canvasGroup;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maskController.maskDown)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
}
