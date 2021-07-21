using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var robotController = collision.collider.GetComponent<RobotTouchController>();

        if (robotController)
        {
            Destroy(gameObject);

            if (!robotController.IsShieldActive)
            {
                Destroy(collision.gameObject);
            }
        }

        else if (collision.collider.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
