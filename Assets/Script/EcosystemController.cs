using UnityEngine;
using System.Collections;
using System.Linq;

// PopulationControllers can consult Ecosystem
// and ask for food
public class EcosystemController : MonoBehaviour {

    public PopulationController[] populationControllers;

    void Awake() {
        populationControllers = populationControllers.OrderBy(x => x.foodChainRank).ToArray();
        foreach(PopulationController pc in populationControllers) {
            pc.SetEcosystem(this);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public float RegisterFoodRequest(PopulationController popController, float foodAmt) {
        int foodChainRank = popController.foodChainRank;
        // if lowest on food chain, give free food
        if(foodChainRank == 0) {
            return foodAmt;
        } else {
            return populationControllers[foodChainRank-1].TakeFood(foodAmt);
        }
    }
}
