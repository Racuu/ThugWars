﻿/*
 Author: JackZhang
 Description: 队伍成员类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurnBaseUtil
{
    public class TeamPlayer : MonoBehaviour
    {
        //序号
        private int index;
        public int Index { get { return index; } set { index = value; } }

        public PlayerController PlayerController { get; set; }

        public Team belongsTo { get; set; }

        private bool isDestroyed;

        private PlayerUI ui;

        //昵称
        private string name;
        public string Name { get { return name; } set { name = value; } }

        //颜色(UI)
        private Color teamColor;

        //血量
        public int hp = 100;

        public TeamPlayer(string name, Color color)
        {
            this.name = name;
            teamColor = color;
        }

        public void InitUI()
        {
            ui.UpdateName(name);
            ui.UpdateColor(teamColor);
            ui.UpdatePlayerHP(hp);
        }

        public void Copy(TeamPlayer teamPlayer)
        {
            name = teamPlayer.Name;
            teamColor = teamPlayer.teamColor;
            belongsTo = teamPlayer.belongsTo;
        }

        public void DoHurt(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                PlayerController.GetComponent<Animator>().SetTrigger("Die");
                PlayerController.GetComponent<AudioSource>().PlayOneShot(PlayerController.dieSFX);
                ui.SetHudActive(false);
                ui.UpdatePlayerHP(hp);
                belongsTo.UpdateHP();
                //Invoke("RemoveSelf", 2f);
                RemoveSelf();
                PlayerController.IsDead = true;
                return;
            }
            ui.UpdatePlayerHP(hp);
            belongsTo.UpdateHP();

        }

        void Start()
        {
            PlayerController = GetComponent<PlayerController>();
            ui = GetComponent<PlayerUI>();
        }

        private void RemoveSelf()
        {
            belongsTo.RemoveTeamPlayer(this);
            Debug.Log(belongsTo.GetCurrentTeamPlayerCount());
            if (belongsTo.GetCurrentTeamPlayerCount() > 0)
            {
                GameManager.Instance.TurnBaseController.EndTurn();
                GameManager.Instance.TurnBaseController.StartTurn();
            }
            else
            {
                Team winTeam = GameManager.Instance.TurnBaseController.GetTeam(GameManager.Instance.TurnBaseController.NextTurnTeamIndex());
                Debug.Log(winTeam.Name + "Win");
                UIManager.Instance.ShowWinInfoUI(winTeam.Name);
                GameManager.Instance.vCam.Follow = winTeam.GetCurrentTurnPlayer().gameObject.transform;
                //Time.timeScale = 0;
            }

            
        }

        void Update()
        {
            if (transform.position.y < -8f)
            {
                if (!isDestroyed && !PlayerController.IsDead)
                {
                    hp = 0;
                    ui.UpdatePlayerHP(hp);
                    belongsTo.UpdateHP();
                    //Invoke("RemoveSelf", 2f);
                    RemoveSelf();
                    Destroy(gameObject);
                    isDestroyed = true;
                }
                
            }
        }
    }
}

