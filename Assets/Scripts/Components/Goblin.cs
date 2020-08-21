using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour, IDamageable
{
    public int MoveTickSpeed = 2;

    private Vector3[] points;
    private int currIter;

    private GameObject targetCrop;

    enum StateEnum {Moving, Attacking, TakingPlant, Leaving}

    private StateEnum currState = StateEnum.Moving;


    public float health = 3;
    private bool dead;

    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(MoveAlongPath, MoveTickSpeed);
    }

    void MoveAlongPath(int time)
    {
        var target = points[currIter];
        Move(target);
        currIter++;

        if (currIter >= points.Length)
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveAlongPath, MoveTickSpeed); // Remove our current callback

            // Get an unreserved crop, reserve it, and set it as our target
            var crop = CropManager.Instance.GetCropList().Find(c => !c.IsReserved());

            // If there is a crop available, reserve it and move towards it
            if (crop != null)
            {
                crop.SetReserved(true);
                CropManager.Instance.RemoveCrop(crop);
                targetCrop = crop.gameObject;
                currState = StateEnum.TakingPlant; // When we get to the end, take a plant
                TimeTicker.Instance.AddOnTimeTickDelegate(MoveToPlant, MoveTickSpeed); // Add the next callback

            // Otherwise just move off screen
            }else
                TimeTicker.Instance.AddOnTimeTickDelegate(MoveOffScreen, MoveTickSpeed); // Remove our current callback

        }

    }

    void MoveToPlant(int time)
    {
        MoveFreeTowardsTarget(targetCrop.transform.position);

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), 
            new Vector2(targetCrop.transform.position.x, targetCrop.transform.position.y)) < 0.2f)
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveToPlant, MoveTickSpeed);
            TimeTicker.Instance.AddOnTimeTickDelegate(MoveOffScreen, MoveTickSpeed);
            currState = StateEnum.Leaving; // When we get to the end, take a plant
            Destroy(targetCrop.gameObject);
        }
    }

    void MoveOffScreen(int time)
    {
        var end = new Vector3(-22.5f, transform.position.y);
        MoveFreeTowardsTarget(end);

        // If we're at the end
        if (Vector3.Distance(end, transform.position) < 0.2f)
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveOffScreen, MoveTickSpeed);
            EnemyManager.Instance.goblinList.Remove(this.gameObject);
            Destroy(gameObject);
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
        return Mathf.Round((coord + 1 * dir)*2) * 0.5f;
    }

    private void Move(Vector3 targetPostion)
    {
        transform.DOMove(targetPostion, 0.5f).SetEase(Ease.OutExpo);
    }

    public void SetPoints(Vector3[] points)
    {
        this.points = points;
    }

    public void Hit(float angle)
    {
        var speed = 0.05f;
        var scale = 0.3f;

        var x = Mathf.Cos(angle);
        var y = Mathf.Sin(angle);

        var target = Vector3.MoveTowards(transform.position, transform.position + new Vector3(x, y) * scale, 1f);
        var original = transform.position;

        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, speed))
            .Append(transform.DOMove(original, speed));
        
    }

    private void OnDestroy()
    {
        TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveAlongPath, MoveTickSpeed);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0 && !dead)
        {
            EnemyManager.Instance.goblinList.Remove(gameObject);
            Destroy(gameObject);
            dead = true;
        }

        //Debug.Log($"Took damage, health is now {health}");
    }

}
