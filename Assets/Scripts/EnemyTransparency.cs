using UnityEngine;
using System.Collections;

public class EnemyTransparency : MonoBehaviour
{
    [SerializeField] private MaskController maskController;
    [SerializeField] private float fadeDuration = 0.5f;

    private Renderer rend;
    private Material material;
    private bool isVisible = false;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("EnemyAI requiere un Renderer");
            return;
        }

        material = rend.material;

        // Configurar material Lit como transparente
        SetupMaterialTransparent(material);

        // Empezamos invisible
        SetMaterialAlpha(0f);
        isVisible = false;
    }

    private void Update()
    {
        if (maskController.IsMaskDown() && !isVisible) FadeIn();
        else if (!maskController.IsMaskDown() && isVisible) FadeOut();
    }

    private void FadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTo(1f));
        isVisible = true;
    }

    private void FadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTo(0f));
        isVisible = false;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = material.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        SetMaterialAlpha(targetAlpha);
    }

    //Shart de material Lit transparente
    private void SetupMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1f);       
        mat.SetFloat("_Blend", 0f);         
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color c = material.color;
        c.a = alpha;
        material.color = c;

        if (material.HasProperty("_BaseColor"))
        {
            c = material.GetColor("_BaseColor");
            c.a = alpha;
            material.SetColor("_BaseColor", c);
        }
    }
}
