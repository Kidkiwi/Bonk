using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _Force = 10f;
    [SerializeField] float _UpForce = 50f;
    [SerializeField] float _MinJumpForce = 30f;
    [SerializeField] float jumpTime = 0.35f;

    [SerializeField] LayerMask _WhatIsGround;
    [SerializeField] Transform _FeetPos;
    [SerializeField] float _GroundRadius;

    Rigidbody _Rigidbody;
    public bool _Grounded = true;

    Vector3 force = Vector3.zero;
    bool isJumping = false;
    float jumpTimeCounter = 0f;
    bool _Jumped = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_FeetPos.position, _GroundRadius);
    }

    private void FixedUpdate()
    {
        if (force != Vector3.zero)
        {
            _Rigidbody.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        _Grounded = Physics.OverlapSphere(_FeetPos.position, _GroundRadius, _WhatIsGround).Length > 0;

        if (_Grounded)
            _Jumped = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            force = Vector3.left * _Force;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            force = Vector3.right * _Force;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !_Jumped)
        {
            //Vector3 v = _Rigidbody.velocity;
            //v.y = _MinJumpForce;
            //_Rigidbody.velocity = v;

            Debug.Log("INIT JUMP");
            _Rigidbody.AddForce(Vector3.up * _MinJumpForce, ForceMode.Acceleration);

            isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        if( Input.GetKey(KeyCode.UpArrow) && !_Jumped)
        {
            //if( jumpTimeCounter > 0 )
            {
                Debug.Log("ADDING FORCE");
                _Rigidbody.AddForce(Vector3.up * Mathf.Max(3f, _UpForce * jumpTimeCounter), ForceMode.Acceleration);
                jumpTimeCounter -= Time.deltaTime;
            }
            //else
            //{
            //    _Jumped = true;
            //    isJumping = false;
            //}
        }

        if( Input.GetKeyUp(KeyCode.UpArrow) )
        {
            _Jumped = true;
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            force = Vector3.down * (_Force/2f);
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            force = Vector3.zero;
        }
    }

    void Grounded()
    {
        if (_Rigidbody.velocity.y > 0f)
        {
            _Grounded = false;
            return;
        }

        Debug.DrawRay(transform.position - new Vector3(0f, .25f, 0f), Vector3.down * .25f, Color.blue, 1f);

        RaycastHit r;
        bool g = Physics.Raycast(transform.position - new Vector3(0f, .25f, 0f), Vector3.down, out r, .25f);

        if (!g)
        {
            Debug.DrawRay(transform.position - new Vector3(.5f, .25f, 0f), Vector3.down * .25f, Color.blue, 1f);
            g = Physics.Raycast(transform.position - new Vector3(.5f, .25f, 0f), Vector3.down, out r, .25f);
        }

        if (!g)
        {
            Debug.DrawRay(transform.position + new Vector3(.5f, -.25f, 0f), Vector3.down * .25f, Color.blue, 1f);
            g = Physics.Raycast(transform.position + new Vector3(.5f, -.25f, 0f), Vector3.down, out r, .25f);
        }

        _Grounded = g;
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