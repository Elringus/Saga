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

        peer.Connect("191.238.97.27:5055", "Lite");
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
        var parameters = new Dictionary<byte, object> { { (byte)4, damage }, { (byte)3, "I kicked myself" }, { (byte) 1, ActorID } };
        peer.OpCustom(12, parameters,true);

        foreach (object obj in parameters)
            Debug.Log(obj);
    }

    public void RequestMessage()
    {

    }
    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 12:
                //ResponseAttack(eventData.Parameters);
                break;
        }
    }

    private void ResponseAttack(Dictionary<byte,object> parameters)
    {
        foreach (object obj in parameters)
            Debug.Log(obj);

        Debug.Log(parameters.Count);

        string id = parameters[1].ToString();
        string msg = parameters[3].ToString();
        int dmg = (int)parameters[4];
        MmoEngine.I.actors[id].TakeDamage(dmg, msg);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case 12:
                ResponseAttack(operationResponse.Parameters);
                break;
            case 11:
                ResponseMsg(operationResponse);
                break;
        }
    }

    private void ResponseMsg(OperationResponse operationResponse)
    {
        // OK
        Debug.Log(operationResponse.Parameters[3]);
        if (operationResponse.ReturnCode == 0)
        {
            // show the complete content of the response
            Debug.Log(operationResponse.ToStringFull());
        }
        else
        {
            // show the error message
            Debug.Log(operationResponse.DebugMessage);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        if (statusCode == StatusCode.Connect)
        {
            this.Connected = true;
            Debug.Log("On Connect To UnitOperations");
            var parameter = new Dictionary<byte, object>();
            parameter.Add((byte)3, "Hello World");

            peer.OpCustom(11, parameter, true);
        }
        else
        {
            Debug.Log("Error connection to UnitOperations: "+statusCode.ToString());
        }
    }
}
