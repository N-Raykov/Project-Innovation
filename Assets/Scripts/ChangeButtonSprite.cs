using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonSprite : MonoBehaviour{

    [SerializeField] Button button;
    [SerializeField] Sprite spriteOriginal;
    [SerializeField] Sprite spritePressed;


    public void ChangeSprite(bool pState) {

        button.image.sprite = (pState) ? spritePressed : spriteOriginal;
        
    }


}
