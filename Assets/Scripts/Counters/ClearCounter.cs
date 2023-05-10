using Kitchen.SO;
using UnityEngine;

namespace Counters
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSo;

        public override void Interact(Player player) {
            if (!HasKitchenObject()) {
                // no object on counter
                if(player.HasKitchenObject())
                {
                    // player has object
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                else {
                    //player has no object
                }
            }else{
                // object on counter
                if (player.HasKitchenObject())
                {
                    // player has object
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                        //player has plate
                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo())) {
                            GetKitchenObject().DestroySelf();
                        }
                        
                    }
                    else {
                        // Player is not carrying a plate but something else
                        if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo())) {
                                player.GetKitchenObject().DestroySelf();
                            }
                        }
                    }

                }
                else
                {
                    // player has no object
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

    }
}