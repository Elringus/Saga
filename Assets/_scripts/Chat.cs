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

		ChatGUI.Messages.Add("Connecting to chat server...");

		//this.Connected = false;
		peer.Connect("191.238.97.27:4530", "ChatServer");

		//while (!this.connected)
		//{
		//	peer.Service();
		//}

		//var buffer = new StringBuilder();
		//while (true)
		//{
		//	peer.Service();

		//	// read input
		//	if (Console.KeyAvailable)
		//	{
		//		ConsoleKeyInfo key = Console.ReadKey();
		//		if (key.Key != ConsoleKey.Enter)
		//		{
		//			// store input
		//			buffer.Append(key.KeyChar);
		//		}
		//		else
		//		{
		//			// send to server
		//			var parameters = new Dictionary<byte, object> { { 1, buffer.ToString() } };
		//			peer.OpCustom(1, parameters, true);
		//			buffer.Length = 0;
		//		}
		//	}
		//}
	}

	public void PeerService ()
	{
		peer.Service();
	}

	public void SendMessage ()
	{
		var parameters = new Dictionary<byte, object> { { 1, string.Format("[{0}] {1}: {2}",
				DateTime.Now.ToString("HH:mm:ss"), MmoEngine.I.engine.Avatar.Text, ChatGUI.ChatInput) } };
		peer.OpCustom(1, parameters, true);
		ChatGUI.ChatInput = string.Empty;
	}

	public void Attack (string actorID, int damage)
	{
		var parameters = new Dictionary<byte, object> { { 2, new object[] { actorID, damage } } };
		peer.OpCustom(2, parameters, true);
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		ChatGUI.Messages.Add(level + ": " + message);
	}

	public void OnEvent (EventData eventData)
	{
		switch (eventData.Code)
		{
			case 1: 
				ChatGUI.Messages.Add(eventData.Parameters[1].ToString());
				break;
			case 2:
				object[] prm = eventData.Parameters[2] as object[];
				string id = prm[0] as string;
				int damage = (int)prm[1];

				MmoEngine.I.actors[id].HP -= damage;
				break;
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
			ChatGUI.Messages.Add("Status: " + statusCode);
		}
	}
}