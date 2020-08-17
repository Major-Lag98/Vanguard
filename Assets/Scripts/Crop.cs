using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Crop : MonoBehaviour
{

    private bool reserved;

    // Start is called before the first frame update
    void Start()
    {
        CropManager.Instance.AddCrop(this);
        var x = Mathf.Round(transform.position.x * 2) * 0.5f;
        var y = Mathf.Round(transform.position.y * 2) * 0.5f;

        transform.position = new Vector3(x, y, -1); // Snaps this crop to the grid since unity's grid snapping sucks
    }

    public void SetReserved(bool value)
    {
        // If we're trying to reserve but it's already reserved
        if (value && IsReserved())
            return;

        reserved = true;
    }

    public bool IsReserved()
        => reserved;
}
