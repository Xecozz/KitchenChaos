using System;
using Kitchen;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        
        public static event EventHandler OnAnyObjectPlaceHere;
        [SerializeField] private Transform counterTopPoint;
        private KitchenObject kitchenObject;
    
        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter.Interact() called!");
        }
    
        public virtual void InteractAlternate(Player player)
        {
            //Debug.LogError("BaseCounter.InteractAlternate() called!");
        }
    
        public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

        public void SetKitchenObject(KitchenObject kitchenObject) {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null) {
                OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public KitchenObject GetKitchenObject() => kitchenObject;

        public void ClearKitchenObject() {
            kitchenObject = null;
        }

        public bool HasKitchenObject() => kitchenObject != null;
    
    }
}
