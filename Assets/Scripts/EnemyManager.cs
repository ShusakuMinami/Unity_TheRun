using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ブロックレイヤー
    public LayerMask blockLayer;
    
    // 敵制御用Rigidbody2D
    private Rigidbody2D rbody;
    
    // 移動速度
    private float moveSpeed = 1;
    
    // 移動方向定義
    public enum MOVE_DIR
    {
        LEFT,
        RIGHT,
    };
    
    // 移動方向
    private MOVE_DIR moveDirection = MOVE_DIR.LEFT;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void FixedUpdate()
    {
        // 進行方向にブロックがあるか否か
        bool isBlock;
        
        switch(moveDirection){
            // 左に移動
            case MOVE_DIR.LEFT:
                rbody.velocity = new Vector2(moveSpeed * -1, rbody.velocity.y);
                transform.localScale = new Vector2(1, 1);
                
                // 正面にブロックがないかを調べる
                isBlock = Physics2D.Linecast(
                    new Vector2(transform.position.x, transform.position.y + 0.5f),
                    new Vector2(transform.position.x - 0.4f, transform.position.y + 0.5f),
                    blockLayer);
                    
                if(isBlock){
                    moveDirection = MOVE_DIR.RIGHT;
                }
                
                break;
                
            // 右に移動
            case MOVE_DIR.RIGHT:
                rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
                // キャラを反転させる
                transform.localScale = new Vector2(-1, 1);
                
                // 正面にブロックがないかを調べる
                isBlock = Physics2D.Linecast(
                    new Vector2(transform.position.x, transform.position.y + 0.5f),
                    new Vector2(transform.position.x + 0.4f, transform.position.y + 0.5f),
                    blockLayer);
                    
                if(isBlock){
                    moveDirection = MOVE_DIR.LEFT;
                }
                
                break;
        }
    }
    
    
    // 敵オブジェクト削除処理
    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
