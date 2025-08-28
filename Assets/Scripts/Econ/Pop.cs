using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pop : MonoBehaviour
{
    [SerializeField]
    string popName;

    int popMoney = 20;

    int costOfLiving = 1; // Money spent on essentials each consumption cycle

    Dictionary<GoodSO, ConcreteGood> goodsOwned = new Dictionary<GoodSO, ConcreteGood>();

    [SerializeField]
    bool subsistenceMode; // If true, pop will take a good they produce
    ConcreteGood subsistingGood; // The good a pop will put asside if in subsistence mode

    void Start()
    {
        // Initialize goodsOwned with all GoodSOs set to null
        foreach (GoodSO goodType in GoodsManager.instance.GetGoodSOs())
        {
            goodsOwned.Add(goodType, null);
        }

        // Subscribe to market consumption tick event
        MarketManager.instance.OnMarketUpdate += MarketManager_OnMarketUpdate;
    }

    public string GetPopName()
    {
        return popName;
    }

    // Gets the good from the market, deducts money from the pop, and adds money to the good's owner
    public bool PurchaseGood(ConcreteGood good)
    {
        if (good.GetPrice() > popMoney)
            return false; // Cannot afford the good

        if (!good.GetGoodType().IsProductionGood())
            goodsOwned[good.GetGoodType()] = good; // Production goods are not stored

        popMoney -= good.GetPrice();

        if (good.GetOwner() != null) // If the good has an owner, pay them
        {
            good.GetOwner().IncreaseMoney(good.GetPrice());
        }

        // If the good is essential or for production, add its price to the cost of living
        if (good.GetGoodType().IsEssentialGood() || good.GetGoodType().IsProductionGood())
        {
            costOfLiving += good.GetPrice();
        }

        return true;
    }

    public void AddSubsistenceGood(ConcreteGood good)
    {
        subsistingGood = good;
    }

    private void UseSubsistenceGood()
    {
        if (subsistingGood != null)
        {
            goodsOwned[subsistingGood.GetGoodType()] = subsistingGood;
            subsistingGood = null;
        }
    }

    public bool HasGood(GoodSO goodType)
    {
        return goodsOwned[goodType] != null;
    }

    // Consumes one use of each good owned by the pop. If a good runs out of uses and is not a production good, it is removed.
    public void ConsumeGoods()
    {
        foreach (var key in new List<GoodSO>(goodsOwned.Keys))
        {
            if (goodsOwned[key] == null) continue;

            if (!goodsOwned[key].UseGood() && !key.IsProductionGood())
            {
                goodsOwned[key] = null;
            }
        }

        // Slots the subsistence good if in subsistence mode
        if (subsistenceMode)
            UseSubsistenceGood();
    }

    public int GetMoney() => popMoney;
    public int GetCostOfLiving() => costOfLiving > 0 ? costOfLiving : 1;
    public bool IsInSubsistenceMode() => subsistenceMode;
    public void IncreaseMoney(int money)
    {
        popMoney += money;
    }
    private void MarketManager_OnMarketUpdate(object sender, EventArgs e)
    {
        costOfLiving = 0;
    }
}