using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI treasuryAmountTag;

    private void Start()
    {
        CountryManager.Instance.GetPlayerCountry().OnTreasuryChanged += Country_OnTreasuryChanged;
        SetTreasuryTag(CountryManager.Instance.GetPlayerCountry().GetTreasury());
    }

    public void SetTreasuryTag(int amount)
    {
        treasuryAmountTag.text = amount.ToString();
    }

    private void Country_OnTreasuryChanged(object sender, EventArgs e)
    {
        SetTreasuryTag(CountryManager.Instance.GetPlayerCountry().GetTreasury());
    }
}
