using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{

    public static CropManager Instance;


    private List<Crop> cropList = new List<Crop>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            throw new System.Exception("There should only ever be one CropManager");

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCrop()
    {

    }

    public void RemoveCrop()
    {

    }

    public void DestroyCrop()
    {

    }
}
