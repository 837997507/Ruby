using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool  vertical;
    public float changeTime = 3.0f;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private bool _isBroken = true;
    
    float       timer;
    int         direction = 1;
    
    // 在第一次帧更新之前调用 Start
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        timer       = changeTime;
    }

    void Update()
    {
        if (!_isBroken)
        {
            return;;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer     = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        if (!_isBroken)
        {
            return;;
        }
        
        Vector2 position = _rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            _animator.SetFloat("MoveX", 0);
            _animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            _animator.SetFloat("MoveX", direction);
            _animator.SetFloat("MoveY", 0);
        }
        
        _rigidbody2D.MovePosition(position);
    }

    public void Fix()
    {
        _isBroken               = false;
        _rigidbody2D.simulated  = false;
        _animator.SetTrigger("Fixed");
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}