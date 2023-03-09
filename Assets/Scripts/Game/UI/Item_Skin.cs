using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Skin : MonoBehaviour
{
    public Image img_skin;

    public void Set(Sprite s)
    {
        img_skin.sprite = s;
        img_skin.transform.localScale = Vector3.one;
    }

    public void SetGray(bool b)
    {
        img_skin.color = b ? Color.gray : Color.white;
    }

    public void SetScale(float sacle)
    {
        img_skin.transform.localScale = Vector3.one * sacle;
    }
}
