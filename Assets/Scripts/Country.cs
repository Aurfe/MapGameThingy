using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    public event EventHandler OnTreasuryChanged;

    public string countryName;

    public Color32 countryColor;

    public string countryDescription;

    private int treasury = 0;

    private int taxRate = 2;

    List<Province> provinces = new List<Province>();

    private void Start()
    {
        MarketManager.instance.OnMarketUpdate += MarketManager_OnMarketUpdate;
    }

    private void MarketTick()
    {
        Debug.Log($"Market Tick for Country: {countryName}");
        TaxProvinces();
    }

    public void TaxProvinces()
    {
        foreach (Province province in provinces)
        {
            treasury += province.TaxPops();
        }
    }

    public int GetTaxRate()
    {
        return taxRate;
    }

    public void AddProvince(Province province)
    {
        provinces.Add(province);
    }

    public int GetTreasury()
    {
        return treasury;
    }

    public void AdjustTreasury(int amount)
    {
        treasury += amount;
        OnTreasuryChanged?.Invoke(this, EventArgs.Empty);
    }
    private void MarketManager_OnMarketUpdate(object sender, EventArgs e)
    {
        MarketTick();
    }
}
