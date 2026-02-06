using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private CountryUI countryUI;
    private ProvinceUI provinceUI;

    List<IManageable> uiManagers;

    private void Awake()
    {
        // Subscribe to events from ProvinceManager
        ProvinceManager.Instance.OnProvinceSelected += ProvinceManager_OnProvinceSelected;
        ProvinceManager.Instance.OnProvinceDeselected += ProvinceManager_OnProvinceDeselected;
    }

    private void Start()
    {
        countryUI = transform.Find("CountryUI Manager").GetComponent<CountryUI>();
        provinceUI = transform.Find("ProvinceUI Manager").GetComponent<ProvinceUI>();

        uiManagers = new List<IManageable> { countryUI, provinceUI };
    }

    //Get the current mapmode from the Mapmode manager instance
    private void SelectUIManager()
    {
        MapMode mapMode = MapModeManager.Instance.GetCurrentMapMode();

        switch (mapMode)
        {
            case MapMode.Province:
                provinceUI.Selected();
                break;
            case MapMode.Country:
                countryUI.Selected();
                break;
        }
    }
    void DeselectAllUIManagers()
    {
        foreach (var uiManager in uiManagers)
        {
            uiManager.Deselected();
        }
    }


    void ProvinceManager_OnProvinceSelected(object sender, EventArgs e)
    {
        DeselectAllUIManagers();
        SelectUIManager();
    }
    void ProvinceManager_OnProvinceDeselected(object sender, EventArgs e)
    {
        DeselectAllUIManagers();
    }
}
