/****************************************************
    文件：BaseElement.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 21:6:33
	功能：元素基类
*****************************************************/

using UnityEngine;

public class BaseElement : MonoBehaviour 
{
    private int _positionX;
    private int _positionY;

    public ElementStates elementState;
    public ElementTypes elementType;
    public ElementContents elementContent;

    public int PositionX { get => _positionX;}
    public int PositionY { get => _positionY;}

    public virtual void Awake()
    {
        _positionX = (int)transform.position.x;
        _positionY = (int)transform.position.y;
        name = "(" + _positionX + "," + _positionY + ")";
    }

    public virtual void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonDown();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            OnRightMouseButtonDown();
        }
        else if(Input.GetMouseButtonDown(2))
        {
            OnMiddleMouseButtonDown();
        }
    }

    public virtual void OnPlayerStand()
    {
        //翻开元素操作
    }

    public virtual void OnLeftMouseButtonDown()
    {
        //寻路
        Debug.Log(PositionX + " " + PositionY);
        OnPlayerStand();
    }

    public virtual void OnRightMouseButtonDown()
    {
        //单翻元素与双翻元素插旗
    }

    public virtual void OnMiddleMouseButtonDown()
    {
        //数字元素快速访问
    }

    public void LoadSprite(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ClearShadow()
    {
        Transform shadow = transform.Find("shadow");
        if(shadow!=null)
        {
            Destroy(shadow.gameObject);
        }
    }

    
}