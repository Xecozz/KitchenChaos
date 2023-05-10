using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipeRefsSO audioClipeRefsSo;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomethingUp;
        BaseCounter.OnAnyObjectPlaceHere += BaseCounter_OnAnyObjectPlaceHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }
    
    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e) {
        PlaySound(audioClipeRefsSo.trash, ((TrashCounter) sender).transform.position);
    }
    
    private void BaseCounter_OnAnyObjectPlaceHere(object sender, EventArgs e) {
        PlaySound(audioClipeRefsSo.objectDrop, ((BaseCounter) sender).transform.position);
    }
    
    private void Player_OnPickedSomethingUp(object sender, EventArgs e) {
        PlaySound(audioClipeRefsSo.objectPickup, Player.Instance.transform.position);
    }
    
    private void CuttingCounter_OnAnyCut(object sender, EventArgs e) {
        PlaySound(audioClipeRefsSo.chop, ((CuttingCounter) sender).transform.position);
    }
    
    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipeRefsSo.deliverySuccess, deliveryCounter.transform.position);
    }
    
    private void DeliveryManager_OnRecipeFail(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipeRefsSo.deliveryFail, deliveryCounter.transform.position);
    }


    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    
    
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
    public void PlayFootStepSound(Vector3 position, float volume = 1f) {
        PlaySound(audioClipeRefsSo.footstep, position, volume);
    }
}
