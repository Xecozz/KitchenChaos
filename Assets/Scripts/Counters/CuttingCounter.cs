using System;
using Kitchen;
using Kitchen.SO;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler OnAnyCut;
        
        new public static void ResetStaticData() {
            OnAnyCut = null;
        }
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler OnCut; 
        
        [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

        private int cuttingProgress;

        public override void Interact(Player player) {
            if (!HasKitchenObject()) {
                // no object on counter
                if (player.HasKitchenObject()) {
                    // player has object
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) {
                        //Player has object that can be cut
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        cuttingProgress = 0;
                        CuttingRecipeSO cuttingRecipeSo = getCuttingRecipeSOWithinput(GetKitchenObject().GetKitchenObjectSo());
                    
                        OnProgressChanged?.Invoke(this,
                            new IHasProgress.OnProgressChangedEventArgs
                                { progressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax });
                    }
                }
                else {
                    //player has no object
                }
            }
            else {
                // object on counter
                if (player.HasKitchenObject()) {
                    // player has object
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                        //player has plate
                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo())) {
                            GetKitchenObject().DestroySelf();
                        }
                        
                    }
                }
                else {
                    // player has no object
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

        public override void InteractAlternate(Player player) {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo())) {
                // object on counter and can be cut
                cuttingProgress++;
            
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            
                CuttingRecipeSO cuttingRecipeSo = getCuttingRecipeSOWithinput(GetKitchenObject().GetKitchenObjectSo());
            
                OnProgressChanged?.Invoke(this,
                    new IHasProgress.OnProgressChangedEventArgs
                        { progressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax });
            
                if (cuttingProgress >= cuttingRecipeSo.cuttingProgressMax) {
                    // cutting done
                    KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(output, this);
                }

            }
        }

        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
            CuttingRecipeSO cuttingRecipeSo = getCuttingRecipeSOWithinput(inputKitchenObjectSO);
            return cuttingRecipeSo != null;
        }

        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
            CuttingRecipeSO cuttingRecipeSo = getCuttingRecipeSOWithinput(inputKitchenObjectSO);
            if (cuttingRecipeSo != null) {
                return cuttingRecipeSo.output;
            }
            else {
                return null;
            }
        }

        private CuttingRecipeSO getCuttingRecipeSOWithinput(KitchenObjectSO inputKitchenObjectSO) {
            foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
                if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                    return cuttingRecipeSO;
                }
            }

            return null;
        }
    }
}