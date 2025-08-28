using UnityEngine;
using TMPro;

public class GoodUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI goodNameTag;
    [SerializeField]
    TextMeshProUGUI goodCostTag;


    public void SetUI(ConcreteGood good)
    {
        goodNameTag.text = good.GetName();
        goodCostTag.text = good.GetPrice().ToString();
    }
}
