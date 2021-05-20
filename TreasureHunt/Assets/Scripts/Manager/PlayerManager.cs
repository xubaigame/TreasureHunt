// ****************************************************
//     文件：PlayerManager.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/21 21:22:23
//     功能：游戏角色管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using AStarPathfinding;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    private static PlayerManager instance = null;
    private Tweener pathTweener;
    private Animator playerAnimator;
    private bool pathFinding;
    private Vector3Int perPos;
    private Vector3Int nowPos;

    public GameObject hoeSelect;
    public GameObject tntSelect;
    public GameObject mapSelect;
    public static PlayerManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        instance = this;
        playerAnimator = GetComponent<Animator>();
        pathFinding = false;
        
    }

    // Update is called once per frame
    
    /// <summary>
    /// 角色移动逻辑
    /// </summary>
    void Update()
    {
        nowPos = transform.position.ToVector3Int();
        if (nowPos != perPos)
        {
            float dirx = Mathf.Clamp(nowPos.x - perPos.x,-1,1);
            float diry = Mathf.Clamp(nowPos.y - perPos.y,-1,1);
            playerAnimator.SetFloat("DirX",dirx);
            playerAnimator.SetFloat("DirY",diry);
            
            MapManager.Instance.UncoverElementDouble(nowPos.x, nowPos.y);
            if (ElementContents.Trap == MapManager.Instance.GetElementContentByPosition(nowPos.x, nowPos.y))
            {
                pathTweener.Kill();
                nowPos = perPos;
                transform.position = perPos;
            }
            else
            {
                perPos = nowPos;
            }
        }

    }

    /// <summary>
    /// 初始化角色位置
    /// </summary>
    /// <param name="x">横坐标</param>
    /// <param name="y">纵坐标</param>
    public void InitPlayerPosition(int x,int y)
    {
        transform.position = new Vector3(x, y, 0);
        perPos=transform.position.ToVector3Int();
        nowPos=Vector3Int.zero;
    }

    /// <summary>
    /// 获得角色位置
    /// </summary>
    /// <returns>角色位置对象</returns>
    public Vector2Int GetPlayerPosition()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }
    
    /// <summary>
    /// 按照路径移动角色
    /// </summary>
    /// <param name="Path">路径点数组</param>
    public void MoveWithPath(Vector3[] Path)
    {
        if(pathFinding)
            pathTweener.Kill();
        //CameraManager.Instance.ResetFollowTarget();
        pathFinding = true;
        playerAnimator.SetBool("Move",pathFinding);
        pathTweener= transform.DOPath(Path,Path.Length*0.1f);
        pathTweener.SetEase(Ease.Linear);
        pathTweener.onComplete += () =>
        {
            pathFinding = false;
            playerAnimator.SetBool("Move", pathFinding);
        };
        pathTweener.onKill += () =>
        {
            pathFinding = false;
            playerAnimator.SetBool("Move", pathFinding);
        };
    }

    /// <summary>
    /// 设置动画状态机播放Why动画
    /// </summary>
    public void ShowWhyAnimation()
    {
        playerAnimator.SetTrigger("Why");
    }

    /// <summary>
    /// 设置动画状态机播放QuickCheck动画
    /// </summary>
    public void ShowQuickCheckAnimation()
    {
        playerAnimator.SetTrigger("QuickCheck");
    }
    
    /// <summary>
    /// 设置动画状态机播放TakeDamage动画
    /// </summary>
    public void ShowTakeDamageAnimation()
    {
        playerAnimator.SetTrigger("TakeDamage");
    }

    /// <summary>
    /// 设置动画状态机播放Die动画
    /// </summary>
    public void ShowDieAnimation()
    {
        playerAnimator.SetTrigger("Die");
    }
    
    /// <summary>
    /// 设置动画状态机播放Pass动画
    /// </summary>
    public void ShowPassAnimation()
    {
        playerAnimator.SetTrigger("Pass");
    }

    /// <summary>
    /// 处理角色受伤方法
    /// </summary>
    public void TakeDamage()
    {
        if (GameDataManager.Instance.gameData.Armor > 0)
        {
            GameDataManager.Instance.ChangeArmor(-1);
        }
        else
        {
            AudioManager.Instance.PlayEffect(Consts.hurt);
            GameDataManager.Instance.ChangeHp(-1);
        }
    }
}
