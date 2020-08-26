using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector]
    public Vector3 targetPosition;

    [SerializeField]
    float speed = 5;

    public Goblin enemy = null;

    public int Damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.position += moveDirection * speed * Time.deltaTime;

        float angle = GetAngleFromVectorFloat(moveDirection);
        transform.eulerAngles = new Vector3(0,0,angle);


        if (Vector3.Distance(transform.position, targetPosition) < .2f)
        {
            gameObject.SetActive(false);

            if (enemy != null)
            {
                enemy.Damage(Damage);
                enemy.Hit(angle);
            }

            Destroy(gameObject);
        }

    }

    float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return n;
    }
}
