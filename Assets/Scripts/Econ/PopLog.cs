using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PopLog : MonoBehaviour
{
    List<string> logEntries = new List<string>();


    public void AddLogEntry(string entry)
    {
        logEntries.Add(entry);
        if (logEntries.Count > 20) // Keep only the last 20 entries
        {
            logEntries.RemoveAt(0);
        }
    }

    public List<string> GetLogEntries()
    {
        return new List<string>(logEntries);
    }
}
