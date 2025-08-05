using System;
using TMPro;
using UnityEngine;

public class ProvinceUI : MonoBehaviour, IUIManager
{
    [SerializeField] GameObject provinceUIContainer;

    [Header("Province UI Elements")]
    [SerializeField] TextMeshProUGUI provinceNameText;
    [SerializeField] TextMeshProUGUI populationText;
    [SerializeField] TextMeshProUGUI gdpText;
    [SerializeField] TextMeshProUGUI capitalText;
    [SerializeField] TextMeshProUGUI climateText;

    public void Selected()
    {
        provinceUIContainer.SetActive(true);
        SetUI();
    }

    public void Deselected()
    {
        provinceUIContainer.SetActive(false);
    }

    private void SetUI()
    {
        Province selectedProvince = ProvinceManager.Instance.GetSelectedProvince();
        if (selectedProvince == null)
        {
            Debug.LogWarning("No province selected, cannot set UI.");
            return;
        }

        provinceNameText.text = selectedProvince.provinceName;

        populationText.text = selectedProvince.population.ToString("N0");
        gdpText.text = "$" + selectedProvince.gdp.ToString("N0") + "b";
        capitalText.text = selectedProvince.provinceCapital;
        climateText.text = selectedProvince.climateType;
    }
}
