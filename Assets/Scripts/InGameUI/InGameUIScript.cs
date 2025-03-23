using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIScript : MonoBehaviour
{
    public static InGameUIScript Instance { get; private set; }
    public Image upKey;
    public Image leftKey;
    public Image downKey;
    public Image rightKey;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ToggleButton(Image button, bool isActive)
    {
        button.color = isActive ? Color.yellow : Color.white;
    }


}
