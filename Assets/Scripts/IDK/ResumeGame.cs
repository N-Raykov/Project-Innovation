using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    [SerializeField] GameObject objectToDisable;

   public void Resume() {
        objectToDisable.SetActive(false);
   }
}
