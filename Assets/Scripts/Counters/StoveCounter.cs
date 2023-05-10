using System;
using System.Collections;
using Kitchen;
using Kitchen.SO;
using UnityEngine;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
        public class OnStateChangedEventArgs : EventArgs
        {
            public State state;
        }
        
        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned
        }

        [SerializeField] private FryingRecipeSO[] fryingRecipesSOArray;
        [SerializeField] private BurningRecipeSO[] burningRecipesSOArray;

        private State state;
        private float fryingTimer;
        private float burningTimer;

        private FryingRecipeSO fryingRecipeSo;
        private BurningRecipeSO burningRecipeSo;

        private void Start() {
            state = State.Idle;
        }

        private void Update() {
            if (HasKitchenObject()) {
                switch (state) {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax});

                        if (fryingTimer > fryingRecipeSo.fryingTimerMax) {
                            //fried
                            Debug.Log("fried");
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(fryingRecipeSo.output, this);

                            state = State.Fried;
                            burningTimer = 0f;
                            burningRecipeSo = getBurningRecipeSOWithinput(GetKitchenObject().GetKitchenObjectSo());
                            
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{state = state});

                        }

                        break;

                    case State.Fried:
                        burningTimer += Time.deltaTime;
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = burningTimer / burningRecipeSo.burningTimerMax});

                        if (burningTimer > burningRecipeSo.burningTimerMax) {
                            //fried
                            GetKitchenObject().DestroySelf();

                            KitchenObject.SpawnKitchenObject(burningRecipeSo.output, this);
                            
                            Debug.Log("Object Burned!");
                            state = State.Burned;
                            
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{state = state});
                            
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = 0f});
                        }

                        break;

                    case State.Burned:
                        break;
                }
            }
        }

        public override void Interact(Player player) {
            if (!HasKitchenObject()) {
                // no object on counter
                if (player.HasKitchenObject()) {
                    // player has object
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) {
                        //Player has object that can be fried
                        player.GetKitchenObject().SetKitchenObjectParent(this);

                        fryingRecipeSo = getFryingRecipeSOWithinput(GetKitchenObject().GetKitchenObjectSo());
                        
                        state = State.Frying;
                        fryingTimer = 0f;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{state = state});
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax});
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

                            state = State.Idle;
                    
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{state = state});
                    
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = 0f});
                        }
                        
                    }
                }
                else {
                    // player has no object
                    GetKitchenObject().SetKitchenObjectParent(player);
                    
                    state = State.Idle;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{state = state});
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = 0f});
                    
                }
            }
        }

        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
            FryingRecipeSO fryingRecipeSO = getFryingRecipeSOWithinput(inputKitchenObjectSO);
            return fryingRecipeSO != null;
        }

        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
            FryingRecipeSO fryingRecipeSO = getFryingRecipeSOWithinput(inputKitchenObjectSO);
            if (fryingRecipeSO != null) {
                return fryingRecipeSO.output;
            }
            else {
                return null;
            }
        }

        private FryingRecipeSO getFryingRecipeSOWithinput(KitchenObjectSO inputKitchenObjectSO) {
            foreach (FryingRecipeSO fryingRecipeSO in fryingRecipesSOArray) {
                if (fryingRecipeSO.input == inputKitchenObjectSO) {
                    return fryingRecipeSO;
                }
            }

            return null;
        }
        
        private BurningRecipeSO getBurningRecipeSOWithinput(KitchenObjectSO inputKitchenObjectSO) {
            foreach (BurningRecipeSO burningRecipeSo in burningRecipesSOArray) {
                if (burningRecipeSo.input == inputKitchenObjectSO) {
                    return burningRecipeSo;
                }
            }

            return null;
        }
    }
}