using UnityEngine;

public class GoodMarketData
{
    int highestPrice = 0;
    int lowestPrice = 0;
    int amountSold = 0;
    int totalSales = 0;

    public void ClearData()
    {
        highestPrice = 0;
        lowestPrice = 0;
        amountSold = 0;
    }
    public void RecordSale(int price)
    {
        totalSales += price;
        amountSold++;
        if (price > highestPrice)
            highestPrice = price;
        if (price < lowestPrice || lowestPrice == 0)
            lowestPrice = price;
    }

    public int GetHighestPrice() => highestPrice;
    public int GetLowestPrice() => lowestPrice;
    public int GetAmountSold() => amountSold;
    public int GetAveragePrice() => amountSold == 0 ? 0 : totalSales / amountSold;
}
