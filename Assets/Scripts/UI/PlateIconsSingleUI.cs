using System.Collections;
using System.Collections.Generic;
using Kitchen.SO;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSo) {
        image.sprite = kitchenObjectSo.sprite;
        
    }
}
