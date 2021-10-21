using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{

    TextMeshProUGUI healthText;
    PlayerShip player;

    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<PlayerShip>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = player.GetHealth().ToString();
    }
}
