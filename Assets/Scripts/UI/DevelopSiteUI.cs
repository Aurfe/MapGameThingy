using UnityEngine;

public class DevelopSiteUI : MonoBehaviour
{
    [SerializeField]
    GameObject possibleDevelopmentPrefab;
    [SerializeField]
    Transform content;

    public void SetPanel(WildSiteSO wildSite)
    {
        foreach (SiteSO site in wildSite.GetPossibleSites())
        {
            Instantiate(possibleDevelopmentPrefab, content).GetComponent<PossibleSiteDevelopmentUI>().SetPanel(site);
        }
    }

    private void Update()
    {
        // If player clicks anywhere outside the UI, close it
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                this.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
