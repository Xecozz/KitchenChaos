using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    
    private List<GameObject> plateVisualGameObjectsList;

    private void Awake() {
        plateVisualGameObjectsList = new List<GameObject>();
    }

    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounterOnOnPlateRemoved;
    }
    
    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        
        float playOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0,playOffsetY * plateVisualGameObjectsList.Count,0);
        
        plateVisualGameObjectsList.Add(plateVisualTransform.gameObject);
    }
    
    private void PlatesCounterOnOnPlateRemoved(object sender, EventArgs e) {
        GameObject plateGameObject = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];
        plateVisualGameObjectsList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
    
}
