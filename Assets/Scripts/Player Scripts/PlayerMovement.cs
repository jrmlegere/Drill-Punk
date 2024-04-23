using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        WALKING,
        CROUCH,
        CROUCHWALK,
        RAD_IDLE,
        RAD_WALKING,
        RAD_CROUCH,
        RAD_CROUCHWALK,
        DEAD,
        RAD_DEAD
    }

    PlayerStates CurrentState
    {
        set
        {
            currentState = value;

            switch(currentState)
            {
                case PlayerStates.IDLE:
                    animator.Play("Idle");
                    break;
                case PlayerStates.WALKING:
                    animator.Play("Walking");
                    break;
                case PlayerStates.CROUCH:
                    animator.Play("Crouch");
                    break;
                case PlayerStates.CROUCHWALK:
                    animator.Play("CrouchWalk");
                    break;
                case PlayerStates.RAD_IDLE:
                    animator.Play("Rad Idle");
                    break;
                case PlayerStates.RAD_WALKING:
                    animator.Play("Rad Walking");
                    break;
                case PlayerStates.RAD_CROUCH:
                    animator.Play("Rad Crouch");
                    break;
                case PlayerStates.RAD_CROUCHWALK:
                    animator.Play("Rad CrouchWalk");
                    break;
                case PlayerStates.DEAD:
                    animator.Play("Dead");
                    break;
                case PlayerStates.RAD_DEAD:
                    animator.Play("Rad Dead");
                    break;
            }
        }
    }

    public GameObject logic;
    public GameObject drill;
    public float horizontal;
    public float speed = 8f;
    public float jumpingPower = 18.5f;
    public bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask rockLayer;

    public Inventory inventory;
    public bool inventoryShowing = false;

    public Crafting crafting;
    public bool craftingShowing = false;

    public GameObject quest0;
    public bool quest0Showing = false;
    public GameObject quest1;
    public bool quest1Showing = false;
    public GameObject quest2;
    public bool quest2Showing = false;
    public GameObject quest3;
    public bool quest3Showing = false;
    public GameObject quest4;
    public bool quest4Showing = false;

    public LayerMask questLayer;
    public bool isAtQuest = false;
    //public Crafting crafting;
    public bool questShowing = false;

    public LayerMask dropLayer;
    public LayerMask campLayer;
    public bool isAtCamp = false;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public PlayerStates currentState;
    public bool isCrouching = false;
    public bool radSuit = false;
    public bool isPaused = false;

    public GameObject pauseScreen;

    public AudioSource crouchAudio;
    public AudioSource jumpAudio;
    public AudioSource itemAudio;
    public AudioSource suitAudio;

    public AudioSource mole1Audio;
    public AudioSource mole2Audio;
    public AudioSource mole3Audio;

    public int questNum = 0;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        crafting = GetComponent<Crafting>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector2(0f, -25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerHealth>().dead && !isPaused)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.S))
            {
                crouchAudio.PlayOneShot(crouchAudio.clip);
            }

            if (Input.GetKey(KeyCode.S))
            {
                isCrouching = true;
                if (radSuit)
                {
                    speed = 2f;
                }
                else
                {
                    speed = 4f;
                }
                GetComponent<BoxCollider2D>().size = new Vector2(2.8f, 4.7f);
            }
            else
            {
                isCrouching = false;
                if (radSuit)
                {
                    speed = 4f;
                }
                else
                {
                    speed = 8f;
                }
                GetComponent<BoxCollider2D>().size = new Vector2(2.8f, 5.7f);
            }

            if (!drill.GetComponent<DrillDestroy>().drillOn || !drill.GetComponent<DrillDestroy>().touchingDirt)
            {
                horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown("Jump") && IsGrounded())
                {
                    jumpAudio.PlayOneShot(jumpAudio.clip);
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                }

                if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }

                Flip();

                if (radSuit)
                {
                    if (horizontal == 0)
                    {
                        if (isCrouching)
                            CurrentState = PlayerStates.RAD_CROUCH;
                        else
                            CurrentState = PlayerStates.RAD_IDLE;
                    }
                    else
                    {
                        if (isCrouching)
                            CurrentState = PlayerStates.RAD_CROUCHWALK;
                        else
                            CurrentState = PlayerStates.RAD_WALKING;
                    }
                }
                else
                {
                    if (horizontal == 0)
                    {
                        if (isCrouching)
                            CurrentState = PlayerStates.CROUCH;
                        else
                            CurrentState = PlayerStates.IDLE;
                    }
                    else
                    {
                        if (isCrouching)
                            CurrentState = PlayerStates.CROUCHWALK;
                        else
                            CurrentState = PlayerStates.WALKING;
                    }
                }
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float directionX = (mousePosition.x - transform.position.x);
                float directionY = (mousePosition.y - transform.position.y);
                Vector2 direction = new Vector2(directionX, directionY).normalized;
                rb.velocity = direction * speed;
                if (radSuit)
                    CurrentState = PlayerStates.RAD_IDLE;
                else
                    CurrentState = PlayerStates.IDLE;
            }

            ///////////////////////////////////////////////////////// Inventory
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (craftingShowing)
                {
                    craftingShowing = !craftingShowing;
                    crafting.craftingUI.SetActive(craftingShowing);
                }
                if (quest0Showing)
                {
                    quest0Showing = !quest0Showing;
                    quest0.SetActive(quest0Showing);
                }
                if (quest1Showing)
                {
                    quest1Showing = !quest1Showing;
                    quest1.SetActive(quest1Showing);
                }
                if (quest2Showing)
                {
                    quest2Showing = !quest2Showing;
                    quest2.SetActive(quest2Showing);
                }
                if (quest3Showing)
                {
                    quest3Showing = !quest3Showing;
                    quest3.SetActive(quest3Showing);
                }
                if (quest4Showing)
                {
                    quest4Showing = !quest4Showing;
                    quest4.SetActive(quest4Showing);
                }
                inventoryShowing = !inventoryShowing;
                inventory.inventoryUI.SetActive(inventoryShowing);
            }
            /////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////// Crafting
            Collider2D[] touchingCampCheck = Physics2D.OverlapCircleAll(transform.position, 2f, campLayer);

            if (touchingCampCheck.Length != 0)
            {
                isAtCamp = true;
            }
            else
            {
                isAtCamp = false;
            }

            if (Input.GetKeyDown(KeyCode.Q) && isAtCamp)
            {
                if (inventoryShowing)
                {
                    inventoryShowing = !inventoryShowing;
                    inventory.inventoryUI.SetActive(inventoryShowing);
                }
                if (quest0Showing)
                {
                    quest0Showing = !quest0Showing;
                    quest0.SetActive(quest0Showing);
                }
                if (quest1Showing)
                {
                    quest1Showing = !quest1Showing;
                    quest1.SetActive(quest1Showing);
                }
                if (quest2Showing)
                {
                    quest2Showing = !quest2Showing;
                    quest2.SetActive(quest2Showing);
                }
                if (quest3Showing)
                {
                    quest3Showing = !quest3Showing;
                    quest3.SetActive(quest3Showing);
                }
                if (quest4Showing)
                {
                    quest4Showing = !quest4Showing;
                    quest4.SetActive(quest4Showing);
                }
                craftingShowing = !craftingShowing;
                crafting.craftingUI.SetActive(craftingShowing);
            }
            /////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////Quest
            Collider2D[] touchingQuestCheck = Physics2D.OverlapCircleAll(transform.position, 1f, questLayer);

            if (touchingQuestCheck.Length != 0)
            {
                questNum = touchingQuestCheck[0].GetComponent<Quest>().questNum;
                isAtQuest = true;
            }
            else
            {
                questNum = -1;
                isAtQuest = false;
            }

            if (Input.GetKeyDown(KeyCode.Q) && isAtQuest)
            {
                int r = Random.Range(1, 4);
                if (r == 1)
                {
                    mole1Audio.PlayOneShot(mole1Audio.clip);
                }
                else if (r == 2)
                {
                    mole2Audio.PlayOneShot(mole2Audio.clip);
                }
                else if (r == 3)
                {
                    mole3Audio.PlayOneShot(mole3Audio.clip);
                }

                if (inventoryShowing)
                {
                    inventoryShowing = !inventoryShowing;
                    inventory.inventoryUI.SetActive(inventoryShowing);
                }
                if (craftingShowing)
                {
                    craftingShowing = !craftingShowing;
                    crafting.craftingUI.SetActive(craftingShowing);
                }

                if (quest0Showing || ((!quest0Showing) && (questNum == 0)))
                {
                    quest0Showing = !quest0Showing;
                    quest0.SetActive(quest0Showing);
                }
                if (quest1Showing || ((!quest1Showing) && (questNum == 1)))
                {
                    quest1Showing = !quest1Showing;
                    quest1.SetActive(quest1Showing);
                }
                if (quest2Showing || ((!quest2Showing) && (questNum == 2)))
                {
                    quest2Showing = !quest2Showing;
                    quest2.SetActive(quest2Showing);
                }
                if (quest3Showing || ((!quest3Showing) && (questNum == 3)))
                {
                    quest3Showing = !quest3Showing;
                    quest3.SetActive(quest3Showing);
                }
                if (quest4Showing || ((!quest4Showing) && (questNum == 4)))
                {
                    quest4Showing = !quest4Showing;
                    quest4.SetActive(quest4Showing);
                }
            }
            /////////////////////////////////////////////////////////

            if (Input.GetKeyDown(KeyCode.Q) && !isAtCamp && !isAtQuest)
            {
                if (inventoryShowing)
                {
                    inventoryShowing = !inventoryShowing;
                    inventory.inventoryUI.SetActive(inventoryShowing);
                }
                if (craftingShowing)
                {
                    craftingShowing = !craftingShowing;
                    crafting.craftingUI.SetActive(craftingShowing);
                }
                if (quest0Showing)
                {
                    quest0Showing = !quest0Showing;
                    quest0.SetActive(quest0Showing);
                }
                if (quest1Showing)
                {
                    quest1Showing = !quest1Showing;
                    quest1.SetActive(quest1Showing);
                }
                if (quest2Showing)
                {
                    quest2Showing = !quest2Showing;
                    quest2.SetActive(quest2Showing);
                }
                if (quest3Showing)
                {
                    quest3Showing = !quest3Showing;
                    quest3.SetActive(quest3Showing);
                }
                if (quest4Showing)
                {
                    quest4Showing = !quest4Showing;
                    quest4.SetActive(quest4Showing);
                }
            }

            if (inventory.radSuitUnlocked)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    suitAudio.PlayOneShot(suitAudio.clip);
                    radSuit = !radSuit;
                    if (radSuit)
                        speed = speed / 2f;
                    else
                        speed = speed * 2f;
                }
            }
        }
        else if (GetComponent<PlayerHealth>().dead)
        {
            GetComponent<BoxCollider2D>().size = new Vector2(2.8f, 2f);
            if (radSuit)
            {
                CurrentState = PlayerStates.RAD_DEAD;
            }
            else
            {
                CurrentState = PlayerStates.DEAD;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !GetComponent<PlayerHealth>().dead)
        {
            if (craftingShowing)
            {
                craftingShowing = !craftingShowing;
                crafting.craftingUI.SetActive(craftingShowing);
            }
            else if (inventoryShowing)
            {
                inventoryShowing = !inventoryShowing;
                inventory.inventoryUI.SetActive(inventoryShowing);
            }
            else if (quest0Showing)
            {
                quest0Showing = !quest0Showing;
                quest0.SetActive(quest0Showing);
            }
            else if (quest1Showing)
            {
                quest1Showing = !quest1Showing;
                quest1.SetActive(quest1Showing);
            }
            else if (quest2Showing)
            {
                quest2Showing = !quest2Showing;
                quest2.SetActive(quest2Showing);
            }
            else if (quest3Showing)
            {
                quest3Showing = !quest3Showing;
                quest3.SetActive(quest3Showing);
            }
            else if (quest4Showing)
            {
                quest4Showing = !quest4Showing;
                quest4.SetActive(quest4Showing);
            }
            else
            {
                TogglePause();
            }
        }

        if (GetComponent<PlayerHealth>().dead || isPaused)
        {
            inventoryShowing = false;
            inventory.inventoryUI.SetActive(inventoryShowing);
            craftingShowing = false;
            crafting.craftingUI.SetActive(craftingShowing);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause the game
            Time.timeScale = 0f;
        }
        else
        {
            // Resume the game
            Time.timeScale = 1f;
        }

        pauseScreen.SetActive(isPaused);
    }

    private void FixedUpdate()
    {
        if (!GetComponent<PlayerHealth>().dead && !isPaused)
        {
            if (!drill.GetComponent<DrillDestroy>().drillOn || !drill.GetComponent<DrillDestroy>().touchingDirt)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(1.1f, 0.1f);
        Vector2 boxCenter = new Vector2(groundCheck.position.x, groundCheck.position.y - boxSize.y / 2f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer | rockLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GetComponent<PlayerHealth>().dead && !isPaused)
        {
            if ((dropLayer.value & 1 << collision.gameObject.layer) != 0)
            {
                itemAudio.PlayOneShot(itemAudio.clip);
                if (collision.gameObject.name == "Coal(Clone)")
                {
                    if (inventory.coalCount < 999)
                        inventory.coalCount++;
                }
                else if (collision.gameObject.name == "Mushroom(Clone)")
                {
                    if (inventory.mushroomCount < 999)
                        inventory.mushroomCount++;
                }
                else if (collision.gameObject.name == "Plastic(Clone)")
                {
                    if (inventory.plasticCount < 999)
                        inventory.plasticCount++;
                }
                else if (collision.gameObject.name == "Steel(Clone)")
                {
                    if (inventory.steelCount < 999)
                        inventory.steelCount++;
                }
                else if (collision.gameObject.name == "Wood(Clone)")
                {
                    if (inventory.woodCount < 999)
                        inventory.woodCount++;
                }
                else if (collision.gameObject.name == "Trinitite(Clone)")
                {
                    if (inventory.trinititeCount < 999)
                        inventory.trinititeCount++;
                }
                else if (collision.gameObject.name == "Waddlepus Steak(Clone)")
                {
                    if (inventory.steakCount < 999)
                        inventory.steakCount++;
                }
                else if (collision.gameObject.name == "Womster Dog(Clone)")
                {
                    if (inventory.dogCount < 999)
                        inventory.dogCount++;
                }
                else if (collision.gameObject.name == "Bomblebee Honeyham(Clone)")
                {
                    if (inventory.honeyhamCount < 999)
                        inventory.honeyhamCount++;
                }
                else if (collision.gameObject.name == "Rations(Clone)")
                {
                    if (inventory.rationsCount < 999)
                        inventory.rationsCount++;
                }
                Destroy(collision.gameObject);
                logic.GetComponent<UILogic>().score += 10;
            }
        }
    }
}