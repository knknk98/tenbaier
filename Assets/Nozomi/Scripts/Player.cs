using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
    
    [SerializeField] private float maxScale = 10f;
    [SerializeField] private float transitionTime;
    [SerializeField] private float initPosX = 0;


    private bool isAlive = true;
    private bool isMaximize = false;
    private float jumpPower;
    private float recoverySpeed;
    private float sizeProgress;
    
    //画面外判定について
    private Rect cameraRect = new Rect(-0.1f,-0.1f,1.1f,2f);

    private Sequence seq;

    private Rigidbody2D rigidbody;
    private Renderer renderer;

    private void Jump()
    {
        if(TouchLayer("Ground", Vector2.down))
        {
            rigidbody.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void Maximize()
    {
        ChangeSize(1);
        isMaximize = true;
    }

    private void Minimize()
    {
        ChangeSize(0);
        isMaximize = false;
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
            rigidbody.gravityScale = minGravityScale + (maxGravityScale - minGravityScale) * sizeProgress;
            transform.localScale = Vector3.one * (1 + (maxScale - 1) * sizeProgress);
            recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
            Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
            jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;

            Camera.main.transform.position = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
        }));
    }
    
    private void GameOver()
    {
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
        
        sizeProgress = isMaximize ? 1 : 0;
        recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
        jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;
        Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
        rigidbody.gravityScale = minGravityScale + (maxGravityScale - minGravityScale) * sizeProgress;
        Camera.main.transform.position = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
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
            

            //画面外判定
            var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (!cameraRect.Contains(viewportPos))
            {
                GameOver();
            }
        }
    }
    
}
