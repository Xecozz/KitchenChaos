using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kitchen;
using Kitchen.SO;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class  OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSo;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSoList;
    private List<KitchenObjectSO> KitchenObjectSoList;

    private void Awake() {
        KitchenObjectSoList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo) {
        if (!validKitchenObjectSoList.Contains(kitchenObjectSo)) {
            return false;
        }
        if (KitchenObjectSoList.Contains(kitchenObjectSo)) {
            return  false;
        }
        else {
            KitchenObjectSoList.Add(kitchenObjectSo);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{ kitchenObjectSo = kitchenObjectSo});
            return true;
        }
        
    }
    
    public List<KitchenObjectSO> GetKitchenObjectSoList() {
        return KitchenObjectSoList;
    }
}
