using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StatModMenu : MonoBehaviour {

    #region Singleton

    public static StatModMenu instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one StatModMenu instance found!");
            return;
        }
        instance = this;
    }

    #endregion
    
    public GameObject firstButton;
    public TMP_Text pointsText;

    public StatMod atkStatMod;
    public StatMod manaStatMod;
    public StatMod defStatMod;
    public StatMod speedStatMod;

    private int availablePoints;
    private int maxPoints = 4;
    private int updateRate = 15;
    private int count = 0;
    private static bool gamePaused = true;

    private void Start() {
        Pause();
        availablePoints = maxPoints;
        pointsText.text = availablePoints.ToString();

        atkStatMod.SetBaseValue(Player.instance.ATK);
        manaStatMod.SetBaseValue(Player.instance.MANA);
        defStatMod.SetBaseValue(Player.instance.DEF);
        speedStatMod.SetBaseValue((int) Player.instance.speed);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause") || Input.GetButton("Fire3")) {
            if (gamePaused) {
                Resume();
            } else {
                Pause();
            }
        }

        count += 1;

        if (count >= updateRate) {
            if (Input.GetAxis("Horizontal") > 0.5f) {
                SubtractPoint(GetSelectedStatMod());
                count = 0;
            } else if (Input.GetAxis("Horizontal") < -0.5f) {
                AddPoint(GetSelectedStatMod());
                count = 0;
            }
        }
    }

    private StatMod GetSelectedStatMod() {
        if (EventSystem.current.currentSelectedGameObject == atkStatMod.button) {
            return atkStatMod;
        } else if (EventSystem.current.currentSelectedGameObject == manaStatMod.button) {
            return manaStatMod;
        } else if (EventSystem.current.currentSelectedGameObject == defStatMod.button) {
            return defStatMod;
        } else if (EventSystem.current.currentSelectedGameObject == speedStatMod.button) {
            return speedStatMod;
        } else {
            return null;
        }
    }

    private void SubtractPoint(StatMod selectedStat) {
        if (availablePoints > 0) {

            availablePoints -= 1;
            pointsText.text = availablePoints.ToString();

            selectedStat.AddOnePoint();

            if (availablePoints == 0) {
                DisableAllRightArrows();
            }

            UpdatePlayerStats();
        }
    }

    private void AddPoint(StatMod selectedStat) {
        if (availablePoints < maxPoints && (selectedStat.actualValue > selectedStat.baseValue)) {

            availablePoints += 1;
            pointsText.text = availablePoints.ToString();
            
            selectedStat.SubtractOnePoint();

            EnableAllRightArrows();

            UpdatePlayerStats();
        }
    }

    private void DisableAllRightArrows() {
        atkStatMod.rightArrow.SetActive(false);
        manaStatMod.rightArrow.SetActive(false);
        defStatMod.rightArrow.SetActive(false);
        speedStatMod.rightArrow.SetActive(false);
    }

    private void EnableAllRightArrows() {
        atkStatMod.rightArrow.SetActive(true);
        manaStatMod.rightArrow.SetActive(true);
        defStatMod.rightArrow.SetActive(true);
        speedStatMod.rightArrow.SetActive(true);
    }

    private void UpdatePlayerStats() {
        Player.instance.ATK = atkStatMod.actualValue;
        Player.instance.MANA = manaStatMod.actualValue;
        Player.instance.DEF = defStatMod.actualValue;
        Player.instance.speed = (float) speedStatMod.actualValue;
    }

    public void Pause() {

        SetFirstButton();

        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Resume() {
        StartCoroutine(ResumeWaiter(0.2f));
    }

    private void SetFirstButton() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private IEnumerator ResumeWaiter(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);

        Time.timeScale = 1f;
        gamePaused = false;

        this.gameObject.SetActive(false);
        MagicTable.instance.SetInMenu(false);
    }
}
