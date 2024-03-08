using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour{

    [SerializeField] Image characterImage;
    [SerializeField] RectTransform content;

    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI accelerationText;
    [SerializeField] TextMeshProUGUI attackCooldownText;
    [SerializeField] TextMeshProUGUI characterNameText;

    [Header("Skill Page")]
    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillDescriptionText;
    [SerializeField] TextMeshProUGUI skillDurationText;
    [SerializeField] TextMeshProUGUI SkillCooldownText;

    private void Awake(){
        for (int i = 0; i < content.childCount; i++) { 
            content.GetChild(i).GetComponent<DisplayCharacterStats>().OnClick += UpdateDisplay;
        }
    }
    private void UpdateDisplay(Sprite pSprite,Sprite pDisplaySprite, float pDamageValue, float pMaxSpeed, string pAcceleration, float pAttackCooldown,string pCharacterName,string pSkillName,string pSkillDescription,float pSkillDuration,float pSkillCooldown,GameObject pCharacterPrefab, bool pIsOn){

        if (!pIsOn)
            return;

        characterImage.sprite = pDisplaySprite;
        damageText.text = pDamageValue.ToString();
        speedText.text = pMaxSpeed.ToString();
        accelerationText.text = pAcceleration;
        attackCooldownText.text = pAttackCooldown.ToString();
        characterNameText.text = pCharacterName;

        skillNameText.text = pSkillName;
        skillDescriptionText.text = pSkillDescription;
        skillDurationText.text = pSkillDuration.ToString();
        SkillCooldownText.text = pSkillCooldown.ToString();

        CharacterSpawner.instance.SetSelectedCharacter(pCharacterPrefab);

    }

    public void LoadScene(string pTargetScene) {
        SceneManager.LoadScene(pTargetScene);
    }

}
