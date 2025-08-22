using UnityEngine;

public class MarketUI : MonoBehaviour
{
    [SerializeField]
    Transform goodListContent;

    [SerializeField]
    Transform goodItemPrefab;

    public void GenerateGoodList(Market market)
    {
        ClearGoodListUI();
        foreach(Good good in market.GetGoodsInMarket())
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
