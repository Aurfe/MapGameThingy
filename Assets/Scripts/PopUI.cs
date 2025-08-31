using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popNameTag;
    [SerializeField] TextMeshProUGUI popWealthTag;

    [SerializeField] TextMeshProUGUI foodGoodIcon;
    [SerializeField] TextMeshProUGUI toolsGoodIcon;

    [SerializeField] GameObject popLogPanel;
    [SerializeField] Transform popLogContent;
    [SerializeField] Transform popLogEntryPrefab;

    public void SetUI(Pop pop)
    {
        popNameTag.text = pop.GetPopName();
        popWealthTag.text = pop.GetMoney().ToString();
        SetGoodDemandIcons(pop);
        SetPopLogContent(pop);
        popLogPanel.SetActive(false);
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

    private void SetPopLogContent(Pop pop)
    {
        List<string> logEntries = pop.GetPopLog();

        // Clear existing log entries
        foreach (Transform child in popLogContent)
        {
            Destroy(child.gameObject);
        }
        // Add new log entries
        for (int i = 0; i < logEntries.Count; i++)
        {
            Transform entry = Instantiate(popLogEntryPrefab, popLogContent);
            TextMeshProUGUI entryText = entry.GetComponent<TextMeshProUGUI>();
            string logNumber = (i + 1).ToString();
            entryText.text = logNumber + " " + logEntries[i];
        }
    }

    public void OpenPopLog()
    {
        popLogPanel.SetActive(!popLogPanel.activeSelf);
    }
}
