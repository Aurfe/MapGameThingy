using TMPro;
using UnityEngine;

public class PopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popNameTag;
    [SerializeField] TextMeshProUGUI popWealthTag;

    [SerializeField] TextMeshProUGUI foodGoodIcon;
    [SerializeField] TextMeshProUGUI toolsGoodIcon;

    public void SetUI(Pop pop)
    {
        popNameTag.text = pop.GetPopName();
        popWealthTag.text = pop.GetMoney().ToString();
        SetGoodDemandIcons(pop);
    }

    private void SetGoodDemandIcons(Pop pop)
    {
        if(pop.HasGood(GoodsManager.instance.GetGoodSOByName("Food")))
        {
            foodGoodIcon.color = Color.white;
        }
        else
        {
            foodGoodIcon.color = Color.black;
        }

        if(pop.HasGood(GoodsManager.instance.GetGoodSOByName("Tools")))
        {
            toolsGoodIcon.color = Color.white;
        }
        else
        {
            toolsGoodIcon.color = Color.black;
        }
    }
}
