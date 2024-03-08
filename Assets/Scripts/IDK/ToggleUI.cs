using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{

    [SerializeField] GameObject uiObject;

    public void Toggle() {
        uiObject.SetActive(!uiObject.activeSelf);
    }
}
