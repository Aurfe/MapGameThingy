using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    [SerializeField]
    Transform goodListContent;

    [SerializeField]
    Transform goodItemPrefab;

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
