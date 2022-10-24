﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    public UnityAction turnStartAction;
    public UnityAction turnEndAction;

    public Text windForceValueText;

    public RectTransform teamA_HP_Mask;
    private float maskA_Width;

    public RectTransform teamB_HP_Mask;
    private float maskB_Width;

    public RectTransform windArrow;

    public RectTransform winPanel;

    public Sprite teamA_Sprite;
    public Sprite teamB_Sprite;
    public Image teamImage;

    public Transform endTurnButton;
    private float endX;
    private float endY;

    public bool isEnd;

    public Transform infoTip;
    public Text infoTipText;

    public bool isOpenedBag { get; set; }

    void Start()
    {
        maskA_Width = teamA_HP_Mask.sizeDelta.x;
        maskB_Width = teamB_HP_Mask.sizeDelta.x;
        endX = endTurnButton.localPosition.x;
        endY = endTurnButton.localPosition.y;
    }

    public void UpdateTeamA_HP_UI(float percentage)
    {
        teamA_HP_Mask.sizeDelta = new Vector2(maskA_Width * percentage, teamA_HP_Mask.sizeDelta.y);
    }

    public void UpdateTeamB_HP_UI(float percentage)
    {
        teamB_HP_Mask.sizeDelta = new Vector2(maskB_Width * percentage, teamB_HP_Mask.sizeDelta.y);
    }

    public void ShowWinInfoUI(string teamName)
    {
        switch (teamName)
        {
            case "A":
                teamImage.sprite = teamA_Sprite;
                break;

            case "B":
                teamImage.sprite = teamB_Sprite;
                break;
        }

        winPanel.DOLocalMoveY(0, 0).SetUpdate(true);
        SetEndTurnButtonActive(false);
        isEnd = true;
    }

    public void SetInfoTipActive(bool isActive)
    {
        infoTip.gameObject.SetActive(isActive);
    }

    public void SetInfoTipText(string text)
    {
        infoTipText.text = text;
    }

    public void SetEndTurnButtonActive(bool isActive)
    {
        endTurnButton.gameObject.SetActive(isActive);
    }

    public void GameInited()
    {
        Debug.Log("UI Init");
        turnStartAction = new UnityAction(OnTurnStartAction);
        turnEndAction = new UnityAction(OnTurnEndAction);
        GameManager.Instance.TurnBaseController.OnTurnStart.AddListener(turnStartAction);
        GameManager.Instance.TurnBaseController.OnTurnEnd.AddListener(turnEndAction);
    }

    void OnTurnStartAction()
    {
        Debug.Log("TurnStart");
        //回合开始，设置风力值
        windForceValueText.text = Mathf.Abs(GameManager.Instance.TurnBaseController.TurnProperties.WindForce.x).ToString();
        
        if(GameManager.Instance.TurnBaseController.TurnProperties.WindForce.x < 0)
        {
            windArrow.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            windArrow.localScale = new Vector3(1, 1, 1);
        }

        GameManager.Instance.CheckPlayer();
    }

    void OnTurnEndAction()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
