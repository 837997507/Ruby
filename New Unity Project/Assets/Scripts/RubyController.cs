using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public int health { get { return _currentHealth; }}
    
    private int _currentHealth;
    private float _horizontal; 
    private float _vertical;
    private Rigidbody2D _rigidbody2d;
    private bool  _isInvincible;
    private float _invincibleTimer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
    }

    // 每帧调用一次 Update
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
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
        }
        
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        Debug.Log(_currentHealth + "/" + maxHealth);
    }
}
