﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game.Menu
{
    public class GameOverHUD: MonoBehaviour
    {

        public CanvasGroup goodEndingHud;

        public CanvasGroup badEndingHud;

        public TextMeshProUGUI wavesSurvivedText;

        public CanvasGroup gameOverHudCanvasGroup;

        public Button playAgainButton;

        private void Awake()
        {
            badEndingHud.gameObject.SetActive(false);
            goodEndingHud.gameObject.SetActive(false);
            gameOverHudCanvasGroup.interactable = false;
            gameOverHudCanvasGroup.alpha = 0f;
            
            playAgainButton.gameObject.SetActive(false);
            
            playAgainButton.onClick.AddListener(PlayAgain);
        }

        public void BadEnding(int wavesSurvived)
        {
            badEndingHud.gameObject.SetActive(true);
            gameOverHudCanvasGroup.interactable = true;
            gameOverHudCanvasGroup.alpha = 1f;
            wavesSurvivedText.text = $"Ye survived <b>{wavesSurvived}</b> waves!";

            StartCoroutine(WaitToShowPlayAgainButtonRoutine());
        }

        private IEnumerator WaitToShowPlayAgainButtonRoutine()
        {
            yield return new WaitForSeconds(5f);
            ShowPlayAgainButton();
            yield break;
        }

        public void ShowPlayAgainButton()
        {
            playAgainButton.gameObject.SetActive(true);
        }

        public void GoodEnding()
        {
            goodEndingHud.gameObject.SetActive(true);
            gameOverHudCanvasGroup.interactable = true;
            gameOverHudCanvasGroup.alpha = 1f;
        }

        private void PlayAgain()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}