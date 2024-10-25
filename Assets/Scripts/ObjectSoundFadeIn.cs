using System.Collections;
using UnityEngine;

public class ObjectSoundFadeIn : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource component
    public Transform player;         // The player object (reference to its Transform)
    public Camera mainCamera;        // Reference to the main camera
    public float maxVolumeDistance = 5f;  // Distance where the sound will be at max volume
    public float fadeDuration = 1f;  // Time it takes to fade in sound
    public float targetVolume = 1f;  // Maximum volume

    private Renderer objectRenderer;  // To check visibility on screen
    private bool isSoundPlaying = false;

    void Start()
    {
        // Get the renderer of the object to check visibility
        objectRenderer = GetComponent<Renderer>();

        // Ensure volume is initially 0
        audioSource.volume = 0;
    }

    void Update()
    {
        // Check if the object is visible on the screen
        if (IsObjectVisible(mainCamera, objectRenderer))
        {
            // Get the distance between the player and the object
            float distance = Vector3.Distance(player.position, transform.position);

            // Check if the object is within the maximum distance range and to the right side of the player
            if (distance <= maxVolumeDistance && transform.position.x > player.position.x)
            {
                if (!isSoundPlaying)
                {
                    // Start the fade-in if it's not already playing
                    StartCoroutine(FadeIn(audioSource, fadeDuration, targetVolume));
                    isSoundPlaying = true;
                }

                // Adjust volume based on the player's proximity (closer = louder)
                float volume = Mathf.Lerp(0, targetVolume, 1 - (distance / maxVolumeDistance));
                audioSource.volume = volume;
            }
        }
        else
        {
            // If the object is not visible, stop the sound or fade out
            if (isSoundPlaying)
            {
                StartCoroutine(FadeOut(audioSource, fadeDuration));
                isSoundPlaying = false;
            }
        }
    }

    // Check if the object is visible in the camera's view
    bool IsObjectVisible(Camera camera, Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    // Coroutine to fade in sound
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

    // Coroutine to fade out sound
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
