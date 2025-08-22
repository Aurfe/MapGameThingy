using UnityEngine;
using UnityEngine.EventSystems;

public class MapInteractor : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                ProvinceManager.Instance.SetSelectedProvince(GetProvinceFromClick());
            }
        }
        else if(Input.GetMouseButtonDown(1)) // Right mouse button
        {
            ProvinceManager.Instance.DeselectProvince();
        }
    }

    private Province GetProvinceFromClick()
    {
        // This method gets the province from a raycast hit on the map

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Add your interaction logic here

            Renderer renderer = hit.collider.GetComponent<Renderer>();

            Texture2D texture = renderer.material.mainTexture as Texture2D;

            Vector2 uv = hit.textureCoord;

            // Convert UV coordinates to pixel coordinates
            int pixelX = Mathf.FloorToInt(uv.x * texture.width);
            int pixelY = Mathf.FloorToInt(uv.y * texture.height);

            // Get the color at the pixel
            Color32 color = texture.GetPixel(pixelX, pixelY);

            // Return the province associated with the color
            return ProvinceManager.Instance.GetProvinceByColor(color);
        }
        else
        {
            Debug.Log("No hit detected.");
            return null;
        }
    }
}
