using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private bool isGameOver;
    [SerializeField] private GameObject gameOverTextSimple;
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

    [SerializeField] private float maxShotPower;
    [SerializeField] private float minShotPower;
    [SerializeField] private float rotPower;


    private bool isAlive = true;
    private bool isAbleToBeatClerk = false;
    private bool isAbleToBreakGrass = false;
    private bool isMaximize = false;
    private bool isInChange = false;
    private float jumpPower;
    private float recoverySpeed;
    private float speedRate;

    private Vector3 cameraBasePos;

    private float shotPower;

    private float sizeProgress;

    private int jumpCount;
    private const int MaxJumpCount = 3;

    //ジャンプしたあとに地面に着地した際にジャンプカウントをリセットする
    private bool isJumpCountReseted = true;

    //画面外判定について
    private Rect cameraRect = new Rect(-0.1f, -0.1f, 1.1f, 2f);

    private Sequence seq;

    private Rigidbody2D rigidbody;
    private Renderer renderer;

    private void Jump()
    {
        if (TouchLayer("Ground", Vector2.down))
        {
            jumpCount = MaxJumpCount;
        }

        if (TouchLayer("Grass", Vector2.down) && !isAbleToBreakGrass)
        {
            jumpCount = MaxJumpCount;
        }

        if (jumpCount < MaxJumpCount)
        {
            isJumpCountReseted = false;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, transform.localScale.x * 0.5f + margin,
            layerMask);
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
            if (target > 0.5f)
            {
                if (value > 0.5f)
                {
                    isAbleToBreakGrass = true;
                }

                if (value > 0.25f)
                {
                    isAbleToBeatClerk = true;
                }
            }
            else
            {
                if (value < 0.5f)
                {
                    isAbleToBreakGrass = false;
                }

                if (value < 0.25f)
                {
                    isAbleToBeatClerk = false;
                }
                else
                {
                    isAbleToBeatClerk = true;
                }
            }

            sizeProgress = value;
            speedRate = minSpeedRate + (maxSpeedRate - minSpeedRate) * sizeProgress;
            gsab.SetSpeedRate(speedRate);
            rigidbody.gravityScale = minGravityScale + (maxGravityScale - minGravityScale) * sizeProgress;
            transform.localScale = Vector3.one * (1 + (maxScale - 1) * sizeProgress);
            recoverySpeed = minRecoverySpeed + (maxRecoverySpeed - minRecoverySpeed) * sizeProgress;
            Camera.main.orthographicSize = minCameraSize + (maxCameraSize - minCameraSize) * sizeProgress;
            jumpPower = minJumpPower + (maxJumpPower - minJumpPower) * sizeProgress;
            shotPower = minShotPower + (maxShotPower - minShotPower) * sizeProgress;
            cameraBasePos = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
        })).OnComplete(() => { isInChange = false; });
    }

    private void GameOver()
    {
        isGameOver = true;
        /*
        var itemList = ScoreManager.SingletonInstance.GetItemList();
        foreach (var item in itemList)
        {
            Debug.Log("name:" + item.name);
            Debug.Log("price:" + item.price);
            Debug.Log("count:" + item.count);
        }
        */

        Die(gameObject, new Vector2(-0.41f, 0.41f), shotPower, rotPower, false);
        SoundManager.SingletonInstance.StopBGM();
        SoundManager.SingletonInstance.PlaySE("gameover", false, 0.3f);
        gsab.SetSpeedRate(0);
        if (!isAlive) return;

        if (seq != null)
        {
            seq.Kill();
        }

        isAlive = false;
        //Instantiate(gameOverText);
        Invoke("GoToResultScene", 3f);
    }

    private void GoToResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

    private void Die(GameObject target, Vector2 direction, float power, float torque, bool destroy = true)
    {
        var col = target.GetComponent<Collider2D>();
        Destroy(col);
        var rig = target.GetComponent<Rigidbody2D>();
        if (!rig)
        {
            rig = target.AddComponent<Rigidbody2D>();
        }

        rig.constraints = RigidbodyConstraints2D.None;
        rig.velocity = Vector2.zero;
        rig.AddForce(power * direction, ForceMode2D.Impulse);
        if (destroy)
        {
            DOVirtual.DelayedCall(2f, () => { Destroy(target); });
        }

        rig.AddTorque(torque, ForceMode2D.Impulse);
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

        shotPower = minShotPower + (maxShotPower - minJumpPower) * sizeProgress;
        Camera.main.transform.position = Vector3.Lerp(minCameraPos, maxCameraPos, sizeProgress);
    }

    private void Start()
    {
        SoundManager.SingletonInstance.PlayBGM("playBGM", true, 0.3f);
        ScoreManager.SingletonInstance.InitScore();
        Camera.main.orthographicSize = minCameraSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            //gameOverTextSimple.transform.position = (gameOverTextSimple.transform.position + new Vector3(0f, Screen.height/2f, 0f));
            gameOverTextSimple.transform.localPosition = Vector3.Lerp(gameOverTextSimple.transform.localPosition,
                new Vector3(0, 0, 0), 0.03f);
        }

        if (!isJumpCountReseted && TouchLayer("Ground", Vector2.down))
        {
            isJumpCountReseted = true;
            jumpCount = MaxJumpCount;
        }

        if (!isJumpCountReseted && TouchLayer("Grass", Vector2.down) && !isAbleToBreakGrass)
        {
            isJumpCountReseted = true;
            jumpCount = MaxJumpCount;
        }

        if (isAlive)
        {
            TouchInfo info = AppUtil.GetTouch();
            bool isJumpButton = false;
            bool isChangeSizeButton = false;
            // if (info == TouchInfo.Began)
            // {
            //     Vector3 vector3 = AppUtil.GetTouchPosition();
            //     Debug.Log(vector3);
            //     if (vector3.x < Screen.width / 2f)
            //     {
            //         isJumpButton = true;
            //     }
            //     else
            //     {
            //         isChangeSizeButton = true;
            //     }
            // }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(Input.touchCount-1);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < Screen.width / 2f)
                    {
                        isJumpButton = true;
                    }
                    else
                    {
                        isChangeSizeButton = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || isJumpButton)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Return) || isChangeSizeButton)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == "Trap")
        {
            SoundManager.SingletonInstance.PlaySE("damage", false, 0.3f);
            GameOver();
        }

        switch (layerName)
        {
            case "Trap":
                SoundManager.SingletonInstance.PlaySE("damage", false, 0.3f);
                GameOver();
                break;

            case "Clerk":
                if (isAbleToBeatClerk)
                {
                    SoundManager.SingletonInstance.PlaySE("damage", false, 0.3f);
                    Die(other.gameObject, new Vector2(0.5f, 0.5f), minShotPower, -rotPower, true);
                }
                else
                {
                    SoundManager.SingletonInstance.PlaySE("damage", false, 0.3f);
                    GameOver();
                }

                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == "Grass")
        {
            if (isAbleToBreakGrass)
            {
                SoundManager.SingletonInstance.PlaySE("grass", false, 0.3f);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == "Grass")
        {
            if (isAbleToBreakGrass)
            {
                SoundManager.SingletonInstance.PlaySE("grass", false, 0.3f);
                Destroy(other.gameObject);
            }
        }
    }
}