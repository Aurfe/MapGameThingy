using UnityEngine;

public class CountryMapGenerator : MonoBehaviour
{
    private Texture2D countryMapTexture;
    [SerializeField] private Material countryMapMaterial;
    private Texture2D provinceMapTexture;

    private void Start()
    {
        provinceMapTexture = ProvinceManager.Instance.GetProvinceMap(); // Get the province map texture from the ProvinceManager

        countryMapTexture = new Texture2D(provinceMapTexture.width, provinceMapTexture.height, TextureFormat.ARGB32, false);
        countryMapTexture.filterMode = FilterMode.Point; // Set filter mode to Point for pixel art style

        GenerateMap();
    }

    void GenerateMap()
    {
        foreach (Province province in ProvinceManager.Instance.GetProvinces())
        {
            foreach(Vector2Int pixel in province.pixelCoordinates)
            {
                if(province.country != null) // Ensure the province has a country assigned
                {
                    Color32 color = province.country.countryColor; // Get the country's color
                    countryMapTexture.SetPixel(pixel.x, pixel.y, color); // Set the pixel color in the country map texture
                }
                else
                {
                    countryMapTexture.SetPixel(pixel.x, pixel.y, Color.clear); // Set to transparent if no country is assigned
                }
            }
        }

        countryMapTexture.Apply(); // Apply changes to the texture
        countryMapMaterial.mainTexture = countryMapTexture; // Set the material's texture
    }
}
