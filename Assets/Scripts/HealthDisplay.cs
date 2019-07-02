using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI health;
    Player player;

	void Start ()
    {
        health = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
        health.text = player.GetHealth().ToString();
	}
	
	void Update ()
    {
        health.text = player.GetHealth().ToString();	
	}
}
