using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public float TimeToSellCrops;
    public GameObject CropCoin;

    public delegate void OnSellingCropsEventDelegate();

    private OnSellingCropsEventDelegate OnStartSellingCrops;
    private OnSellingCropsEventDelegate OnStopSellingCrops;
    private OnSellingCropsEventDelegate OnSoldCrop;

    public static CropManager Instance;

    private List<Crop> cropList = new List<Crop>();

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("There should only ever be one CropManager");

        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCrop(Crop crop)
    {
        cropList.Add(crop);
    }

    public void RemoveCrop(Crop crop)
    {
        cropList.Remove(crop);
    }

    public void DestroyCrop(Crop crop)
    {
        cropList.Remove(crop);
        Destroy(crop.gameObject);
    }

    public List<Crop> GetCropList() => cropList;

    public void SellCrops()
    {
        StartCoroutine(SellCropsCoroutine());
    }

    /// <summary>
    /// Sells each crop (with an animation) adding credits to the player
    /// </summary>
    /// <returns></returns>
    IEnumerator SellCropsCoroutine()
    {
        OnStartSellingCrops?.Invoke();

        // Get a copy of the crop list
        var crops = new List<Crop>(cropList);
        crops.Shuffle(); // Shuffle it
        var timerPerCrop = TimeToSellCrops / crops.Count; // The time we spend per crop

        foreach (var crop in crops)
        {
            var pos = crop.transform.position + new Vector3(0, 0.5f, -1); // Boost the starting position up a little
            var coin = Instantiate(CropCoin, pos, Quaternion.identity); // The coin
            TweenCoin(coin); // Our tween function for good looks
            TweenCrop(crop.gameObject); // Tween our crop
            audioSource.Play(); // Play our sound 
            OnSoldCrop?.Invoke(); // Invoke our delegates
            PlayerData.Instance.AddCredits(crop.CreditsAtHarvest); // Add the credits to the player
            yield return new WaitForSeconds(timerPerCrop); // Wait our specified amount of time
        }

        yield return new WaitForSeconds(2f); // TODO Magic number
        OnStopSellingCrops?.Invoke();
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

    public void AddStartSellingCropDelegate(OnSellingCropsEventDelegate del)
        => OnStartSellingCrops += del;

    public void AddStopSellingCropDelegate(OnSellingCropsEventDelegate del)
       => OnStopSellingCrops += del;

    public void AddOnCropSoldDelegate(OnSellingCropsEventDelegate del)
       => OnSoldCrop += del;

    public void RemoveStartSellingCropDelegate(OnSellingCropsEventDelegate del)
       => OnStartSellingCrops -= del;

    public void RemoveStopSellingCropDelegate(OnSellingCropsEventDelegate del)
       => OnStopSellingCrops -= del;

    public void RemoveOnCropSoldDelegate(OnSellingCropsEventDelegate del)
       => OnSoldCrop -= del;



}
