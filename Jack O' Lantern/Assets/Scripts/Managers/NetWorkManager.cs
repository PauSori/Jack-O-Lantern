using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetWorkManager : NetworkManager
{
   public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Cliente conectado en el servidor");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log("Un jugador se ha añadido al servidor");
    }
}
