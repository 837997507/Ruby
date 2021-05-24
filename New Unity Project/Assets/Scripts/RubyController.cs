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
    /// 预制件
    /// </summary>
    public GameObject projectilePrefab;
    public ParticleSystem cureEffect;
    public ParticleSystem hurtEffect;

    /// <summary>
    /// 资源
    /// </summary>
    public AudioClip hitAudio;
    public AudioClip launchAudio;

    /// <summary>
    /// 组件
    /// </summary>
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private AudioSource _audioSource;    
    
    private int _currentHealth;
    private float _horizontal; 
    private float _vertical;
    private bool  _isInvincible;
    private float _invincibleTimer;
    private Vector2 _lookDirection = Vector2.one;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = maxHealth;
    }

    // 每帧调用一次 Update
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody2d.position + Vector2.up * 0.2f, _lookDirection, 1.5f, LayerMask.GetMask("Npc"));
            if (hit.collider != null)
            {
                var character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
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
            var hurtEffectObj = Instantiate(hurtEffect, _rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            hurtEffectObj.Play();
            _animator.SetTrigger("Hit");
            _audioSource.PlayOneShot(hitAudio);
        }
        else
        {
            var cureEffectObj = Instantiate(cureEffect, _rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            cureEffectObj.Play();
        } 
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(_currentHealth / (float)maxHealth);
    } 

    public void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
        
        _animator.SetTrigger("Launch");
        _audioSource.PlayOneShot(launchAudio);
    }
    
    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
