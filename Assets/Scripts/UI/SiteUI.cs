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

    ConcreteSite currentSite;
    public void SetSiteUI(ConcreteSite site)
    {
        siteNameTag.text = site.GetName();

        if (site.IsSiteType(SiteType.Wild))
        {
            siteDevButton.SetActive(true);
            currentSite = site;
        }
    }

    public void OpenDevelopmentPanel()
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        GameObject obj = Instantiate(developmentPanelPrefab, new Vector3(transform.position.x, transform.position.y), transform.rotation);
        obj.transform.SetParent(canvas.transform, false);

        obj.GetComponent<DevelopSiteUI>().SetPanel((WildSiteSO)currentSite.GetSiteData());
    }
}
