using TMPro;
using UnityEngine;

public class PopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popNameTag;
    [SerializeField] TextMeshProUGUI popWealthTag;

    public void SetUI(Pop pop)
    {
        popNameTag.text = pop.GetPopName();
        popWealthTag.text = pop.GetMoney().ToString();
    }
}
