using System.Collections;
using System.Collections.Generic;
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
    public Image fillImage;

    void Awake()
    {
        instance = this;
        fillImage.fillAmount = 1f;
    }
}
