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

public class PlayerManager : MonoBehaviour
{
    

    private static PlayerManager instance = null;
    private Tweener pathTweener;
    private Animator playerAnimator;
    private bool pathFinding;
    private Vector3Int perPos;
    private Vector3Int nowPos;
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

    
    public void InitPlayerPosition(int x,int y)
    {
        transform.position = new Vector3(x, y, 0);
        perPos=transform.position.ToVector3Int();
        nowPos=Vector3Int.zero;
    }

    public AStarPoint GetPlayerPosition()
    {
        return new AStarPoint((int)transform.position.x, (int)transform.position.y);
    }
    public void MoveWithPath(Vector3[] Path)
    {
        if(pathFinding)
            pathTweener.Kill();

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

    public void ShowWhyAnimation()
    {
        playerAnimator.SetTrigger("Why");
    }
    
    
    
}
