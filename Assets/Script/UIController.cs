using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject energyCounter;
    private Text energyCounterText;

	// Use this for initialization
	void Start() {
        energyCounterText = energyCounter.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update() {

	}

    public void UpdateEnergy(double energy) {
        energyCounterText.text = System.Math.Round(energy, 2).ToString();
    }

}
