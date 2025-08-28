using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    [SerializeField]
    string popName;

    int money = 20;

    int costOfLiving = 1; // Money spent on essentials each consumption cycle

    Dictionary<GoodSO, ConcreteGood> goodsOwned = new Dictionary<GoodSO, ConcreteGood>();

    void Start()
    {
        // Initialize goodsOwned with all GoodSOs set to null
        foreach (GoodSO goodType in GoodsManager.instance.GetGoodSOs())
        {
            goodsOwned.Add(goodType, null);
        }

        // Subscribe to market consumption tick event
        MarketManager.instance.OnMarketConsumptionTick += MarketManager_OnMarketConsumptionTick;
    }

    public string GetPopName()
    {
        return popName;
    }

    // Gets the good from the market, deducts money from the pop, and adds money to the good's owner
    public bool PurchaseGood(ConcreteGood good)
    {
        if (good.GetPrice() > money)
            return false; // Cannot afford the good

        if (!good.GetGoodType().IsProductionGood())
            goodsOwned[good.GetGoodType()] = good; // Production goods are not stored

        money -= good.GetPrice();

        if (good.GetOwner() != null) // If the good has an owner, pay them
            good.GetOwner().IncreaseMoney(good.GetPrice());

        // If the good is essential or for production, add its price to the cost of living
        if (good.GetGoodType().IsEssentialGood() || good.GetGoodType().IsProductionGood())
        {
            costOfLiving += good.GetPrice();
        }
        return true;
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
    }

    public int GetMoney() => money;
    public void IncreaseMoney(int money)
    {
        money += money;
    }
    public int GetCostOfLiving() => costOfLiving;

    private void MarketManager_OnMarketConsumptionTick(object sender, EventArgs e)
    {
        costOfLiving = 0;
    }
}