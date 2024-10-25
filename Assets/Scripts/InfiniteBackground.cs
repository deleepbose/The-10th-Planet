using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform player;                // Reference to the player
    public Transform[] backgrounds;         // Array of background elements
    public float backgroundWidth;           // Width of a single background section

    private Vector3[][] originalFuelCanPositions; // Store the original positions of fuel cans

    void Start()
    {
        // Initialize and store the original positions of fuel cans
        originalFuelCanPositions = new Vector3[backgrounds.Length][];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            Transform background = backgrounds[i];
            FuelCan[] fuelCans = background.GetComponentsInChildren<FuelCan>();

            originalFuelCanPositions[i] = new Vector3[fuelCans.Length];
            for (int j = 0; j < fuelCans.Length; j++)
            {
                originalFuelCanPositions[i][j] = fuelCans[j].transform.localPosition;
            }
        }
    }

    void Update()
    {
        HandleBackgroundRepositioning();
    }

    void HandleBackgroundRepositioning()
    {
        // Determine the current background the player is in
        Transform currentBackground = GetCurrentBackground();

        if (currentBackground != null)
        {
            int index = System.Array.IndexOf(backgrounds, currentBackground);

            // If the player is in the middle of a background and not in the first or second backgrounds
            if (index > 1 && player.position.x > currentBackground.position.x + backgroundWidth / 2)
            {
                // Move the background 2 places back, to the end
                Transform previousBackground = backgrounds[index - 2];
                MoveBackgroundToEnd(previousBackground, index - 2);
            }
        }
    }

    Transform GetCurrentBackground()
    {
        // Check which background the player is currently on
        foreach (var background in backgrounds)
        {
            if (player.position.x > background.position.x &&
                player.position.x < background.position.x + backgroundWidth)
            {
                return background;
            }
        }
        return null;
    }

    void MoveBackgroundToEnd(Transform background, int backgroundIndex)
    {
        // Find the last background
        Transform lastBackground = backgrounds[backgrounds.Length - 1];

        // Move the given background to the end
        background.position = new Vector3(lastBackground.position.x + backgroundWidth, background.position.y, background.position.z);

        // Reset fuel cans to their original positions
        FuelCan[] fuelCans = background.GetComponentsInChildren<FuelCan>();
        for (int i = 0; i < fuelCans.Length; i++)
        {
            fuelCans[i].transform.localPosition = originalFuelCanPositions[backgroundIndex][i];
            fuelCans[i].ResetFuelCan(); // Assuming you have a method to reset the state
        }

        // Reorder the backgrounds array
        for (int i = 0; i < backgrounds.Length - 1; i++)
        {
            backgrounds[i] = backgrounds[i + 1];
        }
        backgrounds[backgrounds.Length - 1] = background;
    }
}
