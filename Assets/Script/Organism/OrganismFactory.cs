using UnityEngine;
using System.Collections;

public class OrganismFactory : MonoBehaviour {

    // override this
    public virtual Organism Create(Population population) {
        return new Organism(population,
                            "unnamed",
                            0f,
                            0f,
                            0f,
                            0f);
    }
}
