using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Production Method", menuName = "Scriptable Objects/Production Method")]
public class ProductionMethod : SerializedScriptableObject
{
    [SerializeField]
    Dictionary<GoodSO, int> inputGoods;

    [SerializeField]
    Dictionary<GoodSO, int> outputGoods;

    public Dictionary<GoodSO, int> GetOutputGoods()
    {
        return outputGoods;
    }

    public Dictionary<GoodSO, int> GetInputGoods()
    {
        return inputGoods;
    }
}
