using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSwap : MonoBehaviour
{
    public int bulletType;
    public Image bulletIcon;
    public Text bulletQuantity;

    public Sprite steelBulletSprite;
    public Sprite plasticBulletSprite;
    public Sprite bouncyBulletSprite;
    public Sprite radBulletSprite;

    public AudioSource ammoSwapSound;

    // Start is called before the first frame update
    void Start()
    {
        bulletType = 1;
        ChangeBulletUI();
    }

    void Update()
    {
        if (!GetComponent<PlayerMovement>().isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ammoSwapSound.PlayOneShot(ammoSwapSound.clip);
                bulletType = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ammoSwapSound.PlayOneShot(ammoSwapSound.clip);
                bulletType = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ammoSwapSound.PlayOneShot(ammoSwapSound.clip);
                bulletType = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ammoSwapSound.PlayOneShot(ammoSwapSound.clip);
                bulletType = 4;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                bulletType++;
                if (bulletType > 4)
                {
                    bulletType = 1;
                }
                ammoSwapSound.PlayOneShot(ammoSwapSound.clip);
            }
            ChangeBulletUI();
        }
    }

    public void ChangeBulletUI()
    {
        // Update bullet icon and quantity text based on the current bullet type
        switch (bulletType)
        {
            case 1:
                bulletIcon.sprite = steelBulletSprite;
                bulletQuantity.text = GetComponent<Inventory>().steelbulletCount + "x";
                break;
            case 2:
                bulletIcon.sprite = plasticBulletSprite;
                bulletQuantity.text = GetComponent<Inventory>().plasticbulletCount + "x";
                break;
            case 3:
                bulletIcon.sprite = bouncyBulletSprite;
                bulletQuantity.text = GetComponent<Inventory>().bouncybulletCount + "x";
                break;
            case 4:
                bulletIcon.sprite = radBulletSprite;
                bulletQuantity.text = GetComponent<Inventory>().radbulletCount + "x";
                break;
            default:
                break;
        }
    }
}
