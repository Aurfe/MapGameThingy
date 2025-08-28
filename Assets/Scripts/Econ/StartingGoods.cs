using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StartingGoods : MonoBehaviour
{
    [SerializeField]
    Market market;

    [SerializeField]
    int startingFoodAmount = 10;
    [SerializeField]
    int startingToolAmount = 5;
    [SerializeField]
    int startingIronAmount = 3;
    void Start()
    {
        List<ConcreteGood> startingGoods = new List<ConcreteGood>();

        for (int i = 0; i < startingFoodAmount; i++)
        {
            ConcreteGood food = new ConcreteGood(GoodsManager.instance.GetGoodSOByName("Food"), 5);
            startingGoods.Add(food);
        }

        for (int i = 0; i < startingToolAmount; i++)
        {
            ConcreteGood tool = new ConcreteGood(GoodsManager.instance.GetGoodSOByName("Tools"), 5);
            startingGoods.Add(tool);
        }

        for (int i = 0; i < startingIronAmount; i++)
        {
            ConcreteGood iron = new ConcreteGood(GoodsManager.instance.GetGoodSOByName("Iron"), 5);
            startingGoods.Add(iron);
        }

        foreach (ConcreteGood good in startingGoods)
        {
            market.AddGoodToMarket(good);
        }

        market.UpdateMarketUI();
    }
}
