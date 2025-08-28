using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ConcreteSite : MonoBehaviour
{
    [SerializeField]
    protected string siteName;

    [SerializeField]
    protected SiteSO siteData;

    [SerializeField]
    protected int baseProductionBonus = 0;

    protected Pop sitePop;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            sitePop = child.gameObject.GetComponent<Pop>();
        }
    }

    public string GetName()
    {
        return siteName;
    }

    public Pop GetPop()
    {
        return sitePop;
    }    

    public void AssignPop(Pop pop)
    {
        sitePop = pop;
    }

    //Produce Goods based on the site's production method and add them to the market
    public virtual void ProduceGood(Market market)
    {
        ProductionMethod productionMethod = siteData.GetProductionMethod();
        int productionBonus = CalculateProductionBonus();

        // Check if there are input goods required for production
        if (productionMethod.GetInputGoods() != null)
        {
            // Purchase input goods from market
            foreach (var input in productionMethod.GetInputGoods())
            {
                // Check if the market has enough of the input good and get total price
                int totalPrice = 0;
                List<ConcreteGood> inputGoodsNeeded = market.GetNumberOfGoodsByType(input.Key, input.Value, out totalPrice);

                if (sitePop.GetMoney() < totalPrice)
                {
                    return; // Cannot afford input goods, abort production
                }

                // Purchase each input good
                foreach (ConcreteGood good in inputGoodsNeeded)
                {
                    sitePop.PurchaseGood(good);
                    market.RemoveGoodFromMarket(good);
                }
            }
        }

        // Iterate through each output good and produce the specified amount
        foreach (var output in productionMethod.GetOutputGoods())
        {
            int amountToProduce = output.Value + productionBonus;
            Debug.Log($"Producing {amountToProduce} of {output.Key.GetGoodName()} at {siteName}");

            // If the pop is in subsistence mode, ensure at least one good is reserved for their own use
            if (sitePop.IsInSubsistenceMode() && amountToProduce > 0)
            {
                if(!sitePop.HasGood(output.Key))
                {
                    sitePop.AddSubsistenceGood(new ConcreteGood(output.Key, 0, sitePop));
                    amountToProduce -= 1;
                }
            }

            if (amountToProduce <= 0)
            {
                return; // No goods left to produce for market
            }

            GoodSO goodType = output.Key;
            int pricePerGood = CalculateGoodPrice(amountToProduce);

            for (int i = 0; i < amountToProduce; i++)
            {
                ConcreteGood producedGood = new ConcreteGood(goodType, pricePerGood, sitePop);
                market.AddGoodToMarket(producedGood);
            }
        }
    }

    protected int CalculateGoodPrice(int amountOfGoods)
    {
        int goodPrice = 0;
        goodPrice += sitePop.GetCostOfLiving();
        goodPrice = goodPrice / amountOfGoods; // Distribute cost across all goods produced
        goodPrice = Mathf.Max(goodPrice, 1); // Ensure price is at least 1


        return goodPrice;
    }

    // How many extra goods are produced
    protected virtual int CalculateProductionBonus()
    {
        int totalProductionBonus = baseProductionBonus;
        // Bonus for having tools good
        if (sitePop.HasGood(GoodsManager.instance.GetGoodSOByName("Tools")))
        {
            totalProductionBonus += 1;
        }

        return 0;
    }
}
