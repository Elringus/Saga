using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExitGames.Client.Photon;
using UnityEngine;

class UnitOperations : IPhotonPeerListener
{
    public bool Connected;
    private PhotonPeer peer;
    private string ActorID;

    public UnitOperations(string id)
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);

        ActorID = id;

        peer.Connect("191.238.97.27:5055", "UnitOperations");
    }

    public void PeerService()
    {
        peer.Service();
    }
   

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void RequestAttack(string actorID, int damage)
    {
        var parameters = new Dictionary<byte, object> { { 4, 3 }, { 3, "I kicked myself" }, { 1, ActorID } };
        peer.OpCustom(12, parameters,true);
    }

    public void RequestMessage()
    {

    }
    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 12:
                ResponseAttack(eventData.Parameters);
                break;
        }
    }

    private void ResponseAttack(Dictionary<byte,object> parametrs)
    {
        string id = parametrs[1].ToString();
        string msg = parametrs[3].ToString();
        int dmg = (int)parametrs[4];
        MmoEngine.I.actors[id].TakeDamage(dmg, msg);

    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        if (statusCode == StatusCode.Connect)
        {
            this.Connected = true;
            Debug.Log("On Connect To UnitOperations");
        }
        else
        {
            Debug.Log("Error connection to UnitOperations: "+statusCode.ToString());
        }
    }
}
