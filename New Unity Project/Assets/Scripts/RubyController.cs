using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public int health { get { return _currentHealth; }}
    
    /// <summary>
    /// 飞弹预制件
    /// </summary>
    public GameObject projectilePrefab;
    
    
    private int _currentHealth;
    private float _horizontal; 
    private float _vertical;
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private bool  _isInvincible;
    private float _invincibleTimer;
    private Vector2 _lookDirection = Vector2.one;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
    }

    // 每帧调用一次 Update
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
        
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        var move = new Vector2(_horizontal,_vertical);

        if (!Mathf.Approximately(move.x,0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            _lookDirection.Set(move.x,move.y);
            _lookDirection.Normalize();
        }
        
        _animator.SetFloat("Look X", _lookDirection.x);
        _animator.SetFloat("Look Y", _lookDirection.y);
        _animator.SetFloat("Speed",  move.magnitude);
        
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
                _isInvincible = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = _rigidbody2d.position;
        position.x = position.x + speed * _horizontal * Time.deltaTime;
        position.y = position.y + speed * _vertical * Time.deltaTime;

        _rigidbody2d.MovePosition(position);
    }
    
    public void  ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (_isInvincible)
                return;
            
            _isInvincible = true;
            _invincibleTimer = timeInvincible;
            _animator.SetTrigger("Hit");
        }
        
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        Debug.Log(_currentHealth + "/" + maxHealth);
    }

    public void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
        
        _animator.SetTrigger("Launch");
    }
}
