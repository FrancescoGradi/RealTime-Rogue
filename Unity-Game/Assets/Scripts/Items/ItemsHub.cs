using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsHub : MonoBehaviour
{
    public GameObject longSwordSprite;
    public GameObject bastardSwordSprite;
    public GameObject woodShieldSprite;
    public GameObject goldenShieldSprite;
    public GameObject healthPotionSprite;
    public GameObject bonusPotionSprite;
    public GameObject emptySprite;

    public TMP_Text itemCollected;
    public GameObject itemCanvas;

    private GameObject actualSwordSprite;
    private GameObject actualShieldSprite;
    private GameObject actualPotionSprite;

    void Start() {

        actualShieldSprite = Instantiate(emptySprite);
        actualShieldSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        
        actualShieldSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        actualSwordSprite = Instantiate(emptySprite);
        actualSwordSprite.transform.SetParent(this.gameObject.transform);

        pos = this.gameObject.transform.position;
        pos.x -= 175;
        
        actualSwordSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        actualPotionSprite = Instantiate(emptySprite);
        actualPotionSprite.transform.SetParent(this.gameObject.transform);

        pos = this.gameObject.transform.position;
        pos.x -= 350;
        
        actualPotionSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        SetSword("Long Sword");
        SetShield("Wood Shield");
    }

    public void CollectedItem(string name) {

        itemCanvas.gameObject.SetActive(true);

        itemCollected.text = name + " collected!";

        StartCoroutine(Wait(3f));
    }

    public void SetSword(string name) {

        Vector3 posSprite = actualSwordSprite.transform.position;
        Destroy(actualSwordSprite);

        if (name == "Long Sword") {
            actualSwordSprite = Instantiate(longSwordSprite, posSprite, Quaternion.identity);
        } else if (name == "Bastard Sword") {
            actualSwordSprite = Instantiate(bastardSwordSprite, posSprite, Quaternion.identity);
        } else {
            actualSwordSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        }

        actualSwordSprite.transform.SetParent(this.gameObject.transform);
    }

    public void SetShield(string name) {

        Vector3 posSprite = actualShieldSprite.transform.position;
        Destroy(actualShieldSprite);

        if (name == "Wood Shield") {
            actualShieldSprite = Instantiate(woodShieldSprite, posSprite, Quaternion.identity);
        } else if (name == "Golden Shield") {
            actualShieldSprite = Instantiate(goldenShieldSprite, posSprite, Quaternion.identity);
        } else {
            actualShieldSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        }

        actualShieldSprite.transform.SetParent(this.gameObject.transform);
    }

    public void SetPotion(string name) {

        Vector3 posSprite = actualPotionSprite.transform.position;
        Destroy(actualPotionSprite);

        if (name == "Health Potion") {
            actualPotionSprite = Instantiate(healthPotionSprite, posSprite, Quaternion.identity);
        } else if (name == "Bonus Potion") {
            actualPotionSprite = Instantiate(bonusPotionSprite, posSprite, Quaternion.identity);
        } else {
            actualPotionSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        }

        actualPotionSprite.transform.SetParent(this.gameObject.transform);
    }

    public void DestroyActualPotion() {
        
        Vector3 posSprite = actualPotionSprite.transform.position;
        Destroy(actualPotionSprite);

        actualPotionSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        actualPotionSprite.transform.SetParent(this.gameObject.transform);
    }

    private IEnumerator Wait(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        itemCanvas.gameObject.SetActive(false);
    }
}
