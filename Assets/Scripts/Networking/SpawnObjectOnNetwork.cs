using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public class SpawnObjectOnNetwork : MonoBehaviour
{

    private NetworkObject no;

    // Start is called before the first frame update
    void Start()
    {
        no = GetComponent<NetworkObject>();
        no.Spawn();
    }

}
