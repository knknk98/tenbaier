using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GenerateStageAndBackground gsab;
    [SerializeField] private InputUI inputUI;
    [SerializeField] private GameObject gameOverText;
    
    [SerializeField] private float maxJumpPower;
    [SerializeField] private float minJumpPower;
    
    [SerializeField] private float maxCameraSize;
    [SerializeField] private float minCameraSize;
    
    [SerializeField] private float maxRecoverySpeed;
    [SerializeField] private float minRecoverySpeed;
    
    [SerializeField] private float maxGravityScale;
    [SerializeField] private float minGravityScale;

    [SerializeField] private Vector3 maxCameraPos;
    [SerializeField] private Vector3 minCameraPos;

    [SerializeField] private float maxSpeedRate;
    [SerializeField] private float minSpeedRate;
    
    [SerializeField] private float maxScale = 10f;
    [SerializeField] private float transitionTime;
    [SerializeField] private float initPosX = 0;


    private bool isAlive = true;
    private bool isMaximize = false;
    private bool isInChange = false;
    private float jumpPower;
    private float recoverySpeed;
    private float speedRate;
    private Vector3 cameraBasePos;
    private float sizeProgress;

    private int jumpCount;
    private const int MaxJumpCount = 2;
    
    
    //画面外判定について
    private Rect cameraRect = new Rect(-0.1f,-0.1f,1.1f,2f);

    private Sequence seq;

    private Rigidbody2D rigidbody;
    private Renderer renderer;

    private void Jump()
    {
        if(TouchLayer("Ground", Vector2.down))
        {
            jumpCount = MaxJumpCount;
        }

        if (jumpCount > 0)
        {
            jumpCount--;
            var vel = rigidbody.velocity;
            vel.y = 0;
            rigidbody.velocity = vel;
            SoundManager.SingletonInstance.PlaySE("jump", false, 0.3f);
            rigidbody.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void Maximize()
    {
        SoundManager.SingletonInstance.PlaySE("nobiru", false, 0.3f);
        isInChange = true;
        ChangeSize(1);
        isMaximize = true;
        inputUI.Swap(isMaximize);
    }

    private void Minimize()
    {
        SoundManager.SingletonInstance.PlaySE("tidimu", false, 0.3f);
        isInChange = true;
        ChangeSize(0);
        isMaximize = false;
        inputUI.Swap(isMaximize);
    }

    private bool TouchLayer(string layerName, Vector2 direction, float margin = 0.1f)
    {
        int layerNo = LayerMask.NameToLayer(layerName);
        int layerMask = 1 << layerNo;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, transform.localScale.x * 0.5f + margin, layerMask);
        if (hit.collider)
        {
            return true;
        }

        return false;
    }
    private void ChangeSize(float target)
    {
        if (seq != null)
        {
            seq.Kill();
        }
        seq = DOTween.Sequence().Append(DOVirtual.Float(sizeProgress, target, transitionTime, value =>
        {
            sizeProgress = value;
            speedRate = minSpeedRate + (maxSpeedRate - minSpeedRate) * sizeProgress;
            gsab.SetSpeedRate(speedRate);
            rigidbody.gravityScale = minGravityScale + (maxGravityScale - minGravityScale) * sizeProgress;
            transform.localScale = Vector3.one * (1 + (maxScale - 1) * sizeProgress);
            recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
            Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
            jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;

            cameraBasePos = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
        })).OnComplete(() => { isInChange = false;});
    }
    
    private void GameOver()
    {
        SoundManager.SingletonInstance.PlaySE("gameover", false, 0.3f);
        gsab.SetSpeedRate(0);
        if (!isAlive) return;

        if (seq != null)
        {
            seq.Kill();
        }
        isAlive = false;
        Instantiate(gameOverText);
        Invoke("GoToResultScene", 3f);
    }

    private void GoToResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();

        jumpCount = MaxJumpCount;
        
        sizeProgress = isMaximize ? 1 : 0;
        speedRate = minSpeedRate + (maxSpeedRate - minSpeedRate) * sizeProgress;
        gsab.SetSpeedRate(speedRate);
        recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
        jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;
        Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
        rigidbody.gravityScale = minGravityScale + (maxGravityScale - minGravityScale) * sizeProgress;
        cameraBasePos = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
    }

    private void Start()
    {
        SoundManager.SingletonInstance.PlayBGM("playBGM", false, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (isMaximize)
                {
                    Minimize();
                }
                else
                {
                    Maximize();
                }
            }

            if (!TouchLayer("Ground", Vector2.right, 0.3f))
            {
                var velocity = rigidbody.velocity;
                velocity.x = 0;
                rigidbody.velocity = velocity;

                if (initPosX - transform.position.x > 0)
                {
                    transform.position += recoverySpeed * Time.deltaTime * Vector3.right;
                }
            }

            //巨大化中に壁にぶつかったら縮む
            if (isMaximize && isInChange)
            {
                if (TouchLayer("Ground", Vector2.up, 0.3f))
                {
                    Minimize();
                }
            }
            
            //カメラ位置
            var nextCameraPos = cameraBasePos;
            if (transform.position.y > cameraBasePos.y)
            {
                nextCameraPos.y = transform.position.y;
            }

            Camera.main.transform.position = nextCameraPos;

            //画面外判定
            var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (!cameraRect.Contains(viewportPos))
            {
                GameOver();
            }
        }
    }
    
}
