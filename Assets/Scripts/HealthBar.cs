using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public PlayerStatus playerStatus;
    public Text nameText;
    public Image barFill;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (nameText != null && playerStatus.strixReplicator.roomMember != null) {
            nameText.text = playerStatus.strixReplicator.roomMember.GetName();
        }

        float rate = playerStatus.health / (float)playerStatus.maxHealth;
        barFill.transform.localScale = new Vector3(rate, 1, 1);

        transform.forward = Camera.main.transform.forward;
    }
}
