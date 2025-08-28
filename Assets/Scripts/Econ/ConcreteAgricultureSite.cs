using System.Collections.Generic;
using UnityEngine;

public class ConcreteAgricultureSite : ConcreteSite
{
    protected override int CalculateProductionBonus()
    {
        int totalProductionBonus = base.CalculateProductionBonus();

        totalProductionBonus += Random.Range(-1, 3); // Using magic numbers for now, will be changed to a variable later

        return totalProductionBonus;
    }
}
