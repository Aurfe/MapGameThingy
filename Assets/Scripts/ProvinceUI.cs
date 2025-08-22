using System;
using TMPro;
using UnityEngine;

public class ProvinceUI : MonoBehaviour, IManageable
{
    [SerializeField] GameObject provinceUIContainer;

    [SerializeField] TextMeshProUGUI provinceNameTag;

    [SerializeField] Transform siteListContent;

    [SerializeField] Transform siteItemPrefab;

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
        provinceNameTag.text = ProvinceManager.Instance.GetSelectedProvince().GetProvinceName();
        GenerateSiteListUI();
    }

    private void GenerateSiteListUI()
    {
        ClearSiteListUI();

        Province province = ProvinceManager.Instance.GetSelectedProvince();

        foreach (ConcreteSite site in province.GetSiteList())
        {
            Transform siteItem = Instantiate(siteItemPrefab, siteListContent);

            siteItem.gameObject.GetComponent<SiteUI>().SetSiteUI(site);
        }
    }

    private void ClearSiteListUI()
    {
        foreach (Transform child in siteListContent)
        {
            Destroy(child.gameObject);
        }
    }
}
