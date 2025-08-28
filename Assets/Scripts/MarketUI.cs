using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    [SerializeField]
    Transform goodListContent;

    [SerializeField]
    Transform goodItemPrefab;

    [SerializeField]
    TextMeshProUGUI totalMarketWealth;

    public void SetTotalMarketWealth(int wealth)
    {
        totalMarketWealth.text = $"{wealth}";
    }

    public void GenerateGoodList(List<ConcreteGood> goods)
    {
        ClearGoodListUI();
        foreach(ConcreteGood good in goods)
        {
            Transform goodItem = Instantiate(goodItemPrefab, goodListContent);
            goodItem.GetComponent<GoodUI>().SetUI(good);
        }
    }

    private void ClearGoodListUI()
    {
        foreach(Transform child in goodListContent)
        {
            Destroy(child.gameObject);
        }
    }
}
