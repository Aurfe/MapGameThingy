using NUnit.Framework;
using TMPro;
using UnityEngine;

public class CountryUI : MonoBehaviour, IManageable
{
    [SerializeField] private GameObject countryUIContainer;

    [Header("Country UI Elements")]
    [SerializeField] private TextMeshProUGUI countryNameText;
    [SerializeField] private TextMeshProUGUI countryDescription;

    public void Selected()
    {
        countryUIContainer.SetActive(true);
        SetUI();
    }
    public void Deselected()
    {
        countryUIContainer.SetActive(false);
    }

    void SetUI()
    {
        Country country = ProvinceManager.Instance.GetSelectedProvince().country;

        if(country == null)
        {
            return;
        }
        countryNameText.text = country.countryName;

        countryDescription.text = country.countryDescription;
    }
}
