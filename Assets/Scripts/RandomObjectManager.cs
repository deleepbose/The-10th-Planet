using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectManager : MonoBehaviour
{
    [System.Serializable]
    public class RandomObject
    {
        public GameObject objectToToggle;  // Object to be made visible/invisible
        public bool randomizePosition = true;  // Whether to randomize its position
        public Vector2 xRange;  // Range for random x position
        public Vector2 yRange;  // Range for random y position
    }

    public RandomObject[] randomObjects;  // Array of objects that will be managed
    public float activationProbability = 0.5f;  // Probability that an object will be activated

    private void OnEnable()
    {
        ToggleAndRandomizeObjects();
    }

    private void ToggleAndRandomizeObjects()
    {
        foreach (RandomObject randomObject in randomObjects)
        {
            //generate a random number between 0 and 1
            float probability = Random.Range(0f, 1f);

            // Decide whether to activate or deactivate the object
            bool shouldActivate = probability < activationProbability;

            randomObject.objectToToggle.SetActive(shouldActivate);

            if (shouldActivate)
            {
                if (randomObject.objectToToggle.CompareTag("Laser"))
                {
                    // Play the object's sound effect if it has one
                    AudioSource audioSource = randomObject.objectToToggle.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.Play();
                        audioSource.volume = 0;
                    }
                }

                if (randomObject.randomizePosition)
                {
                    // Randomize the position within the specified range
                    Vector3 newPosition = randomObject.objectToToggle.transform.position;

                    if (randomObject.xRange != Vector2.zero)
                    {
                        newPosition.x = Random.Range(randomObject.xRange.x, randomObject.xRange.y);
                    }

                    if (randomObject.yRange != Vector2.zero)
                    {
                        newPosition.y = Random.Range(randomObject.yRange.x, randomObject.yRange.y);
                    }

                    randomObject.objectToToggle.transform.position = newPosition;
                }
            }
        }
    }
}
