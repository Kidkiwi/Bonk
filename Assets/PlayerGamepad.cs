using UnityEngine;

public class PlayerGamepad : MonoBehaviour
{
    [SerializeField] float jumpTime = 0.35f;

    public float _UpForceWhenGoingUp = 110f;
    public float _UpForceWhenGoingDown = 75f;
    public float _XForce = 10f;
    public float _DownForce = 10f;
    public float _BigJumpForce = 200f;
    public float _TimeOnGroundBeforeBigJump = 1f;

    [SerializeField] LayerMask _WhatIsGround;
    [SerializeField] Transform _FeetPos;
    [SerializeField] float _GroundRadius;

    [SerializeField] VariableJoystick _Joystick;

    PlayerControls controls;

    Rigidbody _Rigidbody;
    public bool _Grounded = true;
    public UnityEngine.UI.Toggle _Toggle;

    Vector3 force = Vector3.zero;
    bool isJumping = false;
    float groundedTime = 0f;
    float jumpTimeCounter = 0f;
    bool wasGrounded = false;

    Vector2 move;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        controls = new PlayerControls();
        _Rigidbody = GetComponent<Rigidbody>();

        _Toggle.isOn = GameSettings._Gamepad;
        _Toggle.onValueChanged.AddListener((value) =>
        {
            GameSettings._Gamepad = value;
            Debug.Log(GameSettings._Gamepad);
            if (GameSettings._Gamepad)
            {
                controls.Enable();
                _Joystick.gameObject.SetActive(false);
            }
            else
            {
                controls.Disable();
                _Joystick.gameObject.SetActive(true);
            }
        });

        isJumping = false;
        jumpTimeCounter = jumpTime;
        count = 0;

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        if (GameSettings._Gamepad)
        {
            controls.Enable();
        }
    }

    private void OnDisable()
    {
        if (!GameSettings._Gamepad)
        {
            controls.Disable();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

//        if (_Rigidbody.velocity.y < 0f)
        {
            Gizmos.DrawWireSphere(_FeetPos.position, _GroundRadius);
        }
    }

    private void FixedUpdate()
    {
        _Grounded = Physics.OverlapSphere(_FeetPos.position, _GroundRadius, _WhatIsGround).Length > 0;

        if (_Grounded)
            wasGrounded = true;

        if (_Grounded && isJumping)
        {
            isJumping = false;
            count = 0;

            Debug.Log("<color=#f00>GROUNDED</color>");
        }

        if( move.y <= 0f && wasGrounded )
        {
            jumpTimeCounter = jumpTime;
        }
        
        if( !_Grounded )
            groundedTime = 0f;

        if (force != Vector3.zero)
        {
            _Rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        if( move.y > 0f )
        {
            wasGrounded = false;

            float maxforce = 100f;

            if (_Rigidbody.velocity.y < 0f)
                maxforce = 75f;
            else
                maxforce = 110f;

            _Rigidbody.AddForce(Vector3.up * Mathf.Max(5f, jumpTimeCounter * maxforce) * Mathf.Abs(move.y), ForceMode.Acceleration);
        }

        if ( _Grounded && move.y > 0f && groundedTime > _TimeOnGroundBeforeBigJump)
        {
            Debug.Log("Adding jump force");
            _Rigidbody.AddForce(Vector3.up * _BigJumpForce, ForceMode.Acceleration);
            _Grounded = false;
            isJumping = true;
        }

        if (move.y > 0f && jumpTimeCounter > 0f)
        {
        //    _Grounded = false;
        //    isJumping = true;
        //    Debug.Log("C: " + count);
        //    ++count;
            jumpTimeCounter -= Time.fixedDeltaTime * 2f;
        }
    }

    int count = 0;
    private void Update()
    {
        if (!GameSettings._Gamepad)
        {
            move = _Joystick.Direction;
        }

        force.x = _XForce * move.x;

        if (move.y < 0f)
        {
            force.y = _DownForce * move.y;
        }

        if (_Grounded)
            groundedTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision c)
    {
        switch (c.collider.tag)
        {
            case "Finish":
                if (_Grounded)
                {
                    _Rigidbody.AddForce(c.contacts[0].normal.normalized * c.collider.GetComponent<BouncePlatform>()._BounceForce, ForceMode.Acceleration);
                }

                break;
        }
    }
}