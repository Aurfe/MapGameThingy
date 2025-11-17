using UnityEngine;
using TMPro;

public class SiteUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI siteNameTag;

    public void SetSiteUI(ConcreteSite site)
    {
        siteNameTag.text = site.GetName();
    }
}
