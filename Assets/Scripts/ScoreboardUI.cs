using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour
{
    public Text distanceText;
    public Text fuelCansText;

    private void Start()
    {
        // Retrieve distance and fuel can data from PlayerPrefs
        float distance = PlayerPrefs.GetInt("DistanceRan", 0);
        int fuelCansCollected = PlayerPrefs.GetInt("FuelCansCollected", 0);

        // Display the data
        distanceText.text = distance.ToString() + " M";
        fuelCansText.text = fuelCansCollected.ToString();
    }
}
