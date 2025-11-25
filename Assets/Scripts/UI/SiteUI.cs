using UnityEngine;
using TMPro;

public class SiteUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI siteNameTag;
    [SerializeField]
    GameObject developmentPanelPrefab;
    [SerializeField]
    GameObject siteDevButton;

    public void SetSiteUI(ConcreteSite site)
    {
        siteNameTag.text = site.GetName();

        if (site.IsSiteType(SiteType.Wild))
        {
            siteDevButton.SetActive(true);
        }
    }

    public void OpenDevelopmentPanel()
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        GameObject obj = Instantiate(developmentPanelPrefab, new Vector3(transform.position.x, transform.position.y), transform.rotation);
        obj.transform.SetParent(canvas.transform, false);
    }
}
