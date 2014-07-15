using UnityEngine;
using System.Collections.Generic;

public class ChatGUI : MonoBehaviour
{
	public static string ChatInput = "";
	public static List<string> MessagesTab1 = new List<string>();
	public static List<string> MessagesTab2 = new List<string>();
	public static List<string> MessagesTab3 = new List<string>();

	private int selectedTab = 1;
	private int chatHeight = 250;

	private Vector2 chatScroll1 = Vector2.zero;
	private Vector2 chatScroll2 = Vector2.zero;
	private Vector2 chatScroll3 = Vector2.zero;

	public static Chat chat;

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
		GUI.Box(new Rect(10, Screen.height - chatHeight - 10, 500, chatHeight), string.Empty);
		GUILayout.BeginArea(new Rect(10, Screen.height - chatHeight - 10, 500, chatHeight));
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Tab 1")) selectedTab = 1;
		if (GUILayout.Button("Tab 2")) selectedTab = 2;
		if (GUILayout.Button("Tab 3")) selectedTab = 3;
		GUILayout.EndHorizontal();
		switch (selectedTab)
		{
			case 1:
				chatScroll1 = GUILayout.BeginScrollView(chatScroll1);
				for (int i = MessagesTab1.Count - 1; i >= 0; i--) GUILayout.Label(MessagesTab1[i]);
				GUILayout.EndScrollView();
				break;
			case 2:
				chatScroll2 = GUILayout.BeginScrollView(chatScroll2);
				for (int i = MessagesTab2.Count - 1; i >= 0; i--) GUILayout.Label(MessagesTab2[i]);
				GUILayout.EndScrollView();
				break;
			case 3:
				chatScroll3 = GUILayout.BeginScrollView(chatScroll3);
				for (int i = MessagesTab3.Count - 1; i >= 0; i--) GUILayout.Label(MessagesTab3[i]);
				GUILayout.EndScrollView();
				break;
		}
		ChatInput = GUILayout.TextField(ChatInput);
		if ((Event.current.type == EventType.KeyUp) && (Event.current.keyCode == KeyCode.Return)) chat.SendMessage(selectedTab);
		GUILayout.EndArea();
	}
}