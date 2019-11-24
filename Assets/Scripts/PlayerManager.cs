using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ゲームマネージャー
    public GameObject gameManager;
    
    // ブロックレイヤー
    public LayerMask blockLayer;
    
    // プレイヤー制御用Rigidbody2D
    private Rigidbody2D rbody;
    
    // 移動速度固定値、移動速度
    private const float MOVE_SPEED = 3;
    private float moveSpeed;
    // ジャンプ力、ジャンプしたか否か、ブロックに接しているか否か
    private float jumpPower = 400;
    private bool goJump = false;
    private bool canJump = false;
    // ボタンを利用しているか否か
    private bool usingButtons = false;
    
    // 移動方向定義
    public enum MOVE_DIR{
        STOP,
        LEFT,
        RIGHT,
    };
    
    // 移動方向
    private MOVE_DIR moveDirection = MOVE_DIR.STOP;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 指定した線分内に、ブロックレイヤーがあるかどうかを判定し、
        // ジャンプが可能かどうかを調べる
        canJump =
            Physics2D.Linecast(transform.position - (transform.right * 0.3f), transform.position - (transform.up * 0.1f), blockLayer) ||
            Physics2D.Linecast(transform.position + (transform.right * 0.3f),  transform.position - (transform.up * 0.1f), blockLayer);
            
        // キーボード操作時
        if(!usingButtons){
            float x = Input.GetAxisRaw("Horizontal");
            if(x == 0){
                moveDirection = MOVE_DIR.STOP;
            }
            else{
                if(x < 0){
                    moveDirection = MOVE_DIR.LEFT;
                }
                else{
                    moveDirection = MOVE_DIR.RIGHT;
                }
            }
            
            if(Input.GetKeyDown("space")){
                PushJumpButton();
            }
        }
    }
    
    
    void FixedUpdate()
    {
        // 移動方向で処理を分岐
        switch(moveDirection){
            // 停止
            case MOVE_DIR.STOP:
                moveSpeed = 0;
                break;
            // 左に移動
            case MOVE_DIR.LEFT:
                moveSpeed = MOVE_SPEED * -1;
                transform.localScale = new Vector2(-1, 1);
                break;
            // 右に移動
            case MOVE_DIR.RIGHT:
                moveSpeed = MOVE_SPEED;
                transform.localScale = new Vector2(1, 1);
                break;
        }
        rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
        
        // ジャンプ処理
        if(goJump){
            rbody.AddForce(Vector2.up * jumpPower);
            goJump = false;
        }
    }
    
    
    // 衝突処理
    void OnTriggerEnter2D(Collider2D col)
    {
        // プレイ中でなければ接触判定を行わない
        if(gameManager.GetComponent<GameManager>().gameMode != GameManager.GAME_MODE.PLAY){
            return;
        }
        
        // トラップに当たった時
        if(col.gameObject.tag == "Trap"){
            gameManager.GetComponent<GameManager>().GameOver();
            DestroyPlayer();
        }
        
        // ゴールした時
        if(col.gameObject.tag == "Goal"){
            gameManager.GetComponent<GameManager>().GameClear();
        }
        
        // 敵に当たった時
        if(col.gameObject.tag == "Enemy"){
            // 敵を上から踏んだ場合
            if(transform.position.y > col.gameObject.transform.position.y + 0.4f){
                rbody.velocity = new Vector2(rbody.velocity.x, 0);
                rbody.AddForce(Vector2.up * jumpPower);
                col.gameObject.GetComponent<EnemyManager>() .DestroyEnemy();
            }
            // 横から当たった場合
            else{
                gameManager.GetComponent<GameManager>().GameOver();
                DestroyPlayer();
            }
        }
        
        // オーブに当たった時
        if(col.gameObject.tag == "Orb"){
            col.gameObject.GetComponent<OrbManager>().GetOrb();
        }
    }
    
    
    // プレイヤーオブジェクト削除処理
    void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }
    
    
    // 左ボタンを押した
    public void PushLeftButton()
    {
        moveDirection = MOVE_DIR.LEFT;
        usingButtons = true;
    }

    // 右ボタンを押した
    public void PushRightButton()
    {
        moveDirection = MOVE_DIR.RIGHT;
        usingButtons = true;
    }

    // 移動ボタンを離した
    public void ReleaseMoveButton()
    {
        moveDirection = MOVE_DIR.STOP;
        usingButtons = false;
    }
    
    // ジャンプボタンを押した
    public void PushJumpButton()
    {
        if(canJump){
            goJump = true;
        }
    }

}
