using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Market : SerializedMonoBehaviour
{
    MarketUI marketUI;

    [SerializeField]
    Dictionary<GoodSO, List<ConcreteGood>> goodsInMarket = new Dictionary<GoodSO, List<ConcreteGood>>();
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

    //Return a list of all goods currently in the market
    public List<ConcreteGood> GetAllGoodsInMarket()
    {
        List<ConcreteGood> allGoods = new List<ConcreteGood>();

        foreach(List<ConcreteGood> goodList in goodsInMarket.Values)
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
    public int GetNumberOfGoodsInMarket(GoodSO goodType)
    {
        if (goodsInMarket.ContainsKey(goodType))
        {
            return goodsInMarket[goodType].Count;
        }
        return 0;
    }
    public ConcreteGood GetGoodOfTypeInMarket(GoodSO goodType)
    {
        if (goodsInMarket.ContainsKey(goodType) && goodsInMarket[goodType].Count > 0)
        {
            return goodsInMarket[goodType][goodsInMarket[goodType].Count - 1];
        }
        return null;
    }
    // Get a specified number of goods of a certain type from the market, returning the total price
    public List<ConcreteGood> GetSeveralGoodsByType(GoodSO goodType, int number, out int totalPrice)
    {
        totalPrice = 0;
        // Check if the good type exists and there are enough goods available
        if (goodsInMarket.ContainsKey(goodType) && goodsInMarket[goodType].Count >= number)
        {
            // Select the specified number of goods from the end of the list (they will be the cheapest due to sorting)
            List<ConcreteGood> selectedGoods = goodsInMarket[goodType].GetRange(goodsInMarket[goodType].Count - number, number);
            foreach (ConcreteGood good in selectedGoods)
            {
                totalPrice += good.GetPrice();
            }

            return selectedGoods;
        }
        else
        {
            return new List<ConcreteGood>();
        }
    }

    //A market tick consists of:
    private void MarketTick()
    {
        ResetCostOfLiving();
        ConsumeGoods();
        DecreaseGoodPrices();
        ProduceGoods();
        UpdateMarketUI();
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
            ClearMarketData();

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

                    pop.PurchaseGood(goodToPurchase);
                    goodsInMarket[goodType].RemoveAt(goodIndex);

                    marketDataByGoodType[goodType].RecordSale(goodToPurchase.GetPrice());
                }
            }
        }
        ProvinceUI.Instance.GenerateLists();
    }

    //Iterate through each good in the market and decrease its price based on time in market
    private void DecreaseGoodPrices()
    {
        foreach (var goodList in goodsInMarket.Values)
        {
            foreach (ConcreteGood good in goodList)
            {
                good.IncrementTimeInMarket();
            }
        }
    }

    //Iterate through each province and their sites
    //Call the ProduceGood method for each site
    private void ProduceGoods()
    {
        foreach (Province province in provincesInMarket)
        {
            foreach (ConcreteSite site in province.GetSiteList())
            {
                site.ProduceGood(this);
            }
        }

        SortGoodLists();
        ProvinceUI.Instance.GenerateLists();
    }

    public void AddGoodToMarket(ConcreteGood goodToAdd)
    {
        if (goodToAdd == null)
        {
            return; // No good produced
        }

        goodsInMarket[goodToAdd.GetGoodType()].Add(goodToAdd);
    }

    public void RemoveGoodFromMarket(ConcreteGood goodToRemove)
    {
        if (goodToRemove == null)
        {
            return; // No good to remove
        }
        goodsInMarket[goodToRemove.GetGoodType()].Remove(goodToRemove);
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
        marketUI.SetTotalMarketWealth(GetTotalPopWealth());
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

    //A good is considered "in demand" if there are no copies of it in the market
    //This is a very basic measure and can be improved in the future
    public bool IsGoodInDemand(GoodSO goodType)
    {
        if (marketDataByGoodType.ContainsKey(goodType))
        {
            if(GetNumberOfGoodsInMarket(goodType) > 0)
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