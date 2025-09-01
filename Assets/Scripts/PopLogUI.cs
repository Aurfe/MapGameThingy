using UnityEngine;

public class PopLogUI : MonoBehaviour
{
    public static PopLogUI Instance;

    [SerializeField] GameObject popLogUIObject;
    [SerializeField] Transform content;
    [SerializeField] Transform popLogEntryPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OpenPopLog()
    {
        popLogUIObject.SetActive(true);
    }

    public void ClosePopLog()
    {
        popLogUIObject.SetActive(false);
    }

    public void SetPopLog(Pop pop)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < pop.GetPopLog().Count; i++)
        {
            Transform entry = Instantiate(popLogEntryPrefab, content);
            string logEntry = "[" + (i + 1).ToString() + "] : " + pop.GetPopLog()[i];
            entry.GetComponent<TMPro.TextMeshProUGUI>().text = logEntry;
        }
    }
}
