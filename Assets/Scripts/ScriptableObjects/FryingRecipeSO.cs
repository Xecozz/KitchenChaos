using UnityEngine;

namespace Kitchen.SO
{
    [CreateAssetMenu()]
    public class FryingRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public float fryingTimerMax;
    }
}
