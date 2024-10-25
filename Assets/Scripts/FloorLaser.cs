using UnityEngine;
using System.Collections;

public class FloorLaser : MonoBehaviour
{
    public AudioSource audioSource;     // AudioSource component for the floor laser sound
    public Camera mainCamera;           // Reference to the main camera
    public float fadeDuration = 1f;     // Time it takes to fade in sound
    public float targetVolume = 1f;     // Maximum volume

    private Renderer laserRenderer;     // Renderer of the floor laser composite 
    private bool isSoundPlaying = false;

    void Start()
    {
        // Get the Renderer component from the floor laser's child or parent object
        laserRenderer = GetComponentInChildren<SpriteRenderer>();

        // Ensure volume starts at 0
        audioSource.volume = 0;
    }

    void Update()
    {
        // Check if the floor laser is visible on the screen
        if (IsLaserVisible(mainCamera, laserRenderer))
        {
            if (!isSoundPlaying)
            {
                StartCoroutine(FadeIn(audioSource, fadeDuration, targetVolume));
                isSoundPlaying = true;
            }
        }
        else
        {
            // fade out the sound if the laser is off-screen
            if (isSoundPlaying)
            {
                StartCoroutine(FadeOut(audioSource, fadeDuration));
                isSoundPlaying = false;
            }
        }
    }

    // Check if the laser is visible in the camera's view
    bool IsLaserVisible(Camera camera, Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    // Coroutine to fade in the sound
    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    // Coroutine to fade out the sound
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
