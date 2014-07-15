using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using ExitGames.Client.Photon;

public class Chat : IPhotonPeerListener
{
	public bool Connected;
	private PhotonPeer peer;

	public Chat ()
	{
		peer = new PhotonPeer(this, ConnectionProtocol.Tcp);

		ChatGUI.MessagesTab1.Add("Connecting to chat server...");

		peer.Connect("191.238.97.27:4530", "ChatServer");
	}

	public void PeerService ()
	{
		peer.Service();
	}

	public void SendMessage (int tab)
	{
		var parameters = new Dictionary<byte, object> { { 0, string.Format("[{0}] {1}: {2}",
				DateTime.Now.ToString("HH:mm:ss"), MmoEngine.I.engine.Avatar.Text, ChatGUI.ChatInput) } };
		peer.OpCustom((byte)tab, parameters, true);
		ChatGUI.ChatInput = string.Empty;
	}

	public void Attack (string actorID, int damage)
	{
		var parameters = new Dictionary<byte, object> { { 2, new object[] { actorID, damage } } };
		peer.OpCustom(2, parameters, true);
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		ChatGUI.MessagesTab1.Add(level + ": " + message);
	}

	public void OnEvent (EventData eventData)
	{
		switch (eventData.Code)
		{
			case 1: 
				ChatGUI.MessagesTab1.Add(eventData.Parameters[0].ToString());
				break;
			case 2:
				ChatGUI.MessagesTab2.Add(eventData.Parameters[0].ToString());
				break;
			case 3:
				ChatGUI.MessagesTab3.Add(eventData.Parameters[0].ToString());
				break;
				//object[] prm = eventData.Parameters[2] as object[];
				//string id = prm[0] as string;
				//int damage = (int)prm[1];

				//MmoEngine.I.actors[id].HP -= damage;
				//break;
		}
	}

	public void OnOperationResponse (OperationResponse operationResponse)
	{
		//ChatGUI.Messages.Add("Response: " + operationResponse.OperationCode);
	}

	public void OnStatusChanged (StatusCode statusCode)
	{
		if (statusCode == StatusCode.Connect)
		{
			this.Connected = true;
		}
		else
		{
			ChatGUI.MessagesTab1.Add("Status: " + statusCode);
		}
	}
}