using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Button boostButton;
    private Button shieldButton;
    private Image fillImage;

    public float boostAmount = 5f;
    public float duration = 3f;
    public GameObject obstacle;
    public float maxNumberOfObstacles = 5f;
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

        boostButton = ButtonCollection.instance.boostButton;
        fillImage = ButtonCollection.instance.fillImage;
        boostButton.onClick.AddListener(Boost);

        shieldButton = ButtonCollection.instance.shieldButton;
        shieldButton.onClick.AddListener(Shield);

        if(GameManager.instance.robotTouchController == null)
        {
            GameManager.instance.robotTouchController = this;
        }
        #endregion
    }


    void Update()
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
        fillImage.fillAmount = 1f;
        GameManager.instance.LostLives();
    }


    #region Challenges

    public void Jump()
    {
        robotRigidbody.AddForce(transform.up * jumpForce);

    }

    public void Boost()
    {
        moveSpeed *= boostAmount;
        StartCoroutine(TimedBoost(duration));
    }

    private IEnumerator TimedBoost(float duration)
    {
        // set the start time to the time at the
        // beginning of the frame
        float startTime = Time.time;
        float time = duration;
        float value = 0;

        while(Time.time - startTime < duration)
        {
            time -= Time.deltaTime;
            value = time / duration;
            fillImage.fillAmount = value;

            yield return null;
        }

        moveSpeed /= boostAmount;

        fillImage.fillAmount = 1f;
        yield return new WaitForEndOfFrame();
    }

    private void Shield()
    {
        SpawnObstacles(transform.position);
    }

    public void SpawnObstacles(Vector3 position)
    {
        int randomMax = (int)Random.Range(0, maxNumberOfObstacles + 1);

        for (int i = 0; i <= randomMax; i++)
        {
            Vector3 randomPosition = (Random.insideUnitSphere * radius) + position;

            Instantiate(obstacle, randomPosition, Random.rotation);
        }
    }

    #endregion
}
