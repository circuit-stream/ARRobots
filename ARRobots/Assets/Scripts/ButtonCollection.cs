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

    public float duration = 3f;
    public float moveSpeed = 10f;
    public float boostAmount = 2f;

    void Awake()
    {
        instance = this;
        fillImage.fillAmount = 1f;
    }
}
