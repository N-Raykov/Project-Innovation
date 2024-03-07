using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPreviewScript : MonoBehaviour{

    [SerializeField] GameObject charactersScreen;
    [SerializeField] GameObject skillPreviewPage;

    public void ChangePreviewState() {

        charactersScreen.SetActive(!charactersScreen.activeSelf);
        skillPreviewPage.SetActive(!skillPreviewPage.activeSelf);

    }


}
