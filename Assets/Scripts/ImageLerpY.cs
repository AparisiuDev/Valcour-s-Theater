using UnityEngine;
using UnityEngine.UI;

public class ImageLerpY : MonoBehaviour
{
    public RectTransform imageRect; // Asignar la imagen desde el inspector
    public float speed = 2f;        // Velocidad del lerp

    private bool moving = false;     // Controla si el lerp está activo
    private float targetY;           // Posición Y objetivo
    private float startY;            // Posición Y inicial
    private float t = 0f;            // Tiempo interpolado

    void Start()
    {
        if (imageRect == null)
        {
            imageRect = GetComponent<RectTransform>();
        }

        startY = imageRect.anchoredPosition.y; // Posición inicial
        targetY = (startY == 780f) ? 250f : 780f; // Configura el objetivo inicial
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moving = true;
            t = 0f;
            startY = imageRect.anchoredPosition.y;
            targetY = (Mathf.Approximately(startY, 780f)) ? 250f : 780f;
        }

        if (moving)
        {
            t += Time.deltaTime * speed;
            float newY = Mathf.Lerp(startY, targetY, t);
            imageRect.anchoredPosition = new Vector2(imageRect.anchoredPosition.x, newY);

            if (t >= 1f)
            {
                moving = false; // Detiene el lerp al llegar al destino
            }
        }
    }
}
