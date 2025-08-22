using UnityEngine;
using TMPro;

public class GoodUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI goodNameTag;

    public void SetUI(Good good)
    {
        goodNameTag.text = good.GetName();
    }
}
