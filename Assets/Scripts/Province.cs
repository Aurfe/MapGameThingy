using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Province : MonoBehaviour
{
    private void Awake()
    {
        borderPixelCoordinates = new Dictionary<Province, List<Vector2Int>>();
        provinceName = gameObject.name;
    }

    private string provinceName;

    public Country country;

    private List<ConcreteSite> provinceSites = new List<ConcreteSite>();
    private List<Pop> provincePops = new List<Pop>();

    private void Start()
    {
        foreach (Transform child in transform)
            if (child.TryGetComponent<ConcreteSite>(out var site))
            {
                provinceSites.Add(site);
                if (site.GetPop() != null)
                {
                    provincePops.Add(site.GetPop());
                }
            }
    }

    public List<Vector2Int> pixelCoordinates; // List of pixel coordinates representing the province on the map

    public Dictionary<Province, List<Vector2Int>> borderPixelCoordinates; // Dictionary to hold neighboring provinces and their border pixel coordinates

    // Update the border pixels on the border map texture based on neighboring provinces
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

    public string GetProvinceName()
    {
        return provinceName;
    }

    public List<ConcreteSite> GetSiteList()
    {
        return provinceSites;
    }

    public List<Pop> GetPopList()
    {
        return provincePops;
    }
}