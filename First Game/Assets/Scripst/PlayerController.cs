using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Definimos una variable de tipo float que determinara la velocidad
    public float speed = 2.5f;
    public float jumpForce = 2.5f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    // Creamos dos varibles uno de tipo Rigidbody2D y otra de tipo Animator 
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // Creamos una variable de tipo vector que determinara el movimiento
    private Vector2 _movement;
    private bool _facingRight = true;
    private bool _isGrounded;

    // Atacar
    private bool _isAttacking;

    private void Awake()
    {
        // Obtenemos los componenetes Rigidbody2D y Animator
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento 
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        _movement = new Vector2(horizontalInput, 0f);

        // Flip
        if (horizontalInput < 0f && _facingRight == true)
        {
            Flip();
        }
        else if (horizontalInput > 0f && _facingRight == false)
        {
            Flip();
        }

        //Esta suelo??
       _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //Debug.Log("Salto");
        //Esta saltando??
        if (Input.GetButtonDown("Jump") && _isGrounded == false)
        {
            //Debug.Log("Salto");
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Quiero Atacar??
        if(Input.GetButtonDown("Fire1") && _isGrounded == false && _isAttacking == false)
        {
            //Debug.Log("Ataco");
            _movement = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        float horizontalVelocity = _movement.normalized.x * speed;
        _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);

    }

    private void LateUpdate()
    {
        _animator.SetBool("Idle", _movement == Vector2.zero);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VerticalVelocity", _rigidbody.velocity.y);

        // Animator 
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _isAttacking = true;

        }
        else
        {
            _isAttacking = false;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        float localScaleX = transform.localScale.x;

        localScaleX = localScaleX * -1f;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

    }
}
