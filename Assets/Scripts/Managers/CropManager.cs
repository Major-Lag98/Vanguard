using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{

    public static CropManager Instance;

    private List<Crop> cropList = new List<Crop>();

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("There should only ever be one CropManager");

        Instance = this;
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
}
