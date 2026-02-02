using UnityEngine;
using System.Collections;

public class RandomAudioChance : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Tiempo entre sonidos")]
    public float minDelay = 1f;
    public float maxDelay = 3f;

    void Start()
    {

        StartCoroutine(PlayRandomly());
    }

    IEnumerator PlayRandomly()
    {
        while (true)
        {
            // Espera aleatoria antes de sonar
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            // Reproduce el sonido
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.volume = Random.Range(0.7f, 1f);
            audioSource.Play();

            // Espera a que termine el clip
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
