using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    public int playerId = -1;

    private GameObject playerObject;
    // public float playerSpeed;


    gameManager gm;


    private GameObject playerHead;
    private GameObject playerNeckHigh;
    private GameObject playerNeckLow;
    private GameObject playerBottom;

    private float playerInitialLength;
    private float playerSpriteLength;

    ///////////////////////////////////////////////////////////////////////////
    /// Motion Setting
    public float setting_moveSpeed = 1.0f;
    public float setting_moveAnimationSpeed = 10.0f;
    public float setting_moveNeckExtend = 0.2f;

    public float setting_dashSpeed = 1.0f;
    public float setting_dashSlideTime = 0.75f;
    public float setting_dashSlideLength = 15.0f;
    public float setting_dashAnimationSpeed = 6.0f;
    public float setting_dashNeckExtend = 0.2f;

    private float setting_fireSpeed = 10.0f;
    private float setting_fireAnimationSpeed = 1.0f;
    private float setting_fireNeckExtend = 0.05f;

    ///////////////////////////////////////////////////////////////////////
    private Vector3 playerMoveAsked;
    private Vector3 playerDashAsked;
    private Vector3 playerFireAsked;
    private PlayerState playerState;

    private float playerMoveStartTime;
    private float playerDashStartTime;
    private float playerFireStartTime;

    public float setting_playerDashCooldown = 0.5f;
    public float setting_playerFireCooldown = 0.5f;

    enum PlayerState { eIdle, eMove, eFire, eDash, };

    enum DashState { eDashUp, eDashSlide, eDashDown, };


    // Use this for initialization
    void Start()
    {
        playerState = PlayerState.eIdle;
        gm = FindObjectOfType<gameManager>();

        // GameObject playerObject = gameObject;
        playerObject = gameObject;

        playerHead = playerObject.transform.FindChild("Head").gameObject;
        playerNeckHigh = playerObject.transform.FindChild("NeckHigh").gameObject;
        playerNeckLow = playerObject.transform.FindChild("NeckLow").gameObject;
        playerBottom = playerObject.transform.FindChild("Bottom").gameObject;

        // playerNeckHigh.SetActive(false);


        BoxCollider2D playerCollider = playerObject.GetComponent<BoxCollider2D>();
        Vector3 playerColliderSize = playerCollider.bounds.size;
        playerInitialLength = playerColliderSize.x;

        SpriteRenderer playerHeadSprite = playerHead.GetComponent<SpriteRenderer>();
        Vector3 playerPartSize = playerHeadSprite.bounds.size;
        playerSpriteLength = playerPartSize.x;

        float playerPartScaleRatio = playerColliderSize.x / playerPartSize.x;
        playerObject.transform.localScale = new Vector3(playerPartScaleRatio, playerPartScaleRatio, 1.0f);
        playerSpriteLength = playerHeadSprite.bounds.size.x;
    }

    void AnimateMove()
    {
        float jerkPower = 2.0f;

        float t = (Time.time - playerMoveStartTime) * setting_moveAnimationSpeed;
        float playerLength = playerInitialLength * (1.0f + setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower));
        float playerLengthSpeed = setting_moveAnimationSpeed * jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(t);

        if (t >= Mathf.PI)
        {
            playerState = PlayerState.eIdle;
            playerMoveStartTime = Time.time;
            playerMoveAsked.z = 3.0f;
            return;
        }

        // Inside motion
        {
            float headCenter = 0.5f * (playerLength - playerSpriteLength);
            float bodyCenter = -0.5f * (playerLength - playerSpriteLength);
            float partDelta = (bodyCenter - headCenter) / 6.0f;
            float neckHighCenter = -partDelta;
//            float neckLowCenter = 0.8f * partDelta;
            float neckLowCenter = partDelta;

            playerHead.transform.localPosition = new Vector3(headCenter, 0.0f, 0.0f);
            playerNeckHigh.transform.localPosition = new Vector3(neckHighCenter, 0.0f, 0.0f);
            playerNeckLow.transform.localPosition = new Vector3(neckLowCenter, 0.0f, 0.0f);
            // playerNeckHigh.transform.localPosition = new Vector3(2.0f, 0.0f, 0.0f);
            // playerNeckLow.transform.localPosition = new Vector3(-2.0f, 0.0f, 0.0f);
            playerBottom.transform.localPosition = new Vector3(bodyCenter, 0.0f, 0.0f);
        }

        float moveAngle = Mathf.Atan2(playerMoveAsked.y, playerMoveAsked.x);
        playerObject.transform.position += new Vector3(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle)) *
                                               Mathf.Abs(playerLengthSpeed) * Time.deltaTime * setting_moveSpeed;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0.0f, 0.0f, moveAngle * Mathf.Rad2Deg);
        playerObject.transform.rotation = rotation;
    }


    void AnimateDash()
    {
        float jerkPower = 16.0f;

        float t = (Time.time - playerDashStartTime) * setting_dashAnimationSpeed;
        DashState dashState = DashState.eDashUp;

        float playerLength = playerInitialLength * (1.0f + setting_dashNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower));
        float playerLengthSpeed = setting_dashAnimationSpeed * jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(t) * setting_dashSpeed;

        if (t < Mathf.PI / 2.0f)
        {
            dashState = DashState.eDashUp;
            float revT = t;
            playerLength = playerInitialLength * (1.0f + setting_dashNeckExtend * Mathf.Pow(Mathf.Sin(revT), jerkPower));
            playerLengthSpeed = setting_dashAnimationSpeed * jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(revT) * setting_dashSpeed;
        }
        else if (t < Mathf.PI / 2.0f + setting_dashSlideTime)
        {
            dashState = DashState.eDashSlide;
            playerLength = playerInitialLength * (1.0f + setting_dashNeckExtend);
            playerLengthSpeed = setting_dashSlideLength / setting_dashSlideTime;
        }
        else if (t < Mathf.PI)
        {
            dashState = DashState.eDashDown;
            float revT = t - setting_dashSlideTime;
            playerLength = playerInitialLength * (1.0f + setting_dashNeckExtend * Mathf.Pow(Mathf.Sin(revT), jerkPower));
            playerLengthSpeed = setting_dashAnimationSpeed * jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(revT) * setting_dashSpeed;
        }
        else
        {
            playerState = PlayerState.eIdle;
            playerDashStartTime = Time.time;
            playerDashAsked.z = 3.0f;
            return;
        }

        // Inside motion
        {
            float headCenter = 0.5f * (playerLength - playerSpriteLength);
            float bodyCenter = -0.5f * (playerLength - playerSpriteLength);
            float partDelta = (bodyCenter - headCenter) / 6.0f;
            float neckHighCenter = -partDelta;
            //            float neckLowCenter = 0.8f * partDelta;
            float neckLowCenter = partDelta;

            playerHead.transform.localPosition = new Vector3(headCenter, 0.0f, 0.0f);
            playerNeckHigh.transform.localPosition = new Vector3(neckHighCenter, 0.0f, 0.0f);
            playerNeckLow.transform.localPosition = new Vector3(neckLowCenter, 0.0f, 0.0f);
            // playerNeckHigh.transform.localPosition = new Vector3(2.0f, 0.0f, 0.0f);
            // playerNeckLow.transform.localPosition = new Vector3(-2.0f, 0.0f, 0.0f);
            playerBottom.transform.localPosition = new Vector3(bodyCenter, 0.0f, 0.0f);
        }

        float moveAngle = Mathf.Atan2(playerMoveAsked.y, playerMoveAsked.x);
        playerObject.transform.position += new Vector3(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle)) *
                                               Mathf.Abs(playerLengthSpeed) * Time.deltaTime;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0.0f, 0.0f, moveAngle * Mathf.Rad2Deg);
        playerObject.transform.rotation = rotation;
    }



    PlayerState ResolveState()
    {

        // Idle state management
        if (playerState == PlayerState.eIdle)
        {
            if (playerFireAsked.z == 1.0f)
            {
                playerState = PlayerState.eFire;
                playerFireStartTime = Time.time;
                playerFireAsked.z = 2.0f;
            }
            else if (playerDashAsked.z == 1.0f)
            {
                playerState = PlayerState.eDash;
                playerDashStartTime = Time.time;
                playerDashAsked.z = 2.0f;
            }
            else if (playerMoveAsked.z == 1.0f)
            {
                playerState = PlayerState.eMove;
                playerMoveStartTime = Time.time;
                playerMoveAsked.z = 2.0f;
            }
        }

        if (playerState == PlayerState.eFire)
        {
            // AnimateMove();
            // gm.Shoot(gameObject);

            playerState = PlayerState.eIdle;
            playerFireStartTime = Time.time;
            playerFireAsked.z = 3.0f;
        }

        if (playerState == PlayerState.eDash)
        {
            AnimateDash();
        }

        if (playerState == PlayerState.eMove)
        {
            AnimateMove();
        }

        // Cooldown solving
        if (playerState != PlayerState.eFire &&
            playerFireAsked.z == 3.0f &&
            (Time.time - playerFireStartTime) >= setting_playerFireCooldown)
        {
            playerFireAsked.z = 0.0f;
        }

        if (playerState != PlayerState.eDash &&
            playerDashAsked.z == 3.0f &&
            (Time.time - playerDashStartTime) >= setting_playerDashCooldown)
        {
            playerDashAsked.z = 0.0f;
            Debug.Log(playerDashAsked.z.ToString());
        }

        if (playerState != PlayerState.eMove &&
            playerMoveAsked.z == 3.0f)
        {
            playerMoveAsked.z = 0.0f;
        }

        return playerState;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string header = "p" + playerId.ToString();

        float moveHorizontal = Input.GetAxis(header + ".Horizontal");
        float moveVertical = Input.GetAxis(header + ".Vertical");
        
        Vector3 motion = new Vector3(moveHorizontal, moveVertical, 0.0f);
        bool isMotion = (motion.x != 0 || motion.y != 0);

        if (Input.GetButton(header + ".Jump") && (playerDashAsked.z == 0.0f))
        {
            Debug.Log(header + ".Jump");
            playerDashAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);
        }

        if (Input.GetButton(header + ".Fire") && (playerFireAsked.z == 0.0f))
        {
            Debug.Log(header + ".Fire");
            playerFireAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);
        }

        if (isMotion && (playerMoveAsked.z == 0.0f))
        {
            playerMoveAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);
            Debug.Log(header + ".Move");
        }

        ResolveState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            gm.Collect(other.gameObject, playerId);
        }
    }
}
