using UnityEngine;

[CreateAssetMenu(fileName = "SiteSO", menuName = "Scriptable Objects/SiteSO")]
public class SiteSO : ScriptableObject
{
    [SerializeField]
    ProductionMethod productionMethod;

    public ProductionMethod GetProductionMethod() => productionMethod;
}