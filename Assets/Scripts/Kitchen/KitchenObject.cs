using Kitchen.SO;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSo;
        private IKitchenObjectParent kitchenObjectParent;

        public KitchenObjectSO GetKitchenObjectSo() => kitchenObjectSo;

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
            if (this.kitchenObjectParent != null) {
                this.kitchenObjectParent.ClearKitchenObject();
            }

            this.kitchenObjectParent = kitchenObjectParent;

            if (kitchenObjectParent.HasKitchenObject()) {
                Debug.LogError("Counter already has a kitchen object");
            }

            kitchenObjectParent.SetKitchenObject(this);
            transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }

        public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;
    
        public void DestroySelf() {
            kitchenObjectParent.ClearKitchenObject();
            Destroy(gameObject);
        }

        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
            if (this is PlateKitchenObject) {
                plateKitchenObject = this as PlateKitchenObject ;
                return true;
            }
            else {
                plateKitchenObject = null;
                return false;
            }
        }


        public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        
            return kitchenObject;
        }
    }
}