using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    public int playerId = -1;

    public GameObject playerObject;
    // public float playerSpeed;

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

    private float setting_dashSpeed = 10.0f;
    private float setting_dashSlideTime = 0.5f;
    private float setting_dashSlideLength = 2.0f;
    private float setting_dashAnimationSpeed = 1.0f;
    private float setting_dashNeckExtend = 0.05f;

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

    private float playerDashCooldown;
    private float playerFireCooldown;

    enum PlayerState
    {
        eIdle, eMove, eFire, eDash,
    }

    // Use this for initialization
    void Start()
    {
        playerState = PlayerState.eIdle;

        playerHead = playerObject.transform.FindChild("Head").gameObject;
        playerNeckHigh = playerObject.transform.FindChild("NeckHigh").gameObject;
        playerNeckLow = playerObject.transform.FindChild("NeckLow").gameObject;
        playerBottom = playerObject.transform.FindChild("Bottom").gameObject;

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
        float playerLengthSpeed = setting_moveAnimationSpeed * jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(t) * setting_moveSpeed;

        if (t >= Mathf.PI)
        {
            playerState = PlayerState.eIdle;
            playerMoveStartTime = Time.time;
            return;
        }

        // Inside motion
        {
            float headCenter = 0.5f * (playerLength - playerSpriteLength);
            float bodyCenter = -0.5f * (playerLength - playerSpriteLength);
            float partDelta = (bodyCenter - headCenter) / 6.0f;
            float neckHighCenter = partDelta;
            float neckLowCenter = -0.8f * partDelta;

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



    /*void AnimateDash()
    {
        float jerkPower = 16.0f;
        float dashSlideTime = 0.5f;

        float t = (Time.time - playerMoveStartTime) * setting_dashAnimationSpeed;
        float playerLength = playerInitialLength * (1.0f + setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower));
        float playerLengthSpeed = jerkPower * playerInitialLength * setting_moveNeckExtend * Mathf.Pow(Mathf.Sin(t), jerkPower - 1.0f) * Mathf.Cos(t) * setting_moveSpeed;

        if (t >= Mathf.PI)
        {
            playerState = PlayerState.eIdle;
            playerMoveStartTime = Time.time;
            return;
        }

        // Inside motion
        {
            float headCenter = 0.5f * (playerLength - playerSpriteLength);
            float bodyCenter = -0.5f * (playerLength - playerSpriteLength);
            float partDelta = (bodyCenter - headCenter) / 6.0f;
            float neckHighCenter = partDelta;
            float neckLowCenter = -0.8f * partDelta;

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
    }*/



    PlayerState ResolveState()
    {

        // Idle state management
        if (playerState == PlayerState.eIdle)
        {
            if (playerFireAsked.z != 0.0f)
            {
                playerState = PlayerState.eFire;
                playerFireStartTime = Time.time;
            }
            else if (playerDashAsked.z != 0.0f)
            {
                playerState = PlayerState.eDash;
                playerDashStartTime = Time.time;
            }
            else if (playerMoveAsked.z != 0.0f)
            {
                playerState = PlayerState.eMove;
                playerMoveStartTime = Time.time;
            }
        }

        if (playerState == PlayerState.eFire)
        {
            AnimateMove();
        }

        if (playerState == PlayerState.eDash)
        {
            AnimateMove();
        }

        if (playerState == PlayerState.eMove)
        {
            AnimateMove();
        }

        // Cooldown solving
        if (playerState != PlayerState.eFire &&
            playerFireAsked.z != 0.0f &&
            (Time.time - playerFireStartTime) >= playerFireCooldown)
        {
            playerFireAsked.z = 0.0f;
        }

        if (playerState != PlayerState.eDash &&
            playerDashAsked.z != 0.0f &&
            (Time.time - playerDashStartTime) >= playerDashCooldown)
        {
            playerDashAsked.z = 0.0f;
        }

        if (playerState != PlayerState.eMove &&
            playerMoveAsked.z != 0.0f)
        {
            playerMoveAsked.z = 0.0f;
        }

        return playerState;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 motion = new Vector3(moveHorizontal, moveVertical, 0.0f);
        bool isMotion = (motion.x != 0 || motion.y != 0);

        if (Input.GetButton("Jump") && (playerDashAsked.z == 0.0f))
            playerDashAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);
        else if (Input.GetButton("Fire1") && (playerFireAsked.z == 0.0f))
            playerFireAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);
        else if (isMotion && playerMoveAsked.z == 0.0f)
            playerMoveAsked = motion + new Vector3(0.0f, 0.0f, 1.0f);


        ResolveState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // TODO : store gm in private member and fill it in Start()
        Debug.Log("OnTriggerEnter");
        gameManager gm = FindObjectOfType<gameManager>();
        if (other.gameObject.CompareTag("Apple"))
        {
            gm.Collect(other.gameObject, playerId);
        }
    }
}
