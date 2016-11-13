using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{

    public GameObject playerObject;
    public float playerSpeed;

    private GameObject playerHead;
    private GameObject playerNeckHigh;
    private GameObject playerNeckLow;
    private GameObject playerBottom;

    private float playerLength;
    private float playerInitialLength;
    private float playerSpriteLength;
    private float playerGrowingSpeed;

    private bool playerMoving;
    private float playerMovingAngle;
    private float playerMovingStartTime;

    // Use this for initialization
    void Start()
    {
        // playerSpeed = 2.0f;


        playerHead = playerObject.transform.FindChild("Head").gameObject;
        playerNeckHigh = playerObject.transform.FindChild("NeckHigh").gameObject;
        playerNeckLow = playerObject.transform.FindChild("NeckLow").gameObject;
        playerBottom = playerObject.transform.FindChild("Bottom").gameObject;

        BoxCollider2D playerCollider = playerObject.GetComponent<BoxCollider2D>();
        Vector3 playerColliderSize = playerCollider.bounds.size;
        playerLength = playerColliderSize.x;
        playerInitialLength = playerLength;

        playerMoving = false;
//         playerCollider.bounds.min;
//         playerCollider.bounds.max;

        SpriteRenderer playerHeadSprite = playerHead.GetComponent<SpriteRenderer>();
        Vector3 playerPartSize = playerHeadSprite.bounds.size;
        playerSpriteLength = playerPartSize.x;

        float playerPartScaleRatio = playerColliderSize.x / playerPartSize.x;

        Debug.Log("Before " + playerHeadSprite.bounds.size.ToString());

        playerObject.transform.localScale = new Vector3(playerPartScaleRatio, playerPartScaleRatio, 1.0f);

        Debug.Log("After " + playerHeadSprite.bounds.size.ToString());
        playerSpriteLength = playerHeadSprite.bounds.size.x;
    }

    void SetupPlayer()
    {
        if (!playerMoving)
            return;

        float speed = 10.0f;
        float t = (Time.time - playerMovingStartTime) * speed;
        float neckExtend = 0.05f;
        playerLength = playerInitialLength * (1.0f + neckExtend * (1.0f + Mathf.Sin(t)));

        float newPlayerGrowingSpeed = 0.5f * playerInitialLength * neckExtend * Mathf.Cos(t) * Time.deltaTime * speed * 1.5f * playerSpeed;
        if (newPlayerGrowingSpeed > 0 && playerGrowingSpeed <= 0)
        {
            playerGrowingSpeed = newPlayerGrowingSpeed;
            playerMoving = false;
            return;
        }

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


        // float newPlayerGrowingSpeed = 0.5f * playerInitialLength * neckExtend * Mathf.Cos(t) * Time.deltaTime * speed * 1.5f * playerSpeed;
        // if (newPlayerGrowingSpeed < 0.0f && playerGrowingSpeed >= 0.0f)
        //     playerPosition += new Vector2(bodyCenter, 0.0f);
        playerGrowingSpeed = newPlayerGrowingSpeed;

        // if (playerGrowingSpeed >= 0)
        //     playerObject.transform.position = new Vector3(-bodyCenter, 0.0f);
        // else

        playerObject.transform.position += new Vector3(Mathf.Cos(playerMovingAngle), Mathf.Sin(playerMovingAngle)) * Mathf.Abs(playerGrowingSpeed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        float angle = Mathf.Atan2(movement.y, movement.x);

        if (!playerMoving && (movement.x != 0 || movement.y != 0))
        {
            playerMoving = true;
            playerMovingStartTime = Time.time;
            playerGrowingSpeed = 1.0f;

            playerMovingAngle = angle;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0.0f, 0.0f, playerMovingAngle * Mathf.Rad2Deg);
            playerObject.transform.rotation = rotation;
        }

        // playerObject.transform.position += movement;

        SetupPlayer();

        if (movement.x != 0 || movement.y != 0)
        {
            if (!playerMoving)
            {
                playerMovingAngle = angle;
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0.0f, 0.0f, playerMovingAngle * Mathf.Rad2Deg);
                playerObject.transform.rotation = rotation;
            }

            playerMoving = true;
        }
    }
}
