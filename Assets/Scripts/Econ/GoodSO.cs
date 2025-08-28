using UnityEngine;

[CreateAssetMenu(fileName = "GoodSO", menuName = "Scriptable Objects/GoodSO")]
public class GoodSO : ScriptableObject
{
    [SerializeField]
    string goodName;

    [SerializeField]
    int goodID;

    [SerializeField]
    bool isProductionGood;

    [SerializeField]
    bool isEssentialGood;

    [SerializeField]
    int consumptionUses;

    public string GetName() => goodName;
    public int GetID() => goodID;
    public bool IsProductionGood() => isProductionGood;
    public int GetConsumptionUses() => consumptionUses;
    public bool IsEssentialGood() => isEssentialGood;
    public string GetGoodName() => goodName;
}
