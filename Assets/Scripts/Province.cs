using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Province : MonoBehaviour
{
    private void Awake()
    {
        borderPixelCoordinates = new Dictionary<Province, List<Vector2Int>>();
    }

    public string provinceName;

    public int population;

    public float gdp;

    public string provinceCapital;

    public string climateType;

    public Country country; // Reference to the Country this Province belongs to

    public List<Vector2Int> pixelCoordinates; // List of pixel coordinates representing the province on the map

    public Dictionary<Province, List<Vector2Int>> borderPixelCoordinates; // Dictionary to hold neighboring provinces and their border pixel coordinates

    public void UpdateBorderPixels()
    {
        Color32 borderColor;
        Color32 internalBorderColor = ProvinceBorderGenerator.instance.GetInternalBorderColor();
        Color32 externalBorderColor = ProvinceBorderGenerator.instance.GetExternalBorderColor();
        Texture2D borderMapTexture = ProvinceBorderGenerator.instance.GetBorderMapTexture();

        foreach (Province neighbor in borderPixelCoordinates.Keys)
        {
            // Check if the neighbor province is part of the same country or a different one
            if (neighbor.country == country)
            {
                borderColor = internalBorderColor;
            }
            else
            {
                borderColor = externalBorderColor;
            }

            foreach (Vector2Int borderPixel in borderPixelCoordinates[neighbor])
            {
                // Set the border pixel color to the specified border color
                borderMapTexture.SetPixel(borderPixel.x, borderPixel.y, borderColor);
            }
        }

        borderMapTexture.Apply(); // Apply changes to the texture
    }
}