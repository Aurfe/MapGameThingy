using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pop : MonoBehaviour
{
    [SerializeField]
    string popName;

    [SerializeField]
    int popMoney = 20;

    int costOfLiving = 5; // Money spent on essentials each consumption cycle

    Dictionary<GoodSO, ConcreteGood> goodsOwned = new Dictionary<GoodSO, ConcreteGood>();

    [SerializeField]
    bool subsistenceMode; // If true, pop will take a good they produce
    ConcreteGood subsistingGood; // The good a pop will put asside if in subsistence mode

    PopLog popLog;

    void Start()
    {
        popLog = GetComponent<PopLog>();
        // Initialize goodsOwned with all GoodSOs set to null
        foreach (GoodSO goodType in GoodsManager.instance.GetGoodSOs())
        {
            goodsOwned.Add(goodType, null);
        }
    }

    public string GetPopName()
    {
        return popName;
    }

    // Gets the good from the market, deducts money from the pop, and adds money to the good's owner
    public bool PurchaseGood(ConcreteGood good)
    {
        // If the pop cannot afford the good, return false
        if (good.GetPrice() > popMoney)
            return false;

        if (!(good.GetGoodType().IsEssentialGood() || good.GetGoodType().IsProductionGood()) && good.GetPrice() > (popMoney * 0.5))
        {
            return false; // Pops will not buy non-essential goods that cost more than half their money
        }
        if ((good.GetGoodType().IsEssentialGood() || good.GetGoodType().IsProductionGood()) && good.GetPrice() > (popMoney * 0.8))
        {
            return false; // Pops will not buy essential goods that cost more than 80% of their money
        }


        if (!good.GetGoodType().IsProductionGood())
            goodsOwned[good.GetGoodType()] = good; // Production goods are not stored

        if (good.GetOwner() != null) // If the good has an owner, pay them
        {
            good.GetPurchased(this);
        }
        else
        // If the good has no owner, just deduct the money from the pop
        {
            popMoney -= good.GetPrice();
        }

        // If the good is essential or for production, add its price to the cost of living
        if (good.GetGoodType().IsEssentialGood() || good.GetGoodType().IsProductionGood())
        {
            costOfLiving += good.GetPrice();
        }

        AddToLog($"{popName} purchased {good.GetName()} for {good.GetPrice()} from {(good.GetOwner() != null ? good.GetOwner().GetPopName() : "null")}");

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

    // Consumes one use of each good owned by the pop. If a good runs out of uses, it is removed.
    public void ConsumeGoods()
    {
        foreach (var key in new List<GoodSO>(goodsOwned.Keys))
        {
            if (goodsOwned[key] == null) continue;

            if (!goodsOwned[key].UseGood())
            {
                goodsOwned[key] = null;
            }
        }

        // Slots the subsistence good if in subsistence mode
        if (subsistenceMode)
            UseSubsistenceGood();
    }

    public void AddToLog(string entry)
    {
        string timestamp = $"[{MarketManager.instance.GetMarketTickNumber()}] ";
        popLog.AddLogEntry(timestamp + entry);
    }
    public List<string> GetPopLog()
    {
        return popLog.GetLogEntries();
    }
    public void ResetCostOfLiving()
    {
        costOfLiving = 5;
    }
    public int GetMoney() => popMoney;
    public int GetCostOfLiving() => costOfLiving > 0 ? costOfLiving : 1;
    public bool IsInSubsistenceMode() => subsistenceMode;
    public void ChangeMoney(int money)
    {
        popMoney += money;
    }
}