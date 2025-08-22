using UnityEngine;

public class ConcreteSite : MonoBehaviour
{
    [SerializeField]
    string siteName;

    [SerializeField]
    SiteSO siteData;

    Pop sitePop;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            sitePop = child.gameObject.GetComponent<Pop>();
        }
    }

    public string GetName()
    {
        return siteName;
    }

    public Pop GetPop()
    {
        return sitePop;
    }    

    public void AssignPop(Pop pop)
    {
        sitePop = pop;
    }

    //Produce a Good
    //Will be more complicated later, for now just use as placeholder
    public Good ProduceGood()
    {
        Good good = new Good(siteData.GetProducedGood());
        return good;
    }
}
