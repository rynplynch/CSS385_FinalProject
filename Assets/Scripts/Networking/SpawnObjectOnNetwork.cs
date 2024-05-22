using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

[RequireComponent(typeof(NetworkObject))]
public class SpawnObjectOnNetwork : NetworkBehaviour
{

    private NetworkObject no;

    // Start is called before the first frame update
    void Start()
    {
        no = GetComponent<NetworkObject>();
        TestServerRPC();
    }

    [ServerRpc(RequireOwnership =false)]
    private void TestServerRPC()
    {
        Debug.Log("Test ServerRPC");
        no.Spawn(true);
    }

}
