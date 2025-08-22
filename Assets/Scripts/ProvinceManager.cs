using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProvinceManager : SerializedMonoBehaviour
{
    public event System.EventHandler OnProvinceSelected;
    public event System.EventHandler OnProvinceDeselected;

    public static ProvinceManager Instance { get; private set; }

    private Province selectedProvince;

    [SerializeField, Required] private Texture2D provinceMapTexture; // Reference to the texture containing province colors


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AssignPixelCoordinates();
    }

    private void AssignPixelCoordinates()
    {
        // Add the pixel coordinates to each province based on the provinceMapTexture
        for (int i = 0; i < provinceMapTexture.width; i++)
        {
            for (int j = 0; j < provinceMapTexture.height; j++)
            {
                Color32 currentColor = provinceMapTexture.GetPixel(i, j);
                Province province = GetProvinceByColor(currentColor);
                Vector2Int pixel = new Vector2Int(i, j);

                if (province != null)
                {
                    province.pixelCoordinates.Add(pixel); // Add pixel coordinates to the province

                    // Check for bordering provinces and add border pixels
                    for(int x = -1; x <= 1; x++)
                    {
                        for(int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue; // Skip the current pixel
                            
                            Vector2Int borderingPixel = new Vector2Int(i + x, j + y);
                            
                            if (borderingPixel.x >= 0 && borderingPixel.x < provinceMapTexture.width &&
                                borderingPixel.y >= 0 && borderingPixel.y < provinceMapTexture.height)
                            {
                                Color32 borderingColor = provinceMapTexture.GetPixel(borderingPixel.x, borderingPixel.y);
                                Province neighborProvince = GetProvinceByColor(borderingColor);
                                if (neighborProvince != null && neighborProvince != province)
                                {
                                    AddBorderPixelToProvince(province, pixel, borderingPixel);
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    private void AddBorderPixelToProvince(Province province, Vector2Int pixel, Vector2Int borderingPixel)
    {
        Province neighborProvince = GetProvinceByColor(provinceMapTexture.GetPixel(borderingPixel.x, borderingPixel.y));

        if (province.borderPixelCoordinates.ContainsKey(neighborProvince) == false)
        {
            province.borderPixelCoordinates[neighborProvince] = new List<Vector2Int>();
        }
        else
        {
            province.borderPixelCoordinates[neighborProvince].Add(new Vector2Int(pixel.x, pixel.y));
        }
    }

    public void SetSelectedProvince(Province province)
    {
        selectedProvince = province;
        OnProvinceSelected?.Invoke(this, EventArgs.Empty);
    }
    public void DeselectProvince()
    {
        selectedProvince = null;
        OnProvinceDeselected?.Invoke(this, EventArgs.Empty);
    }
    public Province GetSelectedProvince()
    {
        return selectedProvince;
    }

    public Dictionary<Color32, Province> provinceDictionary;

    public Province GetProvinceByColor(Color32 color)
    {
        if (provinceDictionary.TryGetValue(color, out Province province))
        {
            return province;
        }

        Debug.Log("No Province found for color: " + color);
        return null;
    }

    public Texture2D GetProvinceMap()
    {
        return provinceMapTexture;
    }

    public List<Province> GetProvinces()
    {
        return new List<Province>(provinceDictionary.Values);
    }
}