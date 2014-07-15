using UnityEngine;
using System.Collections;

public interface IActor 
{
	string ID { get; set; }
	int HP { get; set; }
    void TakeDamage(int dmg, string text);

    int MaxHP { get; set; }
}