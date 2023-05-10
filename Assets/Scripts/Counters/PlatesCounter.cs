using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using Kitchen;
using Kitchen.SO;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSo;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            
            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;
                
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                
            }
            
        }
    }
    
    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player doesn't have kitchen object
            if (platesSpawnedAmount > 0) {
                // There are plates on the counter
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);
                
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
