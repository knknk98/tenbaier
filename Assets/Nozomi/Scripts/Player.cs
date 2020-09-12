using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxJumpPower;
    [SerializeField] private float minJumpPower;
    
    [SerializeField] private float maxCameraSize;
    [SerializeField] private float minCameraSize;
    
    [SerializeField] private float maxRecoverySpeed;
    [SerializeField] private float minRecoverySpeed;
    
    [SerializeField] private float maxScale = 10f;
    [SerializeField] private float transitionTime;
    [SerializeField] private float initPosX = 0;


    private bool isAlive = true;
    private bool isMaximize = false;
    private bool isGrounded = true;
    private float jumpPower;
    private float recoverySpeed;
    private float sizeProgress;
    
    //画面外判定について
    private Rect cameraRect = new Rect(0,0,1,1);

    private Sequence seq;

    private Rigidbody2D rigidbody;
    private Renderer renderer;

    private void Jump()
    {
        if (!isGrounded) return;

        isGrounded = false;
        rigidbody.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
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

    private void ChangeSize(float target)
    {
        if (seq != null)
        {
            seq.Kill();
        }
        seq = DOTween.Sequence().Append(DOVirtual.Float(sizeProgress, target, transitionTime, value =>
        {
            sizeProgress = value;
            rigidbody.gravityScale = 1 + (maxScale - 1) * sizeProgress;
            transform.localScale = Vector3.one * (1 + (maxScale - 1) * sizeProgress);
            recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
            Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
            jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;
        }));
    }
    
    private void GameOver()
    {
        isAlive = false;
        Debug.Log("DIE...");
    }
    

    private void Awake()
    {
        sizeProgress = isMaximize ? 1 : 0;
        recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
        jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;
        Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
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

            if (initPosX - transform.position.x > 0)
            {
                transform.position += recoverySpeed * Time.deltaTime * Vector3.right;
            }

            //画面外判定
            var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (!cameraRect.Contains(viewportPos))
            {
                GameOver();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == "Ground")
        {
            isGrounded = true;
        }
    }
}
