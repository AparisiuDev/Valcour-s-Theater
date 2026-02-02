using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // <- necesario para List

public class MaskController : MonoBehaviour
{
    [SerializeField] private float maskDuration = 0.5f;
    [SerializeField] private float maxMaskDownTime;
    [SerializeField] private Image maskPNG;
    [SerializeField] private RawImage gameRawImage;
    [SerializeField] private Material blurMaterial;
    [SerializeField] private float maxBlur = 5f;
    [SerializeField] private Slider oxygen;

    [Header("Objetos que se activan con la máscara abajo")]
    [SerializeField] private List<GameObject> objectsToShow;

    private RectTransform rt;
    private Vector2 upPosition;
    private Vector2 downPosition;

    public bool maskDown = false;
    private bool isAnimating = false;
    private bool maskUsed = false; // no se puede bajar de nuevo
    private float maskDownTimer = 0f;

    private void Start()
    {
        rt = maskPNG.rectTransform;
        upPosition = rt.anchoredPosition;
        downPosition = upPosition - new Vector2(0f, rt.rect.height);

        if (blurMaterial != null && gameRawImage != null)
        {
            gameRawImage.material = blurMaterial;
            blurMaterial.SetFloat("_Size", 0f);
        }

        // Inicializar slider
        OxygenController();

        // Inicializar objetos ocultos
        SetObjectsActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAnimating && !maskUsed)
        {
            StartCoroutine(AnimateMask(maskDown ? upPosition : downPosition));
            maskDown = !maskDown;

            // Activar/desactivar objetos según la nueva posición
            SetObjectsActive(maskDown);
        }

        if (maskDown)
        {
            maskDownTimer += Time.deltaTime;

            if (maskDownTimer >= maxMaskDownTime)
            {
                maskDownTimer = maxMaskDownTime;

                if (!isAnimating)
                {
                    StartCoroutine(AnimateMask(upPosition));
                    maskDown = false;
                    maskUsed = true;
                    SetObjectsActive(false); // Desactivar objetos al subir
                }
            }
        }

        OxygenController();
    }

    private IEnumerator AnimateMask(Vector2 targetPosition)
    {
        isAnimating = true;

        Vector2 startPos = rt.anchoredPosition;
        float elapsed = 0f;

        bool goingDown = targetPosition == downPosition;

        while (elapsed < maskDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / maskDuration;

            rt.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);

            if (blurMaterial != null)
            {
                float blurAmount = goingDown ? Mathf.Lerp(0f, maxBlur, t) : Mathf.Lerp(maxBlur, 0f, t);
                blurMaterial.SetFloat("_Size", blurAmount);
            }

            yield return null;
        }

        rt.anchoredPosition = targetPosition;

        if (blurMaterial != null)
        {
            blurMaterial.SetFloat("_Size", goingDown ? maxBlur : 0f);
        }

        isAnimating = false;
    }

    private void SetObjectsActive(bool active)
    {
        if (objectsToShow == null) return;

        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (mr != null)
                    mr.enabled = active;
            }
        }
    }

    public bool IsMaskDown()
    {
        return maskDown;
    }

    public void OxygenController()
    {
        if (oxygen == null) return;

        if (maxMaskDownTime <= 0f)
        {
            oxygen.normalizedValue = 1f;
            return;
        }

        float consumed = Mathf.Clamp01(maskDownTimer / maxMaskDownTime);
        oxygen.normalizedValue = 1f - consumed;
    }
}
