using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
   
    public GameObject[] populations;
    public int[] popUnlockThreshold;
    public UIController uiController;
    private double energy = 0;

    void Awake() {
        populations = GameObject.FindGameObjectsWithTag("population");
        popUnlockThreshold = new int[populations.Length];
        GameObject population;
        PopulationController pc;
        // store population unlock thresholds
        for(int i =0; i < populations.Length; i++) {
            population = populations[i];
            pc = population.GetComponent<PopulationController>();
            popUnlockThreshold[i] = pc.unlockEnergyThreshold;
        }
    }

	void Start() {

	}
	
	void Update() {
        UpdateEnergy();
	}

    private void UpdateEnergy() {
        double additionalEnergy = 0;
        foreach(GameObject population in populations) {
            additionalEnergy += population.GetComponent<PopulationController>().TakeEnergy();
        }
        if(additionalEnergy != 0) {
            SetEnergy(energy + additionalEnergy);
        }
    }
    
    private void SetEnergy(double newEnergy) {
        energy = newEnergy;
        uiController.SetEnergy(energy);
        UpdateLockedPopulations();
    }

    public void UpdateLockedPopulations() {
        GameObject population;
        for(int i=0; i< populations.Length; i++) {
            population = populations[i];
            if(energy > popUnlockThreshold[i]) {
                population.GetComponent<PopulationController>().Unlock();
            }
        }
    }

    public void RegisterUpgradeSignal(PopulationController population) {
        int upgradeCost = population.GetUpgradeCost();
        if(population.GetUpgradeCost() <= energy) {
            population.Upgrade();
            SetEnergy(energy - upgradeCost);
        } else {
            Debug.Log("Not enough energy to upgrade.");
        }
    }
}