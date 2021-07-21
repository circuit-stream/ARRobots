using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RobotTouchController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float turnSpeed = 5f;
    public float deadZone = 0.2f;
    public float jumpForce = 20f;

    private Animator robotAnim;
    private Rigidbody robotRigidbody;
    private Joystick joystick;


    #region Challenge variables

    /// <summary>
    /// Challenges variables
    /// </summary>
    private Button jumpButton;
    private bool isGrounded;

    private Button boostButton;
    public float boostAmount = 2f;
    public float boostDuration = 1f;
    public float boostCooldown = 3f;
    private Coroutine activeBoostCoroutine;
    private Image boostCooldownImage;
    private Image boostActiveImage;

    private Button shieldButton;
    public float shieldDuration = 1f;
    public float shieldCooldown = 5f;
    public GameObject shieldVisual;
    private Coroutine activeShieldCoroutine;
    private Image shieldCooldownImage;
    private Image shieldActiveImage;
    public bool IsShieldActive => shieldVisual.activeSelf;

    public GameObject obstacle;
    public int maxNumberOfObstacles = 3;
    public float radius = 0.3f;

    #endregion

    private void OnEnable()
    {
        joystick = FindObjectOfType<Joystick>();
        robotRigidbody = GetComponent<Rigidbody>();
        robotAnim = GetComponent<Animator>();
        robotAnim.SetBool("Open_Anim", true);

        #region challenge
        // Challenges
        jumpButton = ButtonCollection.instance.jumpButton;
        jumpButton.onClick.AddListener(Jump);

        boostCooldownImage = ButtonCollection.instance.boostCooldownImage;
        boostActiveImage = ButtonCollection.instance.boostActiveImage;
        boostButton = ButtonCollection.instance.boostButton;
        boostButton.onClick.AddListener(Boost);

        shieldCooldownImage = ButtonCollection.instance.shieldCooldownImage;
        shieldActiveImage = ButtonCollection.instance.shieldActiveImage;
        shieldButton = ButtonCollection.instance.shieldButton;
        shieldButton.onClick.AddListener(Shield);
        shieldVisual.SetActive(false);

        if(GameManager.instance.robotTouchController == null)
        {
            GameManager.instance.robotTouchController = this;
        }

        StartCoroutine(SpawnObstacles());
        #endregion
    }

    private void Update()
    {
        // movement
        if(joystick.Direction.magnitude >= deadZone)
        {
            robotRigidbody.AddForce(transform.forward * moveSpeed);

            robotAnim.SetBool("Walk_Anim", true);
        }
        else
        {
            // idle
            robotAnim.SetBool("Walk_Anim", false);
        }

        // rotation
        Vector3 targetDirection = new Vector3(joystick.Direction.x,0, joystick.Direction.y);
        Vector3 direction = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * turnSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnDestroy()
    {
        boostCooldownImage.fillAmount = 1f;
        GameManager.instance.LostLives();
    }


    #region Challenges

    private void Jump()
    {
        if (isGrounded)
        {
            robotRigidbody.AddForce(transform.up * jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;
        }
    }

    private void Boost()
    {
        if (activeBoostCoroutine == null)
        {
            activeBoostCoroutine = StartCoroutine(TimedBoost());
        }
    }

    private IEnumerator TimedBoost()
    {
        // set the start time to the time at the
        // beginning of the frame
        float startTime = Time.time;

        // Activate boost
        moveSpeed *= boostAmount;
        boostActiveImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(boostDuration);

        // Deactivate boost
        moveSpeed /= boostAmount;
        boostActiveImage.gameObject.SetActive(false);

        // Wait cooldown to end
        float elapsedTime = Time.time - startTime;
        while(elapsedTime < boostCooldown)
        {
            boostCooldownImage.fillAmount = elapsedTime / boostCooldown;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        boostCooldownImage.fillAmount = 1f;

        yield return new WaitForEndOfFrame();

        activeBoostCoroutine = null;
    }

    private void Shield()
    {
        if (activeShieldCoroutine == null)
        {
            activeShieldCoroutine = StartCoroutine(TimedShield());
        }
    }

    private IEnumerator TimedShield()
    {
        // set the start time to the time at the
        // beginning of the frame
        float startTime = Time.time;

        shieldVisual.SetActive(true);
        shieldActiveImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        shieldVisual.SetActive(false);
        shieldActiveImage.gameObject.SetActive(false);

        // Wait cooldown to end
        float elapsedTime = Time.time - startTime;
        while(elapsedTime < shieldCooldown)
        {
            shieldCooldownImage.fillAmount = elapsedTime / shieldCooldown;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        shieldCooldownImage.fillAmount = 1f;

        yield return new WaitForEndOfFrame();

        activeShieldCoroutine = null;
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6f, 10f));

            int randomMax = Random.Range(1, maxNumberOfObstacles + 1);
            Vector3 position = transform.position;

            for (int i = 0; i <= randomMax; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * radius;
                Vector3 randomPosition = position + new Vector3(randomCircle.x, 0.5f, randomCircle.y);

                Instantiate(obstacle, randomPosition, Random.rotation);
            }
        }
    }

    #endregion
}
