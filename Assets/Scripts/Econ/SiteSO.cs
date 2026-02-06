using UnityEngine;

[CreateAssetMenu(fileName = "SiteSO", menuName = "Scriptable Objects/SiteSO")]
public class SiteSO : ScriptableObject
{
    [SerializeField]
    ProductionMethod productionMethod;

    public ProductionMethod GetProductionMethod() => productionMethod;

    [SerializeField]
    SiteType siteType;

    public SiteType GetSiteType() => siteType;
}

public enum SiteType
{
    Wild,
    Rural,
    Urban
}