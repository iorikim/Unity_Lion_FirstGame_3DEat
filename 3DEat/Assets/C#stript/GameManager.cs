﻿using UnityEngine;
using UnityEngine.UI;               // UI API
using UnityEngine.SceneManagement;  //場景管理 API

public class GameManager : MonoBehaviour
{
    #region 欄位與屬性
    [Header("道具")]
    public GameObject[] props;
    [Header("文字介面：道具總數")]
    public Text textCount;
    [Header("文字介面：倒數時間")]
    public Text textTime;
    [Header("文字介面：結束畫面標題")]
    public Text textTitle;
    [Header("結束畫面")]
    public CanvasGroup final;


    /// <summary>
    /// 道具總數
    /// </summary>
    private int countTotal;

    /// <summary>
    /// 取得道具數量
    /// </summary>
    private int countProp;
    /// <summary>
    /// 遊戲時間
    /// </summary>
    private float gameTime = 30;
    #endregion

    #region 方法

    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private int CreateProp(GameObject prop, int count)
    {
        //取得隨機道具數量 = 指定的數量 + - 5
        int total = count + Random.Range(-5, 5);
        //for 迴圈
        for (int i = 0; i < total; i++)
        {
            //座標 =(隨機，1.5，隨機)
            Vector3 pos = new Vector3(Random.Range(-9, 9), 6.5f, Random.Range(-9, 9));
            //生成(物件，座標，角度)
            Instantiate(prop, pos, Quaternion.identity);
        }
        //傳回 道具數量
        return total;
    }

    private void CountTime()
    {   
        //如果取得所有雞腿就跳出
        if (countProp == countTotal) return;

        //遊戲時間 遞減 一幀的時間
        gameTime -= Time.deltaTime;

        //遊戲時間 = 數學.夾住 (遊戲時間，最小值，最大值)
        gameTime = Mathf.Clamp(gameTime, 0, 100);

        //更新倒數時間介面 ToString("f小數點位數")
        textTime.text = "倒數時間" + gameTime.ToString("f2");

        Lose();
    }
    #endregion

    #region 事件
    private void Start()
    {
        countTotal = CreateProp(props[0], 10);      // 道具總數 = 生成道具(道具一號，指定數量)

        textCount.text = "道具數量: 0/" + countTotal;

        CreateProp(props[1], 10);                   //生成道具(道具二號，指定數量)
        
    }

    private void Update()
    {
        CountTime();
    }

    public void GetProp(string prop)
    {
        if (prop == "雞腿")
        {
            countProp++;
            textCount.text = "道具數量：" + countProp + "/" + countTotal;

            Win();          //呼叫勝利方法
        }
        else if (prop == "高梁")
        {
            gameTime -= 2;
            textTime.text = "倒數時間：" + gameTime.ToString("f2");

            Lose();
        }
    }

    /// <summary>
    /// 勝利:吃光所有雞腿
    /// </summary>
    private void Win()
    {
        if (countProp == countTotal)                    //如果雞腿數量 = 雞腿總數
        {
            final.alpha = 1;                            //顯示結束畫面、啟動互動、啟動遮擋
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "恭喜你吃完雞腿惹";          //更新結束畫面標題
        }

    }

    /// <summary>
    /// 失敗：時間歸零
    /// </summary>
    private void Lose()
    {
        if(gameTime == 0)
        {
            final.alpha = 1;
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "挑戰失敗!!!";
            FindObjectOfType<Player>().enabled = false;     //取得玩家.啟動 =false
        }
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit(); //應用程式.離開()
    }


    #endregion
}
