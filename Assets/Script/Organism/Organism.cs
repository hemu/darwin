using UnityEngine;
using System.Collections;

public class Organism {

    // value in range [0, 1]
    // if health reaches 0, organism dies
    private float health = 1f;
    // amount organism wants to eat
    private float hungryFoodAmount;
    private double prevFullTime;

    private bool isHungry = false;
    private bool isAlive = true;
    // population this organism belongs to
    private Population population;

    // --- Organism defining properties ---
    // amount organism needs to eat to be full
    private float fullFoodAmt = 1f;
    // how long before organism has to feed again
    private float feedIntervalSec = 10f;
    // window of time animal can be hungry before classified as starving
    // when starving, animal will lose health
    private float hungryWindow = 10f;
    // how fast health decreases if starving
    private float starveRate = 0.2f;
    // how much energy it produces
    private float energyProduceVal = 1.0f;
   
    private string orgName = "unnamed";

    public Organism(Population population,
                    string orgName,
                    float fullFoodAmt,
                    float feedIntSec,
                    float hungryWindow,
                    float energyProduceVal) {
        this.population = population;
        this.orgName = orgName;
        this.fullFoodAmt = fullFoodAmt;
        this.feedIntervalSec = feedIntSec;
        this.hungryWindow = hungryWindow;
        this.energyProduceVal = energyProduceVal;

        prevFullTime = Time.timeSinceLevelLoad;
    }

    public bool IsHungry() {
        return isHungry;
    }

    public string GetName() {
        return orgName;
    }

    public float GetFoodNeeded() {
        if(IsHungry()) {
            return hungryFoodAmount;
        } else {
            return 0f;
        }
    }

    // eat needed amount of food and return leftover food
    public float Feed(float foodAmt) {
        if(IsHungry()) {
            if(foodAmt >= hungryFoodAmount) {
                // can eat full amount and not be hungry
                float newFoodAmt = foodAmt - hungryFoodAmount;
                hungryFoodAmount = 0;
                isHungry = false;
                prevFullTime = Time.timeSinceLevelLoad;
                return newFoodAmt;
            } else {
                // eats and reduces hunger, but still hungry
                hungryFoodAmount -= foodAmt;
                return 0;
            }
        } else {
            return foodAmt;
        }
    }

    public void Update() {
        if(health <= 0) {
            Kill();
        } else {
            float time = Time.timeSinceLevelLoad;
            if(!isHungry && time > (prevFullTime + feedIntervalSec)) {
                Debug.Log(orgName + " became hungry");
                isHungry = true;
                hungryFoodAmount = fullFoodAmt;
            } else if(isHungry && time > prevFullTime + feedIntervalSec + hungryWindow) {
                Debug.Log(orgName + " is starving");
                health -= starveRate * Time.deltaTime;
            }
        }
    }

    public void Kill() {
        Debug.Log(orgName + " died");
        isAlive = false;
        population.RegisterDeath(this);
    }

    public float GetFeedAmount() {
        return fullFoodAmt;
    }

    public float GetEnergyProduceVal() {
        return energyProduceVal;
    }

}
