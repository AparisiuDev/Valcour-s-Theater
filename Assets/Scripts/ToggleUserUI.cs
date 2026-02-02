using UnityEngine;

public class ToggleMoveUI : MonoBehaviour
{
    [Header("UI")]
    public RectTransform uiElement;

    [Header("Movement")]
    public float moveAmount = 400f;

    [Header("Lerp")]
    public float lerpSpeed = 8f;

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private bool isUp = false;

    void Start()
    {
        if (uiElement != null)
        {
            initialPosition = uiElement.anchoredPosition;
            targetPosition = initialPosition;
        }
    }

    void Update()
    {
        if (uiElement == null)
            return;

        // Input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isUp = !isUp;

            targetPosition = isUp
                ? initialPosition + Vector2.up * moveAmount
                : initialPosition;
        }

        // Lerp suave
        uiElement.anchoredPosition = Vector2.Lerp(
            uiElement.anchoredPosition,
            targetPosition,
            Time.deltaTime * lerpSpeed
        );
    }
}
