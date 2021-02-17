using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsHub : MonoBehaviour
{
    public GameObject longSwordSprite;
    public GameObject bastardSwordSprite;
    public GameObject woodShieldSprite;
    public GameObject goldShieldSprite;
    public GameObject healthPotionSprite;
    public GameObject bonusPotionSprite;

    public TMP_Text itemCollected;

    private GameObject actualSwordSprite;
    private GameObject actualShieldSprite;
    private GameObject actualPotion;

    void Start() {
        SetLongSword();
        SetWoodShield();
    }

    public void CollectedItem(string name) {

        itemCollected.gameObject.SetActive(true);
        itemCollected.text = name + " collected!";

        StartCoroutine(Wait(4f));
    }

    public void SetBastardSword() {
        Destroy(actualSwordSprite);
        actualSwordSprite = Instantiate(bastardSwordSprite);
        actualSwordSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        pos.x -= 250;
        
        actualSwordSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void SetLongSword() {
        Destroy(actualSwordSprite);
        actualSwordSprite = Instantiate(longSwordSprite);
        actualSwordSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        pos.x -= 250;
        
        actualSwordSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void SetWoodShield() {
        Destroy(actualShieldSprite);
        actualShieldSprite = Instantiate(woodShieldSprite);
        actualShieldSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        
        actualShieldSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void SetGoldShield() {
        Destroy(actualShieldSprite);
        actualShieldSprite = Instantiate(goldShieldSprite);
        actualShieldSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        
        actualShieldSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void SetHealthPotion() {
        Destroy(actualPotion);
        actualPotion = Instantiate(healthPotionSprite);
        actualPotion.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        pos.x -= 500;
        
        actualPotion.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void SetBonusPotion() {
        Destroy(actualPotion);
        actualPotion = Instantiate(bonusPotionSprite);
        actualPotion.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        pos.x -= 500;
        
        actualPotion.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public void DestroyActualPotion() {
        Destroy(actualPotion);
    }

    private IEnumerator Wait(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        itemCollected.gameObject.SetActive(false);
    }
}
