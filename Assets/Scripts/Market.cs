using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    MarketUI marketUI;

    List<Good> goodsInMarket = new List<Good>();

    [SerializeField]
    List<Province> provincesInMarket;

    public void Start()
    {
        marketUI = GameObject.Find("MarketUI Manager").GetComponent<MarketUI>();
    }

    public List<Good> GetGoodsInMarket()
    {
        return goodsInMarket;
    }

    //Iterate through each province and their sites
    //Call the ProduceGood method for each site
    public void ProduceGoods()
    {
        foreach(Province province in provincesInMarket)
        {
            foreach(ConcreteSite site in province.GetSiteList())
            {
                AddGoodToMarket(site.ProduceGood());
            }
        }

        marketUI.GenerateGoodList(this);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ProduceGoods();
        }
    }

    public void AddGoodToMarket(Good goodToAdd)
    {
        goodsInMarket.Add(goodToAdd);
    }
}