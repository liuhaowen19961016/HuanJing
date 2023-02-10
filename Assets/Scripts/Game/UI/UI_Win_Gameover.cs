using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Gameover : MonoBehaviour
{
    public Button btn_replay;
    public Button btn_menu;
    public Text txt_curScore;
    public Text txt_curDiamond;
    public Text txt_bextScore;

    private void Awake()
    {
        gameObject.SetActive(false);

        btn_menu.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            GameObject.FindObjectOfType<UI_Win_Game>().gameObject.SetActive(false);
            GameObject.FindObjectOfType<UI_Win_Start>().gameObject.SetActive(true);
        });
    }
}
