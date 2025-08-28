using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public event System.EventHandler OnMarketUpdate;

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
    public void TriggerMarketUpdate()
    {
        OnMarketUpdate?.Invoke(this, System.EventArgs.Empty);
    }
}
