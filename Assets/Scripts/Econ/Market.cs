using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Market : SerializedMonoBehaviour
{
    MarketUI marketUI;
    [SerializeField]
    MarketLog log;

    [SerializeField]
    Dictionary<GoodSO, List<ConcreteGood>> goodsInMarket = new Dictionary<GoodSO, List<ConcreteGood>>();
    [SerializeField]
    Dictionary<GoodSO, GoodMarketData> marketDataByGoodType = new Dictionary<GoodSO, GoodMarketData>();

    [SerializeField]
    List<Province> provincesInMarket;

    public void Start()
    {
        //Subscribe to market tick events
        MarketManager.instance.OnMarketUpdate += MarketManager_OnMarketUpdate;

        marketUI = GameObject.Find("MarketUI Manager").GetComponent<MarketUI>();

        // Initialize goodsInMarket with all GoodSOs and empty lists
        foreach (GoodSO goodType in GoodsManager.instance.GetGoodSOs())
        {
            goodsInMarket.Add(goodType, new List<ConcreteGood>());
            marketDataByGoodType.Add(goodType, new GoodMarketData());
        }
    }

    //A market tick consists of:
    private void MarketTick()
    {
        ClearMarketData();
        ResetCostOfLiving();
        ConsumeGoods();
        DecreaseGoodPrices();
        ProduceGoods();
        UpdateMarketUI();
        log.RefreshLogUI();

        // For debugging, print market data for each good type
        // DebugGoodData(GoodsManager.instance.GetGoodSOByName("Clothes"));
    }

    private void ResetCostOfLiving()
    {
        List<Pop> pops = GetAllPops();
        foreach (Pop pop in pops)
        {
            pop.ResetCostOfLiving();
        }
    }

    //Iterate through each pop in the market's provinces and have them consume goods
    private void ConsumeGoods()
    {
        List<Pop> pops = GetAllPops();

        foreach (Pop pop in pops)
        {
            pop.ConsumeGoods();

            //For now, just have each pop consume one of each good type if available
            foreach (GoodSO goodType in GoodsManager.instance.GetGoodSOs())
            {
                if (goodType.IsProductionGood())
                {
                    continue; //Pops don't consume production goods
                }

                // If the pop doesn't already have this good and there are goods of this type in the market
                if (pop.HasGood(goodType) == false && goodsInMarket[goodType].Count > 0)
                {
                    // Purchase the last good in the list (to simulate a stack/queue)
                    // Good lists are sorted by price, so this is the cheapest good available
                    int goodIndex = goodsInMarket[goodType].Count - 1;
                    ConcreteGood goodToPurchase = goodsInMarket[goodType][goodIndex];

                    if(pop.PurchaseGood(goodToPurchase))
                    {
                        RemoveGoodFromMarket(goodToPurchase);
                    }

                    marketDataByGoodType[goodType].RecordSale(goodToPurchase.GetPrice());
                }
            }
        }
    }

    //Iterate through each good in the market and decrease its price based on time in market
    private void DecreaseGoodPrices()
    {
        foreach (var goodList in goodsInMarket.Values)
        {
            // Iterate backwards to avoid issues when removing items
            for (int i = goodList.Count - 1; i >= 0; i--)
            {
                goodList[i].IncrementTimeInMarket(this);
            }
        }
    }

    //Iterate through each province and their sites
    //Call the ProduceGood method for each site
    private void ProduceGoods()
    {
        // Randomize the order of provinces to ensure fairness in production
        var rnd = new System.Random();
        var randomizedProvinces = provincesInMarket.OrderBy(item => rnd.Next());
        provincesInMarket = randomizedProvinces.ToList();

        foreach (Province province in provincesInMarket)
        {
            foreach (ConcreteSite site in province.GetSiteList())
            {
                site.ProduceGood(this);
                SortGoodLists();
            }
        }
    }

    public void AddGoodToMarket(ConcreteGood goodToAdd)
    {
        if (goodToAdd == null)
        {
            return; // No good produced
        }

        goodsInMarket[goodToAdd.GetGoodType()].Add(goodToAdd);

        log.AddLogEntry("Added " + goodToAdd.GetName() + " to market at price " + goodToAdd.GetPrice());
    }

    public void RemoveGoodFromMarket(ConcreteGood goodToRemove)
    {
        if (goodToRemove == null)
        {
            return; // No good to remove
        }
        goodsInMarket[goodToRemove.GetGoodType()].Remove(goodToRemove);

        log.AddLogEntry("Removed " + goodToRemove.GetName() + " from market at price " + goodToRemove.GetPrice());
    }

    //Get a list of all pops in the market's provinces
    private List<Pop> GetAllPops()
    {
        List<Pop> allPops = new List<Pop>();
        foreach(Province province in provincesInMarket)
        {
            allPops.AddRange(province.GetPopList());
        }

        //Randomize the list of pops to ensure fairness in consumption
        var rnd = new System.Random();
        var randomized = allPops.OrderBy(item => rnd.Next());

        return randomized.ToList();
    }

    //Generate a list of goods in the market of the specified type
    public void SpecifyList(string goodTypeName)
    {
        GoodSO goodType = GoodsManager.instance.GetGoodSOByName(goodTypeName);
        if (goodType != null)
        {
            marketUI.GenerateGoodList(GetGoodsOfTypeInMarket(goodType));
        }
        else if (goodTypeName == "All")
        {
            UpdateMarketUI();
        }
        else
        {
            Debug.LogWarning("Good type not found: " + goodTypeName);
        }
    }

    //Sort the lists of goods in the market by price (hight to low)
    public void SortGoodLists()
    {
        foreach (var goodList in goodsInMarket.Values)
        {
            goodList.Sort((a, b) => b.GetPrice().CompareTo(a.GetPrice()));
        }
    }    

    public void UpdateMarketUI()
    {
        marketUI.GenerateGoodList(GetAllGoodsInMarket());
    }

    public int GetTotalPopWealth()
    {
        int totalWealth = 0;
        List<Pop> pops = GetAllPops();
        foreach(Pop pop in pops)
        {
            totalWealth += pop.GetMoney();
        }
        return totalWealth;
    }
    //Return a list of all goods currently in the market
    public List<ConcreteGood> GetAllGoodsInMarket()
    {
        List<ConcreteGood> allGoods = new List<ConcreteGood>();

        foreach (List<ConcreteGood> goodList in goodsInMarket.Values)
        {
            allGoods.AddRange(goodList);
        }

        return allGoods;
    }

    public List<ConcreteGood> GetGoodsOfTypeInMarket(GoodSO goodType)
    {
        if (goodsInMarket.ContainsKey(goodType))
        {
            return new List<ConcreteGood>(goodsInMarket[goodType]);
        }
        else
        {
            return new List<ConcreteGood>();
        }
    }
    public int GetAmountOfGoodsInMarket(GoodSO goodType)
    {
        if (goodsInMarket.ContainsKey(goodType))
        {
            return goodsInMarket[goodType].Count;
        }
        return 0;
    }
    // Gets the single cheapest good of a certain type from the market
    public ConcreteGood GetGoodOfTypeInMarket(GoodSO goodType)
    {
        if (goodsInMarket.ContainsKey(goodType) && goodsInMarket[goodType].Count > 0)
        {

            return goodsInMarket[goodType][goodsInMarket[goodType].Count - 1];
        }
        return null;
    }
    // Get a specified number of goods of a certain type from the market, returning the total price
    public List<ConcreteGood> GetSeveralGoodsByType(GoodSO goodType, int numberOfGoodsRequested, out int totalPrice)
    {
        totalPrice = 0;
        // Check if the good type exists and there are enough goods available
        if (goodsInMarket.ContainsKey(goodType) && goodsInMarket[goodType].Count >= numberOfGoodsRequested)
        {
            List<ConcreteGood> selectedGoods = new List<ConcreteGood>();
            // Select the cheapest goods available and calculate total price
            for (int i = 0; i < numberOfGoodsRequested; i++)
            {
                ConcreteGood good = GetGoodOfTypeInMarket(goodType);
                selectedGoods.Add(good);
                totalPrice += good.GetPrice();
            }

            return selectedGoods;
        }
        else
        {
            return new List<ConcreteGood>();
        }
    }
    private void DebugGoodData(GoodSO goodData)
    {
        GoodMarketData data = marketDataByGoodType[goodData];

        string log = "";
        log += $"---- Market Data {MarketManager.instance.GetMarketTickNumber()} ----\n";
        log += $"Good: {goodData.GetName()}\n";
        log += $" - Amount Sold: {data.GetAmountSold()}\n";
        log += $" - Highest Price: {data.GetHighestPrice()}\n";
        log += $" - Lowest Price: {data.GetLowestPrice()}\n";
        log += $" - Average Price: {data.GetAveragePrice()}\n";
        log += $" - In Market: {GetAmountOfGoodsInMarket(goodData)}\n";
        Debug.Log(log);
    }

    //A good is considered "in demand" if there are no copies of it in the market
    //This is a very basic measure and can be improved in the future
    public bool IsGoodInDemand(GoodSO goodType)
    {
        if (marketDataByGoodType.ContainsKey(goodType))
        {
            if(GetAmountOfGoodsInMarket(goodType) > 0)
            {
                return false;
            }
        }
        return true;
    }
    public int GetNumberOfSalesRecorded(GoodSO goodType)
    {
        if (marketDataByGoodType.ContainsKey(goodType))
        {
            return marketDataByGoodType[goodType].GetAmountSold();
        }
        return 0;
    }
    public int GetHighestPriceRecorded(GoodSO goodType)
    {
        if (marketDataByGoodType.ContainsKey(goodType))
        {
            return marketDataByGoodType[goodType].GetHighestPrice();
        }
        return 0;
    }
    public int GetLowestPriceRecorded(GoodSO goodType)
    {
        if (marketDataByGoodType.ContainsKey(goodType))
        {
            return marketDataByGoodType[goodType].GetLowestPrice();
        }
        return 0;
    }
    private void ClearMarketData()
    {
        foreach (GoodMarketData data in marketDataByGoodType.Values)
        {
            data.ClearData();
        }
    }
    private void MarketManager_OnMarketUpdate(object sender, EventArgs e)
    {
        MarketTick();
    }
}