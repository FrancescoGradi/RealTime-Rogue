using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsHub : MonoBehaviour {
    public GameObject longSwordSprite;
    public GameObject bastardSwordSprite;
    public GameObject woodShieldSprite;
    public GameObject goldenShieldSprite;
    public GameObject healthPotionSprite;
    public GameObject bonusPotionSprite;
    public GameObject fireBallSprite;
    public GameObject emptySprite;

    public TMP_Text itemCollected;
    public GameObject itemCanvas;

    private GameObject actualSwordSprite;
    private GameObject actualShieldSprite;
    private GameObject actualPotionSprite;
    private GameObject actualfireBallSprite;

    void Start() {

        actualShieldSprite = Instantiate(emptySprite);
        actualShieldSprite.transform.SetParent(this.gameObject.transform);

        Vector3 pos = this.gameObject.transform.position;
        
        actualShieldSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        actualfireBallSprite = Instantiate(emptySprite);
        actualfireBallSprite.transform.SetParent(this.gameObject.transform);

        pos = this.gameObject.transform.position;
        pos.x -= 175;
        
        actualfireBallSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        actualSwordSprite = Instantiate(emptySprite);
        actualSwordSprite.transform.SetParent(this.gameObject.transform);

        pos = this.gameObject.transform.position;
        pos.x -= 350;
        
        actualSwordSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        actualPotionSprite = Instantiate(emptySprite);
        actualPotionSprite.transform.SetParent(this.gameObject.transform);

        pos = this.gameObject.transform.position;
        pos.x -= 525;
        
        actualPotionSprite.transform.SetPositionAndRotation(pos, Quaternion.identity);


        Vector3 posSprite = actualSwordSprite.transform.position;
        Destroy(actualSwordSprite);

        actualSwordSprite = Instantiate(longSwordSprite, posSprite, Quaternion.identity);
        longSwordSprite.GetComponent<Sprite>().SetSpriteValue(Player.instance.ATK + 6, "DMG");

        actualSwordSprite.transform.SetParent(this.gameObject.transform);


        posSprite = actualfireBallSprite.transform.position;
        Destroy(actualfireBallSprite);

        fireBallSprite.GetComponent<Sprite>().SetSpriteValue(Player.instance.MANA + 5, "MAG");
        actualfireBallSprite = Instantiate(fireBallSprite, posSprite, Quaternion.identity);

        actualfireBallSprite.transform.SetParent(this.gameObject.transform);


        posSprite = actualShieldSprite.transform.position;
        Destroy(actualShieldSprite);

        woodShieldSprite.GetComponent<Sprite>().SetSpriteValue(Player.instance.DEF + 2, "AC");
        actualShieldSprite = Instantiate(woodShieldSprite, posSprite, Quaternion.identity);

        actualShieldSprite.transform.SetParent(this.gameObject.transform);
    }

    public void CollectedItem(Item item) {

        itemCanvas.gameObject.SetActive(true);

        itemCollected.text = item.name + " collected!";

        StartCoroutine(Wait(3f));
    }

    public void DrinkPotionCanvasFeedback(string name) {

        itemCanvas.gameObject.SetActive(true);
        itemCollected.text = name;

        StartCoroutine(Wait(2f));
    }

    public void SetSword(Item item) {

        Vector3 posSprite = actualSwordSprite.transform.position;
        Destroy(actualSwordSprite);

        if (item.name == "Long Sword") {
            longSwordSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusATK + Player.instance.ATK, "DMG");
            actualSwordSprite = Instantiate(longSwordSprite, posSprite, Quaternion.identity);
        } else if (item.GetComponent<BastardSword>() != null) {
            bastardSwordSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusATK + + Player.instance.ATK, "DMG");
            actualSwordSprite = Instantiate(bastardSwordSprite, posSprite, Quaternion.identity);
        } else {
            actualSwordSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        }

        actualSwordSprite.transform.SetParent(this.gameObject.transform);
    }

    public void SetShield(Item item) {

        Vector3 posSprite = actualShieldSprite.transform.position;
        Destroy(actualShieldSprite);

        if (item.name == "Wood Shield") {
            actualShieldSprite = Instantiate(woodShieldSprite, posSprite, Quaternion.identity);
            woodShieldSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusDEF + Player.instance.DEF, "AC");
        } else if (item.GetComponent<GoldenShield>() != null) {
            goldenShieldSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusDEF + Player.instance.DEF, "AC");
            actualShieldSprite = Instantiate(goldenShieldSprite, posSprite, Quaternion.identity);
        } else {
            actualShieldSprite = Instantiate(emptySprite, posSprite, Quaternion.identity);
        }

        actualShieldSprite.transform.SetParent(this.gameObject.transform);
    }

    public void SetPotion(Item item) {

        Vector3 posSprite = actualPotionSprite.transform.position;
        Destroy(actualPotionSprite);

        if (item.GetComponent<HealthPotion>() != null) {
            healthPotionSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusHP, "HP");
            actualPotionSprite = Instantiate(healthPotionSprite, posSprite, Quaternion.identity);
        } else if (item.GetComponent<BonusPotion>() != null) {
            bonusPotionSprite.GetComponent<Sprite>().SetSpriteValue(item.bonusATK, "ALL");
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
