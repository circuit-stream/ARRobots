using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Button Collection for the challenges
/// </summary>
public class ButtonCollection : MonoBehaviour
{
    public static ButtonCollection instance;

    public Button jumpButton;
    public Button boostButton;
    public Button shieldButton;
    public Image boostCooldownImage;
    public Image boostActiveImage;
    public Image shieldCooldownImage;
    public Image shieldActiveImage;

    void Awake()
    {
        instance = this;
        boostCooldownImage.fillAmount = 1f;
        shieldCooldownImage.fillAmount = 1f;
        shieldActiveImage.gameObject.SetActive(false);
        boostActiveImage.gameObject.SetActive(false);
    }
}
