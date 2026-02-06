using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popNameTag;
    [SerializeField] TextMeshProUGUI popWealthTag;

    [SerializeField] TextMeshProUGUI foodGoodIcon;
    [SerializeField] TextMeshProUGUI toolsGoodIcon;
    [SerializeField] TextMeshProUGUI clothesGoodIcon;

    Pop sitePop;

    public void SetUI(Pop pop)
    {
        sitePop = pop;
        popNameTag.text = sitePop.GetPopName();
        popWealthTag.text = sitePop.GetMoney().ToString();
        SetGoodDemandIcons();
    }

    private void SetGoodDemandIcons()
    {
        if(sitePop.HasGood(GoodsManager.instance.GetGoodSOByName("Food")))
        {
            foodGoodIcon.color = Color.white;
        }
        else
        {
            foodGoodIcon.color = Color.black;
        }

        if(sitePop.HasGood(GoodsManager.instance.GetGoodSOByName("Tools")))
        {
            toolsGoodIcon.color = Color.white;
        }
        else
        {
            toolsGoodIcon.color = Color.black;
        }

        if(sitePop.HasGood(GoodsManager.instance.GetGoodSOByName("Clothes")))
        {
            clothesGoodIcon.color = Color.white;
        }
        else
        {
            clothesGoodIcon.color = Color.black;
        }
    }

    public void OpenPopLog()
    {
        PopLogUI.Instance.OpenPopLog();
        PopLogUI.Instance.SetPopLog(sitePop);
    }
}
