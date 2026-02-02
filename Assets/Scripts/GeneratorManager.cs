using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GeneratorManager : MonoBehaviour
{
    public float progressPercentage;
    public float progressSpeed;

    public Slider progressBar;
    private CanvasGroup transparency;

    public SkillCheckManager skillCheckManager;
    [Range(0f, 1f)]
    public float probabilidad;
    private bool skillCheckActive = false;
    public float successBoost;
    public float failPenalty;

    public AudioSource audioSource;
    public AudioClip working;
    public CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transparency = progressBar.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();

        // Revisar skill check
        if (skillCheckActive && Input.GetKeyDown(KeyCode.R))
        {

            CheckSkillResult();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(progressPercentage >= 100f)
        {
            canvasGroup.alpha = 0f;
            transparency.alpha = 0f;
            skillCheckManager.canvasGroup.alpha = 0f;
            return;
        }
        if (other.CompareTag("Player"))
        {
            canvasGroup.alpha = 1f;
            if (!audioSource.isPlaying)
                audioSource.Play();
            ControlTransparency();
            //IncreaseProgress();
            if (skillCheckManager.isVisible() == false)
                TriggerSkillCheck();
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        skillCheckManager.progress = progressPercentage;
        if (other.CompareTag("Player"))
        {
            //IncreaseProgress();
            ControlTransparency();
            if (progressPercentage >= 100f)
                return;
            
        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        if (progressPercentage >= 100f)
        {
            audioSource.clip = working;
            if (!audioSource.isPlaying)
                audioSource.Play();
            canvasGroup.alpha = 0f;
            transparency.alpha = 0f;
            skillCheckManager.canvasGroup.alpha = 0f;
            return;
        }
        canvasGroup.alpha = 0f;

        audioSource.Stop();
        skillCheckManager.EndSkillCheck();
        ControlTransparency();
        return;
    }

    private void IncreaseProgress()
    {
        progressPercentage += progressSpeed * Time.deltaTime;
        if (progressPercentage > 100f)
        {
            progressPercentage = 100f;
        }
    }

    private void UpdateProgressBar()
    {
        if (progressPercentage < 0f)
            progressPercentage = 0f;
        progressBar.value = progressPercentage / 100f;
    }

    //Fade in out of progress
    private void ControlTransparency()
    {
        float targetAlpha = transparency.alpha == 0f ? 1f : 0f;
        StartCoroutine(FadeCanvasGroup(transparency, targetAlpha, 0.5f));
    }
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    // SKILL CHECK BLOCC

    //pause progress and trigger skill check
    private void TriggerSkillCheck()
    {
        progressSpeed = 0f;
        skillCheckManager.GenerateSkillCheck();
        skillCheckActive = true; 
    }

    private void CheckSkillResult()
    {
        if (skillCheckManager.FinalCheck())
        {
            if (skillCheckManager.lineRotationSpeed < skillCheckManager.lineRotationSpeedMax)
                skillCheckManager.lineRotationSpeed += 40f;

            progressPercentage += successBoost;
            skillCheckManager.RandomizeArea();
        }
        else
        {
            if (skillCheckManager.lineRotationSpeed > skillCheckManager.lineRotationSpeedMin)
                skillCheckManager.lineRotationSpeed -= 50f;
            if(skillCheckManager.lineRotationSpeed < skillCheckManager.lineRotationSpeedMin)
                skillCheckManager.lineRotationSpeed = skillCheckManager.lineRotationSpeedMin;
            progressPercentage -= failPenalty;
        }
    }

    private void AdjustVolume()
    {
        audioSource.minDistance = 5f;
        audioSource.maxDistance = 20f;
    }
}
