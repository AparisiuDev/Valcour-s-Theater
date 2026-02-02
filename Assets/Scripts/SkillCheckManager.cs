using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckManager : MonoBehaviour
{
    public Image Area;
    public Image Line;
    RectTransform areaRect;
    RectTransform lineRect;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    public float lineRotationSpeed = 180f;
    public float lineRotationSpeedMax = 360f;
    public float lineRotationSpeedMin= 100f;
    private Coroutine rotateCoroutine;

    public float progress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup.alpha = 0f; 
        areaRect = Area.GetComponent<RectTransform>();
        lineRect = Line.GetComponent<RectTransform>();
    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log(lineRotationSpeedMax);
        Debug.Log(lineRotationSpeedMin);
    }

    public bool FinalCheck()
    {
        if (IsLineInArea())
        {
            return true;
        }
        if (progress >= 100f)
        {
            EndSkillCheck();
            return true;
        }
        else
        {
           
            return false;
        }
    }

    public void GenerateSkillCheck()
    {
        ControlTransparency();
        RandomizeArea();
        RotateLine();
    }
    public void EndSkillCheck()
    {
        StopRotateLine();
        ControlTransparency();
    }

    public bool isVisible()
    {
        return canvasGroup.alpha > 0f;
    }

    public bool IsLineInArea()
    {
       if ((lineRect.localEulerAngles.z <= areaRect.localEulerAngles.z + 45f) &&
           (lineRect.localEulerAngles.z >= areaRect.localEulerAngles.z - 45f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ControlTransparency()
    {
        if (canvasGroup.alpha == 0f)
            StartCoroutine(FadeCanvas(1f)); // 1 = opaco
        else
            StartCoroutine(FadeCanvas(0f)); // 0 = transparente
    }

    public IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    public void RandomizeArea()
    {
        areaRect.Rotate(Vector3.forward, Random.Range(0f, 360f));
    }

    public void RotateLine()
    {
        if (rotateCoroutine == null)
            rotateCoroutine = StartCoroutine(RotateLineCoroutine());
    }

    public void StopRotateLine()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
            lineRect.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    public IEnumerator RotateLineCoroutine()
    {
        while (true)
        {
            lineRect.Rotate(Vector3.back, lineRotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
