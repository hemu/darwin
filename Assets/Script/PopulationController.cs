using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopulationController : MonoBehaviour {

    public int foodChainRank = 0;
    public GameController gameController;
    public OrganismFactory organismFactory;
    private Population population;
    private EcosystemController ecosystemController;

    public string popName = "unnamed";

    // energy
    public double energyProduceValue;
    private double energy = 0;
   
    // energy production
    private bool inProduction = false;
    public float productionTimeInSec;
    private float prodTime = 0;
    private Image progressBar;
    private float progressPercent = 0f;

    // level
    private Text levelText;
    private int level = 1;

    // upgrade
    private Text upgradeCostText;
    public int upgradeCost = 0;

    // upgrade controls
    public int bonusLevelInterval = 5;
    private int previousBonusLevel = 0;
    public double upgradeEnergyExponent = 1.3;
    public float upgradeCostExponent = 2.0f;

    // unlocking
    public bool unlocked = false;
    public int unlockEnergyThreshold = 2;

    const int AUTOMATIC_LEVEL_THRESHOLD = 2;

    void Awake() {
        population = new Population(this, organismFactory, 1);
        population.SubscribeDeath(RegisterDeath);
    }

    void Start () {
        progressBar = transform.FindChild("progressBar").FindChild("progressBarFG").GetComponent<Image>();
        progressBar.fillAmount = 0f;
        levelText = transform.FindChild("organismLevel").GetComponent<Text>();
        upgradeCostText = transform.FindChild("upgradeBtn").FindChild("upgradeBtnElem").GetComponent<Text>();
        UpdateCost();
        //        Unlock();
        if(!unlocked) {
            Lock();
        }
	}

	void Update () {
        population.Update();
        if(inProduction) {
            UpdateEnergy();
            UpdateProgress();
        }
        UpdateProgressBar();
	}

    public float RegisterFoodRequest(float foodAmount) {
        float food = ecosystemController.RegisterFoodRequest(this, foodAmount);
        Debug.Log(popName + " received " + food + " food");
        return food;
    }

    private void UpdateProgressBar() {
        progressBar.fillAmount = progressPercent;
    }

    private void UpdatePopulation(int newLevel) {
        int levelDiff = newLevel - level;
        if(levelDiff > 0) {
            population.AddOrganisms(levelDiff);
        }
        level = newLevel;
        levelText.text = level.ToString();
    }

    private void UpdateProgress() {
        progressPercent = Mathf.Min(prodTime / productionTimeInSec, 1.0f);
    }

    public void RegisterUpgradeSignal() {
        gameController.RegisterUpgradeSignal(this);
    }

    public void RegisterStartSignal() {
        if(unlocked) {
            if(!inProduction) {
                StartProduction();
            }
        }
    }

    void UpdateEnergy() {
        prodTime += Time.deltaTime;
        if(prodTime > productionTimeInSec) {
            SetEnergy(energy + population.GetEnergyProduceVal());
            ResetProduction();
        }
    }

    private void StartProduction() {
        inProduction = true;
    }

    private void ResetProduction() {
        prodTime = 0;
        progressPercent = 0f;
        if(level <= AUTOMATIC_LEVEL_THRESHOLD) {
            inProduction = false;
        }
    }

    public void Upgrade() {
        UpdateCost();
        UpdateEnergyProduction();
        if(level % bonusLevelInterval == 0) {
            Debug.Log("New organism bonuses unlocked.");
            productionTimeInSec *= 0.5f;
            bonusLevelInterval = (int)(bonusLevelInterval * 1.5) + previousBonusLevel;
            previousBonusLevel = level;
        }
        UpdatePopulation(level + 1);
        if(level > AUTOMATIC_LEVEL_THRESHOLD) { 
            StartProduction();
        }
    }

    private void UpdateCost() {
//        upgradeCost = (int)(Mathf.Pow(upgradeCost, upgradeCostExponent) + 1);
//        upgradeCostText.text = upgradeCost.ToString();
    }

    private void UpdateEnergyProduction() {
//        energyProduceValue = System.Math.Pow(energyProduceValue, upgradeEnergyExponent) + 1.0
    }

    private void RegisterDeath(Organism organism) {
        UpdatePopulation(level - 1);
    }

    public float TakeFood(float foodAmt) {
        float availableFood = population.TakeFood(foodAmt);
        Debug.Log(popName + " giving up " + availableFood + " food");
        return availableFood;
    }

    private void SetEnergy(double newEnergy) {
        energy = newEnergy;
    }

    public double TakeEnergy() {
        double energyFinal = System.Math.Round(energy, 2);
        SetEnergy(0.0);
        return energyFinal;
    }

    public int GetUpgradeCost() {
        return upgradeCost;
    }

    public string GetName() {
        return popName;
    }

    public void Lock() {
        transform.gameObject.SetActive(false);
        unlocked = false;
    }
    
    public void Unlock() {
        if(!unlocked) {
            transform.gameObject.SetActive(true);
            unlocked = true;
        }
    }

    public void SetEcosystem(EcosystemController ec) {
        ecosystemController = ec;
    }

}
