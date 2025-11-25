using UnityEngine;

[CreateAssetMenu(fileName = "SiteSO", menuName = "Scriptable Objects/SiteSO")]
public class SiteSO : ScriptableObject
{
    [SerializeField]
    string siteName;

    [SerializeField]
    ProductionMethod productionMethod;

    public ProductionMethod GetProductionMethod() => productionMethod;

    [SerializeField]
    SiteType siteType;

    public SiteType GetSiteType() => siteType;
    public string GetSiteName() => siteName;
}

public enum SiteType
{
    Wild,
    Rural,
    Urban
}