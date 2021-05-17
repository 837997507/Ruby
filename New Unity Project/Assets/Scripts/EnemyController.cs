using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float changeTime = 3.0f;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _origianlPos;
    private Vector3 _targetPos;
    private Vector2 _currentPos;
    private float _timer = 0;
    private float _randomValue;
    private float _dst;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _origianlPos = transform.position;
    }

    private void Update()
    {
        if (_timer <= 0)
        {
            _randomValue = Random.Range(-1.0f, 1.0f);
            _timer = changeTime;
            
            _targetPos = new Vector3(_origianlPos.x + _randomValue * speed, _origianlPos.y + _randomValue * speed, transform.position.z);
            _dst = Vector3.Distance(_currentPos, _targetPos);
        }
        _timer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        _currentPos = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * speed / _dst);
        _rigidbody2D.MovePosition(_currentPos);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
