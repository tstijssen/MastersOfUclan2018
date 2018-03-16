using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarSelect : MonoBehaviour {

    public GameObject[] VehicleVisualization;

    public int currentVisual;

	// Use this for initialization
	void Start () {
        currentVisual = 0;
        VehicleVisualization[currentVisual].SetActive(true);
    }

    public void SwitchVisual()
    {
        VehicleVisualization[currentVisual].SetActive(false);
        currentVisual = (currentVisual + 1) % VehicleVisualization.Length;
        VehicleVisualization[currentVisual].SetActive(true);
    }

    public void SetVisual(int selection)
    {
        VehicleVisualization[currentVisual].SetActive(false);
        currentVisual = selection;
        VehicleVisualization[currentVisual].SetActive(true);
    }
}
