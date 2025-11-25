using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WildSiteSO", menuName = "Scriptable Objects/WildSite")]
public class WildSiteSO : SiteSO
{
    [SerializeField] List<SiteSO> possibleSites;

    public List<SiteSO> GetPossibleSites() => possibleSites;
}
