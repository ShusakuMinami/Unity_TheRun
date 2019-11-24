using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 定数定義
    // スコア最大数
    private const int MAX_SCORE = 999999;
    
    // 「ゲームオーバー」テキスト、「クリア」テキスト、操作ボタン、スコアテキスト
    public GameObject textGameOver;
    public GameObject textClear;
    public GameObject buttons;
    public GameObject textScoreNumber;

    // ゲーム状態定義
    public enum GAME_MODE
    {
        // プレイ中
        PLAY,
        // クリア
        CLEAR,
    };
    // ゲーム状態
    public GAME_MODE gameMode = GAME_MODE.PLAY;
    
    // スコア、表示用スコア
    private int score = 0;
    private int displayScore = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        RefreshScore();
    }

    
    // Update is called once per frame
    void Update()
    {
        // アニメーションのように点数を増加させる
        if(score > displayScore){
            displayScore += 10;
            
            if(displayScore > score){
                displayScore = score;
            }
            
            RefreshScore();
        }
    }
    
    
    // ゲームオーバー処理
    public void GameOver()
    {
        textGameOver.SetActive(true);
        buttons.SetActive(false);
    }
    
    
    // ゲームクリア処理
    public void GameClear()
    {
        gameMode = GAME_MODE.CLEAR;
        textClear.SetActive(true);
        buttons.SetActive(false);
    }
    
    
    // スコア加算
    public void AddScore(int val)
    {
        score += val;
        if(score > MAX_SCORE){
            score = MAX_SCORE;
        }
    }
    
    
    // スコア表示を更新
    void RefreshScore()
    {
        textScoreNumber.GetComponent<Text>().text = displayScore.ToString();
    }
}
