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
                List<ConcreteGood> inputGoodsNeeded = market.GetSeveralGoodsByType(input.Key, input.Value, out totalPrice);
                if (sitePop.GetMoney() < totalPrice)
                {
                    sitePop.AddToLog($"Cannot afford input goods for production. Needed: {totalPrice}, Available: {sitePop.GetMoney()}");
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

            // If the pop is in subsistence mode, ensure at least one good is reserved for their own use
            if (sitePop.IsInSubsistenceMode() && amountToProduce > 0 && output.Key.IsEssentialGood())
            {
                if(!sitePop.HasGood(output.Key))
                {
                    sitePop.AddSubsistenceGood(new ConcreteGood(output.Key, 0, sitePop));
                    sitePop.AddToLog($"Reserved 1 of {output.Key.GetGoodName()} for subsistence");
                    amountToProduce -= 1;
                }
            }

            if (amountToProduce <= 0)
            {
                return; // No goods left to produce for market
            }

            GoodSO goodType = output.Key;
            int totalPrice = CalculateGoodPrice(market, goodType);
            int pricePerGood = totalPrice / amountToProduce;

            for (int i = 0; i < amountToProduce; i++)
            {
                ConcreteGood producedGood = new ConcreteGood(goodType, pricePerGood, sitePop);
                market.AddGoodToMarket(producedGood);
            }

            sitePop.AddToLog($"Produced {amountToProduce} of {goodType.GetGoodName()} at {siteName} for {pricePerGood} each");
        }
    }

    protected int CalculateGoodPrice(Market market, GoodSO goodType)
    {
        int goodPrice = 0;

        // Basic pricing algorithm: base price modified by supply and demand:

        // If good is not in demand, set price to lowest recorded price
        if (!market.IsGoodInDemand(goodType))
        {
            int lowestRecordedPrice = market.GetLowestPriceRecorded(goodType);

            goodPrice = lowestRecordedPrice;
        }
        else // If good is in demand, set price to highest recorded price plus or minus a small random factor
        {
            int highestRecordedPrice = market.GetHighestPriceRecorded(goodType);
            
            goodPrice = highestRecordedPrice + Mathf.RoundToInt(((float)highestRecordedPrice * Random.Range(0f, 0.4f)));
        }

        if (goodPrice < sitePop.GetCostOfLiving())
        {
            goodPrice = sitePop.GetCostOfLiving(); // Ensure price covers cost of living
        }

        if (goodPrice < 1)
        {
            goodPrice = 1; // Ensure price is at least 1
        }

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
