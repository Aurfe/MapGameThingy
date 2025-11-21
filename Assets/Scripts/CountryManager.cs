using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : SerializedMonoBehaviour
{
    public static CountryManager Instance;

    public List<Country> countries = new List<Country>();

    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Debug.Log(GetPlayerCountry().countryName);
    }

    [SerializeField] Country playerCountry;

    public Country GetPlayerCountry()
    {
        return playerCountry;
    }
}
