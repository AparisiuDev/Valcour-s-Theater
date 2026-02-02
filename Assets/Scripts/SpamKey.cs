using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class SpamKey : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider spamSlider;      // Slider de la UI
    public float fillSpeed = 0.1f; // Cuánto aumenta por cada pulsación de Z
    public float drainSpeed = 0.05f; // Cuánto se vacía por segundo
    public float hideDelay = 3f;   // Tiempo antes de ocultar el slider al completarse

    private bool inZone = false;      // Saber si el player está dentro del trigger
    private bool isCompleted = false; // Saber si el slider ya llegó al máximo

    public AudioSource audioSource;
    public CanvasGroup canvasGroup;

    void Start()
    {
        // Al inicio, esconder el slider
        spamSlider.gameObject.SetActive(false);
        spamSlider.value = 0;
    }

    void Update()
    {
        if (!inZone || isCompleted) return; // Solo funciona si estás en la zona y no completado

        // Spamear Z para llenar el slider
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();


            spamSlider.value += fillSpeed;

            // Comprobar si llegó al máximo
            if (spamSlider.value >= spamSlider.maxValue)
            {

                audioSource.Stop();
                spamSlider.value = spamSlider.maxValue;
                isCompleted = true; // Marcamos como completado
                StartCoroutine(HideSliderAfterDelay()); // Iniciamos la corutina
            }
        }

        // Vaciar lentamente con el tiempo (solo si no se completó)
        if (!isCompleted)
        {
            spamSlider.value -= drainSpeed * Time.deltaTime;
            if (spamSlider.value < 0)
                spamSlider.value = 0;
        }
    }

    // Detectar cuando entra el player en el trigger
    private void OnTriggerEnter(Collider other)
    {
        canvasGroup.alpha = 1f;
        if (spamSlider.value >= spamSlider.maxValue)
        {
            canvasGroup.alpha = 0f;
            return;
        }
        if (other.CompareTag("Player"))
        {
            inZone = true;
            spamSlider.gameObject.SetActive(true); // Mostrar el slider
        }
    }

    // Detectar cuando sale el player del trigger
    private void OnTriggerExit(Collider other)
    {
        canvasGroup.alpha = 0f;
        if (other.CompareTag("Player"))
        {

            audioSource.Stop();
            inZone = false;

            // Solo ocultar si no está completado
            if (!isCompleted)
            {
                spamSlider.gameObject.SetActive(false); // Ocultar el slider
                spamSlider.value = 0; // Reiniciar el slider
            }
        }
    }

    // Corutina para ocultar el slider después de un tiempo
    private IEnumerator HideSliderAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        spamSlider.gameObject.SetActive(false);
    }
}
