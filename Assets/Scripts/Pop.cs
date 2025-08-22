using UnityEngine;

public class Pop : MonoBehaviour
{
    [SerializeField]
    string popName;
    int money;

    public string GetPopName()
    {
        return popName;
    }
}
