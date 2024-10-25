using System.Collections;
using UnityEngine;

public class FadeInSound : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component
    public float fadeDuration = 2f;  // Duration for fade-in
    public float targetVolume = 1f;  // Target volume level (1 is full volume)

    void Start()
    {
        // Set the initial volume to 0
        audioSource.volume = 0;

        // Start the fade-in coroutine
        StartCoroutine(FadeIn(audioSource, fadeDuration, targetVolume));
    }

    // Coroutine for fading in the sound
    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        // Play the audio if it's not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        while (currentTime < duration)
        {
            // Calculate the new volume level based on time
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure volume is set to the target volume
        audioSource.volume = targetVolume;
    }
}
