using UnityEngine;
using System.Collections;

public class FishFactory : OrganismFactory {

    //    string name,
    //    float fullFoodAmt,
    //    float feedIntSec,
    //    float hungryWindow,
    //    float energyProduceVal
    public override Organism Create(Population population) {
        return new Organism(population,
                            "fish",
                            1f,
                            3f,
                            3f,
                            5f);
    }
}
