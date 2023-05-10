using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{
    
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] VisualGameObjectArray;
    
    private void Start() {
        Player.Instance.OnSelectCounterChanged += Player_OnSelectCounterChanged;
    }
    
    private void Player_OnSelectCounterChanged (object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.SelectedCounter == baseCounter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        foreach (var visualGameObject in VisualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
       
    }
    
    private void Hide() {
        foreach (var visualGameObject in VisualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}