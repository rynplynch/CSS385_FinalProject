using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifespan = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
       Destroy(gameObject);
    }
}
