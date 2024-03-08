using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{

   [SerializeField] Material zoomBlurMat;

   public void changeScene(string sceneName)
   {
        //Reset shader effect to 0, otherwise it keeps being on screen when switching scenes
      Time.timeScale = 1f;
      zoomBlurMat.SetFloat("_EffectOpacity", 0f);
      SceneManager.LoadScene(sceneName);
   }
}
