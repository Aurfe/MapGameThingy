using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public event System.EventHandler OnMarketUpdate;

    public static MarketManager instance { get; private set; }

    int marketTick = 0;

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
    public void TriggerMarketUpdate()
    {
        OnMarketUpdate?.Invoke(this, System.EventArgs.Empty);
        ProvinceUI.Instance.GenerateLists();
        marketTick++;
    }
    public int GetMarketTickNumber() => marketTick;
}
