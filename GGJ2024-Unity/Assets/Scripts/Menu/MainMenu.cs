﻿using System;
using Scripts.Game;
using Scripts.MainMenuStuff.Credits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class MainMenu: MonoBehaviour
    {

        [SerializeField] private CanvasGroup entireCanvasGroupForTheWholeMenu;
        
        [SerializeField] private CanvasGroup mainMenuCanvasGroup;

        [SerializeField] private InGameHUD theHUD;
        
        [SerializeField] private CreditsInfo theCredits;

        [SerializeField] private Button startGameButton;

        [SerializeField] private Button showCreditsButton;

        [SerializeField] private Button instructionsPopupButton;

        [SerializeField] private TextMeshProUGUI highScoreText;

        private bool _isShowingCredits = false;

        private bool _isShowingInstructions = false;
        private bool _waitOneFrame = false;

        public bool IsShowingCredits => _isShowingCredits;

        private void Awake()
        {
            _isShowingInstructions = false;
            _isShowingCredits = false;
            _waitOneFrame = false;
            startGameButton.onClick.AddListener(ShowInstructionsPopup);
            instructionsPopupButton.onClick.AddListener(GamerTime);
            showCreditsButton.onClick.AddListener(ShowCredits);
            
            
            instructionsPopupButton.gameObject.SetActive(false);
            
            theCredits.HideMe();
            entireCanvasGroupForTheWholeMenu.alpha = 1f;
            entireCanvasGroupForTheWholeMenu.interactable = true;

            int highestScore = PlayerPrefs.GetInt(GameManager.K_PLAYERPREFS_HIGH_SCORE);
            highScoreText.text = $"High Score: {highestScore}";
        }

        private void ShowInstructionsPopup()
        {
            _waitOneFrame = true;
            _isShowingInstructions = true;
            _isShowingCredits = false;
            mainMenuCanvasGroup.interactable = false;
            mainMenuCanvasGroup.alpha = 0f;
            instructionsPopupButton.gameObject.SetActive(true);
            instructionsPopupButton.Select();
        }

        private void Update()
        {
            if (_isShowingInstructions)
            {
                if (_waitOneFrame)
                {
                    _waitOneFrame = false;
                }
                else if (Input.GetButtonDown("Submit")|| 
                            Input.GetMouseButtonDown(0) ||
                            Input.GetMouseButtonDown(1) ||
                            Input.GetKeyDown(KeyCode.A) ||
                            Input.GetKeyDown(KeyCode.W) ||
                            Input.GetKeyDown(KeyCode.S) ||
                            Input.GetKeyDown(KeyCode.D) ||
                            Input.GetKeyDown(KeyCode.Q) ||
                            Input.GetKeyDown(KeyCode.E))
                {
                    GamerTime();
                }
            }
        }

        private void GamerTime()
        {
            _isShowingInstructions = false;
            entireCanvasGroupForTheWholeMenu.alpha = 0f;
            entireCanvasGroupForTheWholeMenu.interactable = false;
            entireCanvasGroupForTheWholeMenu.gameObject.SetActive(false);

            theHUD.gameObject.SetActive(true);

            GameManager.Instance.ItsGamerTime();
        }

        private void ShowCredits()
        {
            _isShowingCredits = true;
            mainMenuCanvasGroup.alpha = 0f;
            mainMenuCanvasGroup.interactable = false;
            theCredits.ShowMe();
        }

        public void BackToMainMenu()
        {
            _isShowingCredits = false;
            mainMenuCanvasGroup.alpha = 1f;
            mainMenuCanvasGroup.interactable = true;
        }
    }
}