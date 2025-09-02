using UnityEngine;

[CreateAssetMenu(fileName = "GoodSO", menuName = "Scriptable Objects/GoodSO")]
public class GoodSO : ScriptableObject
{
    [SerializeField]
    string goodName;

    [SerializeField]
    bool isPerishable;

    [SerializeField]
    bool isProductionGood;

    [SerializeField]
    bool isEssentialGood;

    [SerializeField]
    int consumptionUses;

    public string GetName() => goodName;
    public bool IsProductionGood() => isProductionGood;
    public bool IsPerishable() => isPerishable;
    public int GetConsumptionUses() => consumptionUses;
    public bool IsEssentialGood() => isEssentialGood;
    public string GetGoodName() => goodName;
}
