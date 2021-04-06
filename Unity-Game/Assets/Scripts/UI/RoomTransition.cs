using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomTransition : MonoBehaviour {

    public TMP_Text roomText;
    public Animator transition;

    public void Transition(string text) {
        this.roomText.text = text;
        transition.SetTrigger("Start");
    }
}
