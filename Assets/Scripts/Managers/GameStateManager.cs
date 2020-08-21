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
            sellingCrops = true;
            StartCoroutine(SellCrops());
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
        DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 1f, 1f);

        Timer = TimeForDayCycle; // Reset time
        TimerText.text = FormatTimeText((int)Timer); // Set the text
        currState = State.Day;
        gameStateChanged?.Invoke(currState);
    }

    /// <summary>
    /// Sells each crop (with an animation) adding credits to the player
    /// </summary>
    /// <returns></returns>
    IEnumerator SellCrops()
    {
        Timer = TimeToSellCrops;

        // Get a copy of the crop list
        var crops = new List<Crop>(CropManager.Instance.GetCropList());
        crops.Shuffle(); // Shuffle it
        var timerPerCrop = TimeToSellCrops / crops.Count; // The time we spend per crop

        foreach (var crop in crops)
        {
            var pos = crop.transform.position + new Vector3(0, 0.5f, -1); // Boost the starting position up a little
            var coin = Instantiate(CropCoin, pos, Quaternion.identity); // The coin
            TweenCoin(coin); // Our tween function for good looks
            TweenCrop(crop.gameObject);
            PlayerData.Instance.Credits += crop.CreditsAtHarvest; // Add the credits to the player
            yield return new WaitForSeconds(timerPerCrop); // Wait our specified amount of time
        }

        yield return new WaitForSeconds(2f); // TODO Magic number
        ToDay(); // Then transition to day
        sellingCrops = false; // Stop selling crops
        yield break;
    }

    void TweenCoin(GameObject coin)
    {
        var seq = DOTween.Sequence();
        var origY = coin.transform.position.y;
        seq.Append(coin.transform.DOMoveY(origY + 1, 0.3f).SetEase(Ease.OutSine))
            .Append(coin.transform.DOMoveY(origY + 0.5f, 0.3f).SetEase(Ease.InSine))
            .OnComplete(() => Destroy(coin));
    }

    void TweenCrop(GameObject crop)
    {
        var seq = DOTween.Sequence();
        var origY = crop.transform.position.y;
        seq.Append(crop.transform.DOMoveY(origY - 0.25f, 0.1f).SetEase(Ease.Linear))
            .Append(crop.transform.DOMoveY(origY, 0.1f).SetEase(Ease.Linear));
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
        var minutes = time / 60;
        var seconds = time % 60;
        return $"{minutes}:{seconds}";
    }

    public void AddGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged += del;

    public void RemoveGameStateChangedDelegate(GameStateChangedDelegate del)
        => gameStateChanged -= del;
}
