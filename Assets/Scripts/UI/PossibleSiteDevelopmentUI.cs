using TMPro;
using UnityEngine;

public class PossibleSiteDevelopmentUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI siteNameText;

    public void SetPanel(SiteSO site)
    {
        siteNameText.text = site.GetSiteName();
    }
}
