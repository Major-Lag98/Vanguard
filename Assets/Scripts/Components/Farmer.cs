using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public int MoveTickSpeed = 1;
    public Transform Home;

    private GameObject targetCrop;

    private bool active;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartPlanting()
    {
        active = true;
        gameObject.SetActive(true);
        targetCrop = CropManager.Instance.GetNextGhostCrop();
        TimeTicker.Instance.AddOnTimeTickDelegate(MoveToPlant, MoveTickSpeed);
    }

    void MoveToPlant(int time)
    {
        if (!active)
            return;

        MoveFreeTowardsTarget(targetCrop.transform.position);

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
            new Vector2(targetCrop.transform.position.x, targetCrop.transform.position.y)) < 0.2f)
        {
            CropManager.Instance.CreateCropAtLocation(targetCrop.transform.position);
            Destroy(targetCrop);
            targetCrop = CropManager.Instance.GetNextGhostCrop();
            if(targetCrop == null)
            {
                TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveToPlant, MoveTickSpeed);
                TimeTicker.Instance.AddOnTimeTickDelegate(MoveToHome, MoveTickSpeed);
            }
        }
    }

    void MoveToHome(int time)
    {
        if (!active)
            return;

        MoveFreeTowardsTarget(Home.position);

        // If we're moving home but we have another crop to place
        if (CropManager.Instance.HasNextGhostPlant())
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveToHome, MoveTickSpeed);
            StartPlanting();
            return;
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
            new Vector2(Home.position.x, Home.position.y)) < 0.2f)
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveToHome, MoveTickSpeed);
            active = false;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Moves towards a target position freely (not following a path)
    /// </summary>
    /// <param name="targetPosition"></param>
    void MoveFreeTowardsTarget(Vector3 targetPosition)
    {
        // The difference between our x and y and the target's x and y
        var yDiff = transform.position.y - targetPosition.y;
        var xDiff = transform.position.x - targetPosition.x;

        // If our Y is at least one cell off, move towards
        if (Mathf.Abs(yDiff) > 0.6f)
        {
            var dir = yDiff > 0 ? -1 : 1;
            float y = StepTowards(transform.position.y, dir);
            var x = transform.position.x;
            Move(new Vector3(x, y));

        }
        // Otherwise if our X is at least one cell off, move towards
        else if (Mathf.Abs(xDiff) > 0.6f)
        {
            var dir = xDiff > 0 ? -1 : 1;
            float x = StepTowards(transform.position.x, dir);
            var y = transform.position.y;
            Move(new Vector3(x, y));
        }
    }

    float StepTowards(float coord, int dir)
    {
        return Mathf.Round((coord + 1 * dir) * 2) * 0.5f;
    }

    private void Move(Vector3 targetPostion)
    {
        transform.DOMove(targetPostion, 0.5f).SetEase(Ease.OutExpo);
    }

    public bool IsActive()
        => active;

}
