using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{

    private bool reserved;

    // Start is called before the first frame update
    void Start()
    {
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
