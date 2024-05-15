using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField]
    private Vector2 input;
    [SerializeField]
    private float speed = 5;


    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(OwnerClientId + "; " + randomNumber.Value);

        if (!IsOwner)
        {
            return;
        }

        /*
        if(Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 100);
            
        }
        */


        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");


        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.position += move * Time.deltaTime * speed;
    }
}
