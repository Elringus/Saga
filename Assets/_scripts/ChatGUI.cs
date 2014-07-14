using UnityEngine;
using System.Collections.Generic;

public class ChatGUI : MonoBehaviour
{
	public static string ChatInput = "";
	public static List<string> Messages = new List<string>();

	private int chatHeight = 250;
	private Vector2 chatScroll = Vector2.zero;

	private Chat chat;

	private void Awake () 
	{
		chat = new Chat();
	}

	private void Update () 
	{
		chat.PeerService();
	}

	private void OnGUI ()
	{
		GUI.GetNameOfFocusedControl();

		GUI.Box(new Rect(10, Screen.height - chatHeight - 10, 500, chatHeight), string.Empty);
		GUILayout.BeginArea(new Rect(10, Screen.height - chatHeight - 10, 500, chatHeight));
		chatScroll = GUILayout.BeginScrollView(chatScroll);
		for (int i = Messages.Count - 1; i >= 0; i--) GUILayout.Label(Messages[i]);
		GUILayout.EndScrollView();
		ChatInput = GUILayout.TextField(ChatInput);
		if ((Event.current.type == EventType.KeyUp) && (Event.current.keyCode == KeyCode.Return)) chat.SendMessage();
		GUILayout.EndArea();
	}
}