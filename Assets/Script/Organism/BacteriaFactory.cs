using UnityEngine;
using System.Collections;

public class BacteriaFactory : OrganismFactory {

    //    string name,
    //    float fullFoodAmt,
    //    float feedIntSec,
    //    float hungryWindow,
    //    float energyProduceVal
    public override Organism Create(Population population) {
        return new Organism(population,
                            "bacteria",
                            1f,
                            20f,
                            20f,
                            1f);
    }
}
