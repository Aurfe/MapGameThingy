using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GivePopMoneyUI : MonoBehaviour
{
    [SerializeField] Slider moneySlider;
    [SerializeField] TextMeshProUGUI moneyAmountText;
    [SerializeField] TextMeshProUGUI maxMoneyText;

    int maxMoney = 2;

    Pop pop;

    int currentMoneyToGive = 0;

    private void Update()
    {
        currentMoneyToGive = (int)moneySlider.value;
        moneyAmountText.text = currentMoneyToGive.ToString();

        // If player clicks anywhere outside the UI, close it
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                this.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OpenUI(Pop popToGiveMoney)
    {
        pop = popToGiveMoney;
        maxMoney = CountryManager.Instance.GetPlayerCountry().GetTreasury();
        maxMoneyText.text = maxMoney.ToString();
        moneySlider.maxValue = maxMoney;
    }

    public void GivePopMoney()
    {
        if (currentMoneyToGive <= 0)
            return;
        Country playerCountry = CountryManager.Instance.GetPlayerCountry();
        if (playerCountry.GetTreasury() >= currentMoneyToGive)
        {
            playerCountry.AdjustTreasury(-currentMoneyToGive);
            pop.ChangeMoney(currentMoneyToGive);
        }

        ProvinceUI.Instance.GenerateLists();

        Destroy(this.gameObject);
    }
}
