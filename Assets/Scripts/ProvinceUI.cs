using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProvinceUI : MonoBehaviour, IManageable
{
    public static ProvinceUI Instance;

    [SerializeField] GameObject provinceUIContainer;

    [SerializeField] List<GameObject> uiMenus = new List<GameObject>();
    int uiMenuIndex = 0;

    [SerializeField] TextMeshProUGUI provinceNameTag;

    [SerializeField] Transform siteListContent;

    [SerializeField] Transform siteItemPrefab;

    [SerializeField] Transform popListContent;

    [SerializeField] Transform popItemPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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
        GenerateLists();
    }

    public void GenerateLists()
    {
        if (ProvinceManager.Instance.GetSelectedProvince() == null)
            return;
        GenerateSiteListUI();
        GeneratePopListUI();
    }

    private void GenerateSiteListUI()
    {
        ClearListUI(siteListContent);

        Province province = ProvinceManager.Instance.GetSelectedProvince();

        foreach (ConcreteSite site in province.GetSiteList())
        {
            Transform siteItem = Instantiate(siteItemPrefab, siteListContent);

            siteItem.gameObject.GetComponent<SiteUI>().SetSiteUI(site);
        }
    }

    private void GeneratePopListUI()
    {
        ClearListUI(popListContent);

        Province province = ProvinceManager.Instance.GetSelectedProvince();

        foreach (Pop pop in province.GetPopList())
        {
            Transform popItem = Instantiate(popItemPrefab, popListContent);

            popItem.gameObject.GetComponent<PopUI>().SetUI(pop);
        }
    }

    private void ClearListUI(Transform list)
    {
        foreach (Transform child in list)
        {
            Destroy(child.gameObject);
        }
    }

    public void changeOpenMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= uiMenus.Count)
        {
            Debug.LogError("Invalid menu index");
            return;
        }
        uiMenus[uiMenuIndex].SetActive(false);
        uiMenuIndex = menuIndex;
        uiMenus[uiMenuIndex].SetActive(true);
    }
}