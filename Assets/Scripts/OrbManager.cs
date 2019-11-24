using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    // オーブの得点
    private const int ORB_POINT = 100;
    
    // ゲームマネージャー
    private GameObject gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    // オーブ入手処理
    public void GetOrb()
    {
        gameManager.GetComponent<GameManager>().AddScore(ORB_POINT);
        Destroy(this.gameObject);
    }
}
