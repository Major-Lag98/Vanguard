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

    public GameObject CropCoin;
    public Light2D GlobalLight;
    public Spawner Spawner;
    public float Timer; // Used for time of day cycle (buying, planting, placing towers) and night (time between attacks)
    public TextMeshProUGUI TimerText;
    public float TimeToSellCrops = 5f;

    [SerializeField]
    AudioSource BackgroundAudio;

    [Tooltip("The time between attacks from enemies at night.")]
    public float TimeBetweenAttacks = 60;

    [Tooltip("The time for the player to buy and place stuff during the peaceful day time")]
    public float TimeForDayCycle = 180;

    public enum State { Day, Night, Attacking};
    State currState = State.Day;

    bool sellingCrops;

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
        currState = State.Day;
        ToNight();
        
        CropManager.Instance.AddStopSellingCropDelegate(() => { sellingCrops = false; ToDay(); });
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
                TimerText.text = FormatTimeText((int)Timer);

                if (!sellingCrops)
                {
                    if (Timer <= 0)
                        ToAttacking();
                }

                break;

            case State.Attacking:
                if (!Spawner.Spawning && EnemyManager.Instance.goblinList.Count == 0)
                    ToNight();
                break;
        }
    }

    void ToNight()
    {
        BackgroundAudio.pitch = .5f;
        DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 0.5f, 1f);

        // Load the next wave when we go into night state. If loaded properly, we can proceed normally
        if (Spawner.LoadNextWave())
        {
            Timer = TimeBetweenAttacks; // Reset our timer
            if (currState == State.Day) // If we're coming from day, go straight to attacking
                ToAttacking();
            else // Otherwise go to regular night cycle
            {
                currState = State.Night;
                gameStateChanged?.Invoke(currState);
            }
        }
        // If the wave was not loaded (no more spawns), we can transition to day instead
        else if(!sellingCrops)
        {
            Timer = TimeToSellCrops;
            sellingCrops = true;
            CropManager.Instance.SellCrops();
            currState = State.Night;
            gameStateChanged?.Invoke(currState);
        }
    }

    void ToAttacking()
    {
        Spawner.Spawning = true;
        currState = State.Attacking;
        gameStateChanged?.Invoke(currState);
    }

    void ToDay()
    {
        BackgroundAudio.pitch = 1;
        DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 1f, 1f);

        Timer = TimeForDayCycle; // Reset time
        TimerText.text = FormatTimeText((int)Timer); // Set the text
        currState = State.Day;
        gameStateChanged?.Invoke(currState);
    }

    /// <summary>
    /// Sets the state to day
    /// </summary>
    public void SetDay()
    {
        ToDay();
    }

    /// <summary>
    /// Sets the state to night
    /// </summary>
    public void SetNight()
    {
        ToNight();
    }

    /// <summary>
    /// Skips the current state
    /// </summary>
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
        var minutes = Mathf.Clamp(time / 60, 0, 100);
        var seconds = Mathf.Clamp(time % 60, 0, 60);
        return $"{minutes}:{seconds:00}";
    }

    public void AddGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged += del;

    public void RemoveGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged -= del;
}
