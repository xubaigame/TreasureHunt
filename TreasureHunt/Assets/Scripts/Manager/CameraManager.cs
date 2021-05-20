// ****************************************************
//     文件：CameraManager.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/22 13:44:17
//     功能：摄像机管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool UseProCamera2D;
    
    private CinemachineVirtualCamera cvc;
    private CinemachineFramingTransposer cft;
    
    private int mapWidth;
    private int mapHeight;
    
    private static CameraManager instance = null;
    public static CameraManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        instance = this;
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        mapWidth = (int)temp.x;
        mapHeight = (int)temp.y;
    }

    public void InitCamera()
    {
        if(UseProCamera2D)
            InitProCamera();
        else
            InitCinemachineCamera();
        
        
        
    }

    public void InitCinemachineCamera()
    {
        cvc=GameObject.Find("Vcam").GetComponent<CinemachineVirtualCamera>();
        cvc.m_Lens.OrthographicSize = (mapHeight + 3f) / 2f;
        cft = cvc.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;
        cft.m_DeadZoneHeight = (mapHeight * 100f) / (300 + mapHeight * 100);
        cft.m_DeadZoneWidth = cft.m_DeadZoneHeight / 9 * 16 / mapHeight;
        
        PolygonCollider2D pc= GetComponent<PolygonCollider2D>();
        pc.SetPath(0, new Vector2[]
        {
            new Vector2(-2f,-2f),
            new Vector2(-2f,mapHeight+1f),
            new Vector2(mapWidth+1f,mapHeight+1f),
            new Vector2(mapWidth+1f,-2f)
        });
        
        CinemachineConfiner cc = cvc.gameObject.GetComponent<CinemachineConfiner>();
        cc.enabled = true;
        cc.m_BoundingShape2D = pc;
        cvc.Follow = PlayerManager.Instance.transform.GetChild(0);
        
        Camera.main.transform.GetChild(0).localPosition = new Vector3(0,cvc.m_Lens.OrthographicSize,0);
        ParticleSystem.ShapeModule sm = Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().shape;

        sm.scale = new Vector3(Screen.width * 35.5f * cvc.m_Lens.OrthographicSize * 1080f / 10f / 1920f / Screen.height,
            1, 1);
    }

    public void InitProCamera()
    {
        ProCamera2DCameraWindow pccw=Camera.main.GetComponent<ProCamera2DCameraWindow>();
        pccw.CameraWindowRect.height=(mapHeight * 100f) / (300 + mapHeight * 100);
        pccw.CameraWindowRect.width = pccw.CameraWindowRect.height / 9 * 16 / mapHeight;
        
        ProCamera2DNumericBoundaries pcnb = Camera.main.GetComponent<ProCamera2DNumericBoundaries>();
        pcnb.LeftBoundary = -2f;
        pcnb.RightBoundary = mapWidth + 1f;
        pcnb.BottomBoundary = -2;
        pcnb.TopBoundary = mapHeight + 1f;
        
        Camera.main.transform.GetChild(0).localPosition = new Vector3(0,Camera.main.orthographicSize,0);
        ParticleSystem.ShapeModule sm = Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
        sm.scale = new Vector3(Screen.width * 35.5f * Camera.main.orthographicSize * 1080f / 10f / 1920f / Screen.height,
            1, 1);
    }


    public void ResetFollowTarget()
    {
        PlayerManager.Instance.transform.GetChild(0).transform.localPosition=Vector3.zero;
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerManager.Instance.transform.GetChild(0).transform.position =
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)) +
                new Vector3(0, 0, 10);
        }
        if (Input.GetMouseButton(0))
        {
            PlayerManager.Instance.transform.GetChild(0).transform.position +=
                new Vector3(-Input.GetAxis("Mouse X") * 0.5f, 0, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ResetFollowTarget();
        }
    }
}
