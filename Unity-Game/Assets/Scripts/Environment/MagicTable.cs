using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTable : MonoBehaviour {

    #region Singleton

    public static MagicTable instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one MagiTable instance found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject magicTableText;
    public LayerMask playerLayer;
    public GameObject statModMenu;
    public int statPoints;

    private float magicTableRange = 2.5f;
    private bool inMenu = false;
    private bool firstTime = true;
    private GameObject statModMenuInstance;

    private void Start() {
        magicTableText.SetActive(false);
    }

    private void Update() {

        if (!inMenu && Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < magicTableRange) {
            magicTableText.SetActive(true);
            if (Input.GetButton("BaseAction")) {

                magicTableText.SetActive(false);

                if (firstTime) {
                    statModMenuInstance = Instantiate(statModMenu, FindObjectOfType<Canvas>().transform);
                    firstTime = false;
                } else {
                    statModMenuInstance.SetActive(true);
                    statModMenuInstance.GetComponent<StatModMenu>().Pause();
                }
                inMenu = true;
            }
        } else {
            magicTableText.SetActive(false);
        }
        
    }

    public void SetInMenu(bool value) {
        this.inMenu = value;
    }

}
