using System.Collections;
using System.Collections.Generic;
using Kitchen.SO;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectsSOList;
    public string recipeName;
    
}
