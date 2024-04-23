using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Text text;
    public bool aPress = false;
    public bool dPress = false;
    public bool sPress = false;
    public bool spacePress = false;
    public bool shift1Press = false;
    public bool shift2Press = false;
    public bool e1Press = false;
    public bool e2Press = false;
    public bool lClick = false;
    public bool rClick = false;
    public bool lrClick = false;
    public bool fPress = false;
    public bool q1Press = false;
    public bool q2Press = false;

    public Image logo;

    public AudioSource boomAudio;
    public AudioSource gameboyAudio;

    // Start is called before the first frame update
    void Start()
    {
        logo.enabled = false;
        StartCoroutine(ShowTutorial());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            aPress = true;
            dPress = true;
            sPress = true;
            spacePress = true;
            shift1Press = true;
            shift2Press = true;
            e1Press = true;
            e2Press = true;
            lClick = true;
            rClick = true;
            lrClick = true;
            fPress = true;
            q1Press = true;
            q2Press = true;
        }
    }

    IEnumerator ShowTutorial()
    {
        text.text = "(Press T to skip Tutorial)";
            yield return new WaitForSeconds(2f);

        text.text = "Press 'A' and 'D' to move Left and Right";
        while (!aPress || !dPress)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                aPress = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                dPress = true;
            }
            yield return null;
        }

        text.text = "Press 'S' to Crouch";
        while (!sPress)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                sPress = true;
            }
            yield return null;
        }

        text.text = "Press 'Space' to Jump";
        while (!spacePress)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spacePress = true;
            }
            yield return null;
        }

        text.text = "Press 'Shift' next to The Baby to pick her up";
        while (!shift1Press)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                shift1Press = true;
            }
            yield return null;
        }

        text.text = "Press 'Shift' again to drop The Baby";
        while (!shift2Press)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                shift2Press = true;
            }
            yield return null;
        }

        text.text = "You and The Baby both have needs";
        if (!e1Press)
            yield return new WaitForSeconds(5f);

        text.text = "If either of your HEALTH becomes empty, you lose";
        if (!e1Press)
            yield return new WaitForSeconds(5f);

        text.text = "If either of your HUNGER becomes empty, you or her will start to lose HEALTH";
        if (!e1Press)
            yield return new WaitForSeconds(5f);

        text.text = "You can replenish your HUNGER by eating food from the Inventory";
        if (!e1Press)
            yield return new WaitForSeconds(5f);

        text.text = "Press 'E' to open your Inventory";
        while (!e1Press)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                e1Press = true;
            }
            yield return null;
        }

        text.text = "Press 'E' again to close your Inventory";
        while (!e2Press)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                e2Press = true;
            }
            yield return null;
        }

        text.text = "If The Baby's ENERGY reaches 0, she will start to lose HEALTH";
        if (!lClick)
            yield return new WaitForSeconds(5f);

        text.text = "You can replenish her ENERGY by leaving her in a bed at camp";
        if (!lClick)
            yield return new WaitForSeconds(5f);

        text.text = "If The Baby's HAPPY reaches 0, she will start to lose HEALTH";
        if (!lClick)
            yield return new WaitForSeconds(5f);

        text.text = "You can replenish her HAPPY by carrying with you";
        if (!lClick)
            yield return new WaitForSeconds(5f);

        text.text = "Press 'Left Click' to use your drill";
        while (!lClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lClick = true;
            }
            yield return null;
        }

        text.text = "Hold 'Right Click' to switch to your gun";
        while (!rClick)
        {
            if (Input.GetMouseButtonDown(1))
            {
                rClick = true;
            }
            yield return null;
        }

        text.text = "Press 'Left Click' while holding 'Right Click' to fire your gun";
        while (!lrClick)
        {
            if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
            {
                lrClick = true;
            }
            yield return null;
        }

        text.text = "Press 'F' to swap between ammo types";
        while (!fPress)
        {
            if ((Input.GetKeyDown(KeyCode.F)))
            {
                fPress = true;
            }
            yield return null;
        }

        text.text = "You can also choose ammo manually by using the 1, 2, 3, or 4 buttons";
        if (!q1Press)
            yield return new WaitForSeconds(5f);

        text.text = "Press 'Q' when near camp to Craft";
        while (!q1Press)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                q1Press = true;
            }
            yield return null;
        }

        text.text = "Press 'Q' again to close your Inventory";
        while (!q2Press)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                q2Press = true;
            }
            yield return null;
        }

        text.text = "";
        logo.enabled = true;
        boomAudio.PlayOneShot(boomAudio.clip);
        yield return new WaitForSeconds(5f);

        logo.enabled = false;
        yield return new WaitForSeconds(1f);

        text.text = "by Jakob Legere";
        gameboyAudio.PlayOneShot(gameboyAudio.clip);
        yield return new WaitForSeconds(5f);

        text.text = "";
    }
}