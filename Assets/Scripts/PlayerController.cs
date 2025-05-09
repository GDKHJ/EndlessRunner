using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private Vector3 moveVector;
    private float vertical_velocity = 0.0f;
    private float gravity = 12.0f;

    [Header("이동 및 점프")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump = 3.0f;
    [SerializeField] private float maxSpeed = 30f;
    private int currentLane = 1;
    private float laneOffset = 1f;

    [Header("무적/데미지")]
    public int maxHp = 3;
    public int currentHp;
    public bool isDead = false;
    public bool isInvincible = false;
    public float invincibleTime = 2f;
    private float invincibleTimer = 0f;

    [Header("HP UI")]
    public GameObject[] hpIcons;

    [Header("렌더러")]
    public Renderer[] renderers;

    [Header("점프 상태")]
    public float jumpResetDelay = 0.5f;
    private float jumpTimer = 0f;

    [Header("ScoreManager")]
    public ScoreManager scoreManager;

    [Header("Ability (궁극기)")]
    public bool isAbilityActive = false;
    public float abilityDuration = 5f;
    private float abilityTimer = 0f;
    public float abilityChargeTime = 10f;
    private float abilityCharge = 0f;

    [Header("Ability UI")]
    public Image ultBar;

    private Color[] originalColors;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
            else
                originalColors[i] = Color.white; // fallback
        }

        UpdateHpUI();
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad < 4f || isDead) return;

        HandleInput();
        ApplyMovement();
        UpdateJumpState();
        UpdateInvincibility();
        UpdateAbility();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentLane = Mathf.Max(0, currentLane - 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            currentLane = Mathf.Min(2, currentLane + 1);

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_velocity = jump;
            animator.SetInteger("JumpState", 1);
            jumpTimer = jumpResetDelay;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && abilityCharge >= abilityChargeTime && !isAbilityActive)
        {
            ActivateAbility();
        }
    }

    private void ApplyMovement()
    {
        moveVector = Vector3.zero;

        float targetX = (currentLane - 1) * laneOffset;
        float deltaX = targetX - transform.position.x;

        moveVector.x = deltaX * 10f;
        moveVector.y = vertical_velocity;
        moveVector.z = speed;

        if (!controller.isGrounded)
            vertical_velocity -= gravity * Time.deltaTime;
        else if (vertical_velocity < 0)
            vertical_velocity = -0.5f;

        controller.Move(moveVector * Time.deltaTime);
    }

    private void UpdateJumpState()
    {
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                animator.SetInteger("JumpState", 0);
            }
        }
    }

    private void UpdateInvincibility()
    {
        if (isInvincible && !isAbilityActive)
        {
            invincibleTimer -= Time.deltaTime;

            float blink = Mathf.PingPong(Time.time * 10f, 1f) > 0.5f ? 1f : 0f;
            foreach (Renderer r in renderers)
                r.enabled = blink > 0;

            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                foreach (Renderer r in renderers)
                    r.enabled = true;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"피해를 입었습니다! 현재 체력: {currentHp}");
        UpdateHpUI();

        if (currentHp <= 0)
        {
            OnDeath();
            scoreManager.OnDead();
        }
        else
        {
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }
    }

    private void OnDeath()
    {
        isDead = true;
    }

    public void SetSpeed(float level)
    {
        speed += level;
        if (speed > maxSpeed) speed = maxSpeed;
        Debug.Log($"현재 스피드 : {speed}");
    }

    public float GetSpeed() => speed;

    private void UpdateAbility()
    {
        if (!isAbilityActive)
        {
            if (abilityCharge < abilityChargeTime)
            {
                abilityCharge += Time.deltaTime;
                ultBar.fillAmount = abilityCharge / abilityChargeTime;
            }
        }
        else
        {
            abilityTimer -= Time.deltaTime;
            ultBar.fillAmount = abilityTimer / abilityDuration;

            if (abilityTimer <= 0f)
            {
                EndAbility();
            }

            // 무지개 색 변화
            float hue = Mathf.PingPong(Time.time * 0.5f, 1f);
            Color rainbow = Color.HSVToRGB(hue, 1f, 1f);
            foreach (Renderer r in renderers)
                r.material.color = rainbow;
        }
    }

    private void ActivateAbility()
    {
        isAbilityActive = true;
        isInvincible = true;
        abilityTimer = abilityDuration;
        abilityCharge = 0f;
        Debug.Log("궁극기 발동!");
    }

    private void EndAbility()
    {
        isAbilityActive = false;
        isInvincible = false;
        Debug.Log("궁극기 종료");

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];

            renderers[i].enabled = true;
        }
    }


    private void UpdateHpUI()
    {
        for (int i = 0; i < hpIcons.Length; i++)
        {
            hpIcons[i].SetActive(i < currentHp);
        }
    }
}
