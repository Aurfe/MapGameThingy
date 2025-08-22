using UnityEngine;

[CreateAssetMenu(fileName = "SiteSO", menuName = "Scriptable Objects/SiteSO")]
public class SiteSO : ScriptableObject
{
    [SerializeField]
    string producedGood;

    public string GetProducedGood()
    {
        return producedGood;
    }
}