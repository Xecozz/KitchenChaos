
using System;
using System.Collections.Generic;
using Kitchen.SO;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int sucessfulRecipesAmount;
    
    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;
            
            if (waitingRecipeSOList.Count < waitingRecipeMax) {
                RecipeSO waitingRecipeSo = recipeListSO.recipeSoList[Random.Range(0, recipeListSO.recipeSoList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSo);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSo = waitingRecipeSOList[i];

            if (waitingRecipeSo.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSoList().Count) {
                // has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectsSOList) {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList()) {
                        if (plateKitchenObjectSo == recipeKitchenObjectSo) {
                            // has the same ingredient
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // missing ingredient
                        plateContentsMatchesRecipe = false;
                    }
                }
                
                if (plateContentsMatchesRecipe) {
                    // Player delivered the correct recipe
                    sucessfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return; 
                }
            }
        }
        // no mathing recipe found
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }
    
    public List<RecipeSO> GetWaitingRecipeSoList() {
        return waitingRecipeSOList;
    }
    
    public int GetSucessfulRecipesAmount() {
        return sucessfulRecipesAmount;
    }
}
