using UnityEngine;

public class Good
{
    [SerializeField]
    string goodName;

    public string GetName()
    {
        return goodName;
    }

    public Good(string name)
    {
        goodName = name;
    }
}
