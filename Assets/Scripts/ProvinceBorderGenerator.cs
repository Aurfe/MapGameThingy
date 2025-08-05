using UnityEngine;

public class ProvinceBorderGenerator : MonoBehaviour
{
    public static ProvinceBorderGenerator instance;

    private Texture2D borderMapTexture;
    [SerializeField] private Material borderMapMaterial;
    [SerializeField] private Texture2D provinceMapTexture;

    [SerializeField] private Color32 externalBorderColor;
    [SerializeField] private Color32 internalBorderColor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        borderMapTexture = new Texture2D(provinceMapTexture.width, provinceMapTexture.height, TextureFormat.ARGB32, false);
        borderMapTexture.filterMode = FilterMode.Point;

        ClearBorderMap();
        GenerateBorders();
    }

    public void GenerateBorders()
    {
        Color32 borderColor = externalBorderColor;

        foreach (Province province in ProvinceManager.Instance.GetProvinces())
        {
            province.UpdateBorderPixels();
        }

        borderMapTexture.Apply(); // Apply changes to the texture
        borderMapMaterial.mainTexture = borderMapTexture; // Set the material's texture
    }

    private void ClearBorderMap()
    {
        for (int i = 0; i < borderMapTexture.width; i++)
        {
            for (int j = 0; j < borderMapTexture.height; j++)
            {
                // Initialize all pixels to transparent
                borderMapTexture.SetPixel(i, j, Color.clear);
            }
        }
    }

    public Texture2D GetBorderMapTexture()
    {
        return borderMapTexture;
    }
    public Color32 GetExternalBorderColor()
    {
        return externalBorderColor;
    }
    public Color32 GetInternalBorderColor()
    {
        return internalBorderColor;
    }
}
