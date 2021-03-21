using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public Player player;
    public GameObject stats;
    public TMP_Text ATK;
    public TMP_Text MANA;
    public TMP_Text DEF;
    public TMP_Text SPEED;

    void Update() {

        if (Input.GetButton("Select")) {
            stats.gameObject.SetActive(true);
            
            ATK.text = player.ATK.ToString();
            MANA.text = player.MANA.ToString();
            DEF.text = player.DEF.ToString();
            SPEED.text = player.speed.ToString();
        } else {
            stats.gameObject.SetActive(false);
        }
    }
}
