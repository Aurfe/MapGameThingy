using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : SerializedMonoBehaviour
{
    public static GoodsManager instance;

    [SerializeField]
    Dictionary<string, GoodSO> goodsDictionary = new Dictionary<string, GoodSO>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public List<GoodSO> GetGoodSOs()
    {
        List<GoodSO> goodSOs = new List<GoodSO>(goodsDictionary.Values);
        return goodSOs;
    }

    public GoodSO GetGoodSOByName(string name)
    {
        if (goodsDictionary.TryGetValue(name, out GoodSO goodSO))
        {
            return goodSO;
        }
        else
        {
            Debug.LogWarning($"GoodSO with name {name} not found.");
            return null;
        }
    }
}
