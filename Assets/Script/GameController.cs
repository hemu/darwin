using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
   
    public GameObject[] organisms;
    public int[] organismUnlockThreshold;
    public UIController uiController;
    private double energy = 0;

    void Awake() {
        organisms = GameObject.FindGameObjectsWithTag("organism");
        organismUnlockThreshold = new int[organisms.Length];
        GameObject organism;
        OrganismController oc;
        for(int i =0; i < organisms.Length; i++) {
            organism = organisms[i];
            oc = organism.GetComponent<OrganismController>();
            organismUnlockThreshold[i] = oc.unlockEnergyThreshold;
        }
    }

  	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update() {
        double additionalEnergy = 0;
        foreach(GameObject organism in organisms) {
            additionalEnergy += organism.GetComponent<OrganismController>().TakeEnergy();
        }
        if(additionalEnergy != 0) {
            UpdateEnergy(energy + additionalEnergy);
        }
	}

    private void UpdateEnergy(double newEnergy) {
        energy = newEnergy;
        uiController.UpdateEnergy(energy);
        CheckLockedOrganisms();
    }

    public void CheckLockedOrganisms() {
        GameObject organism;
        for(int i=0; i< organisms.Length; i++) {
            organism = organisms[i];
            if(energy > organismUnlockThreshold[i]){
                organism.GetComponent<OrganismController>().Unlock();
            }
        }
    }

    public void RegisterUpgradeSignal(OrganismController organism) {
        int upgradeCost = organism.GetUpgradeCost();
        if(organism.GetUpgradeCost() <= energy) {
            organism.Upgrade();
            UpdateEnergy(energy - upgradeCost);
        } else {
            Debug.Log("Not enough energy to upgrade.");
        }
    }
}