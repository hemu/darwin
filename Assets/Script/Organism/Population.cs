using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Represents a population of one type of organism 
 * 
 */

//public class Population<TOrganism> where TOrganism: Organism, new() {
public class Population {
    // number of organisms per second
    private float growthRate = 0f;
    private List<Organism> organisms = new List<Organism>();
    private List<Organism> deadOrganisms = new List<Organism>();
    private float reproducePool = 0f;
    private float energyProducePerOrganism = 0f;
    private OrganismFactory organismFactory;
    private PopulationController populationController;

    public Population(PopulationController populationController,
                      OrganismFactory organismFactory,
                      int initCount) {
        this.populationController = populationController;
        this.organismFactory = organismFactory;
        AddOrganisms(initCount);
        InitParams();
        // subscribe self to organism death messages
        SubscribeDeath(HandleDeath);
    }

    private void InitParams() {
        energyProducePerOrganism = organisms[0].GetEnergyProduceVal();
    }

    public void AddOrganisms(int num) {
        Organism org;
        for(int i=0; i<num; i++) {
            org = organismFactory.Create(this);
            organisms.Add(org);
        }
    }

    public void Update() {
        float foodNeeded = 0f;
        foreach(Organism org in organisms) {
            org.Update();
            foodNeeded += org.GetFoodNeeded();
        }
        if(foodNeeded > 0) {
            Feed(foodNeeded);
        }
        Cleanup();
    }

    public void Feed(float foodNeeded) {
        float food = populationController.RegisterFoodRequest(foodNeeded);
        foreach(Organism org in organisms) {
            org.Feed(food);
        }
    }

    public void Reproduce(float timeDelta) {
        reproducePool += growthRate * timeDelta;
        if(reproducePool >= 1) {
            float numNewOrg = Mathf.Floor(reproducePool);
            reproducePool -= numNewOrg;
            AddOrganisms((int) numNewOrg);
        }
    }

    public float GetEnergyProduceVal() {
        return organisms.Count * energyProducePerOrganism;
    }

    public float TakeFood(float foodAmt) {
        float numOrganisms = organisms.Count;
        if(foodAmt >= numOrganisms) {
            Debug.Log("food request requires entire population: " + numOrganisms);
            int needToKill = (int)foodAmt;
            for(int i=0; i < organisms.Count; i++){
                organisms[i].Kill();
            }
            return numOrganisms;
        } else {
//            organisms.RemoveRange(0, (int)foodAmt);
            int needToKill = (int)foodAmt;
            for(int i=0; i < needToKill; i++){
                organisms[i].Kill();
            }
            return foodAmt;
        }
    }

    public void HandleDeath(Organism organism) {
        deadOrganisms.Add(organism);
    }

    public void RegisterDeath(Organism organism) {
        BroadcastDeath(organism);
    }

    public delegate void DeathHandler(Organism organism);
    private event DeathHandler DeathNotify;

    public void SubscribeDeath(DeathHandler callback) {
        DeathNotify += callback;
    }
    
    public void BroadcastDeath(Organism organism) {
        DeathNotify(organism);
    }

    public void Cleanup() {
        if(deadOrganisms.Count > 0) {
            foreach(Organism org in deadOrganisms){
                organisms.Remove(org);
            }
            deadOrganisms.Clear();
        }
    }

    // remove num from population
    // return how many were able to be killed
//    public int Kill(int num) {
//        int count = organisms.Count;
//        if(count < num) {
//            int prevCount = count;
//            count = 0;
//            return prevCount;
//        } else {
//            count -= num;
//            return num;
//        }
//    }

}
