using System;
using Kitchen;
using Kitchen.SO;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter
    {
    
        public event EventHandler OnPlayerGrabbedObject; 

        [SerializeField] private KitchenObjectSO kitchenObjectSo;

    
        // Start is called before the first frame update
        public override void Interact(Player player) {
            if (!HasKitchenObject()) {
                if (!player.HasKitchenObject()) {
                    // player has no object in hand
                    KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);
                
                    OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
                }
                else {
                    // player has object in hand
                }

            }
        }

    }
}
