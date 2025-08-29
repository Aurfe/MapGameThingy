using UnityEngine;

public class ConcreteGood
{
    [SerializeField]
    GoodSO goodData;

    int usesLeft;
    int price;
    int timeInMarket = 0;

    private Pop goodOwner;

    public string GetName()
    {
        return goodData.GetName();
    }

    public ConcreteGood(GoodSO goodData, int price, Pop goodOwner) : this(goodData, price)
    {
        this.goodOwner = goodOwner;
    }
    public ConcreteGood(GoodSO goodData, int price) : this(goodData)
    {
        this.price = price;
    }
    public ConcreteGood(GoodSO goodData)
    {
        this.goodData = goodData;
        usesLeft = goodData.IsProductionGood() ? -1 : goodData.GetConsumptionUses();
    }

    public bool UseGood()
    {
        usesLeft--;
        if (usesLeft > 0)
        {
            return true; // Successfully used the good
        }
        return false; // No uses left
    }

    public void IncrementTimeInMarket()
    {
        timeInMarket++;

        price -= Mathf.RoundToInt( price * (timeInMarket / 100));

        if (price < 1)
            price = 1;
    }

    public GoodSO GetGoodType() => goodData;
    public int GetPrice() => price;
    public Pop GetOwner() => goodOwner;
}
