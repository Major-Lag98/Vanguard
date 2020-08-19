using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public delegate void GameStateChangedDelegate(State state);

    public Light2D GlobalLight;
    public Spawner Spawner;
    public float Timer; // Used for time of day cycle (buying, planting, placing towers) and night (time between attacks)
    public TextMeshProUGUI TimerText;
    public Button SkipCycleButton;

    [Tooltip("The time between attacks from enemies at night.")]
    public float TimeBetweenAttacks = 60;

    [Tooltip("The time for the player to buy and place stuff during the peaceful day time")]
    public float TimeForDayCycle = 180;

    public enum State { Day, Night, Attacking};
    State currState = State.Day;

    GameStateChangedDelegate gameStateChanged;

    public static GameStateManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ToDay();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case State.Day:
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                    ToNight();
                else
                    TimerText.text = FormatTimeText((int)Timer);

                break;

            case State.Night:
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                    ToAttacking();
                else
                    TimerText.text = FormatTimeText((int)Timer);

                break;

            case State.Attacking:
                if (!Spawner.Spawning && EnemyManager.Instance.goblinList.Count == 0)
                    ToNight();
                break;
        }
    }

    void ToNight()
    {
        // Load the next wave when we go into night state. If loaded properly, we can proceed normally
        if (Spawner.LoadNextWave())
        {
            Timer = TimeBetweenAttacks; // Reset our timer
            ActivateTimerText((int)Timer); // Activate the timer text
            SkipCycleButton.gameObject.SetActive(true); // Activate the skip button
            currState = State.Night;
            gameStateChanged?.Invoke(currState);
        }
        // If the wave was not loaded (no more spawns), we can transition to day instead
        else
        {
            ToDay();
        }
        
    }

    void ToAttacking()
    {
        Spawner.Spawning = true;
        currState = State.Attacking;
        TimerText.gameObject.SetActive(false); // Hide the timer text
        SkipCycleButton.gameObject.SetActive(false); // Hide the skip button
        currState = State.Attacking;
        gameStateChanged?.Invoke(currState);
    }

    void ToDay()
    {
        Timer = TimeForDayCycle;
        TimerText.text = FormatTimeText((int)Timer);
        SkipCycleButton.gameObject.SetActive(true);
        currState = State.Day;
        gameStateChanged?.Invoke(currState);
    }

    public void SetDay()
    {
        DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 1f, 1f);
        ToDay();
    }

    public void SetNight()
    {
        DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 0.5f, 1f);
        ToNight();
    }

    public void SkipCurrentState()
    {
        if (currState == State.Day)
            SetNight();
        else if (currState == State.Night)
            ToAttacking();
    }

    void ActivateTimerText(int time)
    {
        TimerText.gameObject.SetActive(true);
        TimerText.text = FormatTimeText(time);
    }

    string FormatTimeText(int time)
    {
        var minutes = time / 60;
        var seconds = time % 60;
        return $"{minutes}:{seconds}";
    }

    public void AddGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged += del;

    public void RemoveGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged -= del;
}
