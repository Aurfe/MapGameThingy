using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketLog : MonoBehaviour
{
    [SerializeField]
    GameObject marketLogObject;
    [SerializeField]
    Transform logContent;
    [SerializeField]
    Transform logEntryPrefab;

    List<string> logEntries = new List<string>();

    public void AddLogEntry(string entry)
    {
        logEntries.Add(entry);
    }

    public void OpenLog()
    {
        marketLogObject.SetActive(true);
        RefreshLogUI();
    }

    public void CloseLog()
    {
        marketLogObject.SetActive(false);
    }

    public void RefreshLogUI()
    {
        // Clear existing entries
        foreach (Transform child in logContent)
        {
            Destroy(child.gameObject);
        }
        // Add new entries
        foreach (string entry in logEntries)
        {
            Transform newEntry = Instantiate(logEntryPrefab, logContent);
            string entryText = "[" + MarketManager.instance.GetMarketTickNumber() + "] " + entry;

            newEntry.GetComponent<TextMeshProUGUI>().text = entryText;
        }
    }

    public void ClearLog()
    {
        logEntries.Clear();
        RefreshLogUI();
    }
}
