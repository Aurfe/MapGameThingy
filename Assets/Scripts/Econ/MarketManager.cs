using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public event System.EventHandler OnMarketConsumptionTick;
    public event System.EventHandler OnMarketProductionTick;

    public static MarketManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerMarketProductionTick()
    {
        OnMarketProductionTick?.Invoke(this, System.EventArgs.Empty);
    }
    public void TriggerMarketConsumptionTick()
    {
        OnMarketConsumptionTick?.Invoke(this, System.EventArgs.Empty);
    }
}
