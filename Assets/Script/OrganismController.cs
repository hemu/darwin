using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrganismController : MonoBehaviour {

    public GameController gameController;

    public double energyProduceValue;
    public float productionTimeInSec;
    public string organismName = "organism";

    // energy
    private double energy = 0;
    private bool inProduction = false;

    // progress bar
    private float prodTime = 0;
    private Image progressBar;
    private float progressPercent = 0f;

    // levels
    private Text levelText;
    private int level = 1;

    // upgrade
    private Text upgradeCostText;
    public int upgradeCost = 0;

    // upgrade multiplier
    public int bonusLevelInterval = 5;
    private int previousBonusLevel = 0;
    public double upgradeEnergyExponent = 1.3;
    public float upgradeCostExponent = 2.0f;

    public bool unlocked = false;
    public int unlockEnergyThreshold = 2;

    const int AUTOMATIC_LEVEL_THRESHOLD = 2;

    void Awake() {
    }

    void Start () {
        progressBar = transform.FindChild("progressBar").FindChild("progressBarFG").GetComponent<Image>();
        progressBar.fillAmount = 0.5f;
        levelText = transform.FindChild("organismLevel").GetComponent<Text>();
        upgradeCostText = transform.FindChild("upgradeBtn").FindChild("upgradeBtnElem").GetComponent<Text>();
        UpdateCost();
        //        Unlock();
        if(!unlocked) {
            Lock();
        }
	}

	void Update () {
        if(inProduction) {
            UpdateEnergy();
            UpdateProgress();
        }
        UpdateProgressBar();
	}

    public void Lock() {
        transform.gameObject.SetActive(false);
        unlocked = false;
    }

    public void Unlock() {
        Debug.Log("unlocking....");
        Debug.Log(unlocked);
        if(!unlocked) {
            transform.gameObject.SetActive(true);
            unlocked = true;
            Debug.Log("done unlocking");
        }

    }

    private void UpdateProgressBar() {
        progressBar.fillAmount = progressPercent;
    }

    private void UpdateLevel(int newLevel) {
        level = newLevel;
        levelText.text = newLevel.ToString();
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
            SetEnergy(energy + energyProduceValue);
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
        level += 1;
        UpdateCost();
        UpdateEnergyProduction();
        if(level % bonusLevelInterval == 0) {
            Debug.Log("New organism bonuses unlocked.");
            productionTimeInSec *= 0.5f;
            bonusLevelInterval = (int)(bonusLevelInterval * 1.5) + previousBonusLevel;
            previousBonusLevel = level;
        }
        UpdateLevel(level);
        if(level > AUTOMATIC_LEVEL_THRESHOLD) { 
            StartProduction();
        }
    }

    private void UpdateCost() {
        upgradeCost = (int)(Mathf.Pow(upgradeCost, upgradeCostExponent) + 1);
        upgradeCostText.text = upgradeCost.ToString();
    }

    private void UpdateEnergyProduction() {
        energyProduceValue = System.Math.Pow(energyProduceValue, upgradeEnergyExponent) + 1.0;
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

}
