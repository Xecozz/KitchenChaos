using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    
    public static KitchenGameManager Instance { get; private set; }
    
    public event EventHandler OnStateChange;
    

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlay,
        GameOver
            
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float coutdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f) {
                    state = State.CountdownToStart;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                coutdownToStartTimer -= Time.deltaTime;
                if (coutdownToStartTimer <= 0f) {
                    state = State.GamePlay;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlay:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f) {
                    state = State.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }
    
    public bool IsGamePlaying() {
        return state == State.GamePlay;
    }
    
    public bool IsCoutdownToStartActive() {
        return state == State.CountdownToStart;
    }
    
    public float GetCountdownToStartTimer() {
        return coutdownToStartTimer;
    }
    
    public bool IsGameOver() {
        return state == State.GameOver;
    }
    
    public float GetPlayingTimerNormalized() {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }
}
