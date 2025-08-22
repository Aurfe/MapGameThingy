using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum MapMode
{
    Province,
    Country
}

public class MapModeManager : MonoBehaviour
{
    public static MapModeManager Instance { get; private set; }

    [SerializeField] private MeshRenderer countryMapRenderer;

    MapMode currentMapMode = MapMode.Country;

    Dictionary<MapMode, MeshRenderer> mapModes;

    private void Awake()
    {
        Instance = this;

        mapModes = new Dictionary<MapMode, MeshRenderer>();
    }

    private void Start()
    {
        mapModes.Add(MapMode.Country, countryMapRenderer);

        mapModes.Add(MapMode.Province, null);
    }

    public void SwapMapMode(MapModeComponent mapModeComponent)
    {
        MapMode mapMode = mapModeComponent.mapMode;

        // Province map should always be enabled, so we handle it separately
        if (currentMapMode != MapMode.Province)
        {
            mapModes[currentMapMode].enabled = false;
        }
        if (mapMode == MapMode.Province)
        {
            currentMapMode = MapMode.Province;
            return;
        }

        // Enable the new map mode and disable the current one
        mapModes[mapMode].enabled = true;
        currentMapMode = mapMode;
    }

    public MapMode GetCurrentMapMode()
    {
        return currentMapMode;
    }
}
