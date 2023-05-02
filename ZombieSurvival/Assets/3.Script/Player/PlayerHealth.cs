using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    [Header("Player Health UI")]
    public Slider healthSlider;

    [Header("Player Event Sound Clip")]
    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip ItemDropClip;

    private AudioSource playerAudio;
    private Animator playerAni;

    private PlayerMovement playerMove;
    private PlayerShooter playerShoot;

    void Awake()
    {
        TryGetComponent(out playerAudio);
        TryGetComponent(out playerAni);
        TryGetComponent(out playerMove);
        TryGetComponent(out playerShoot);
    }

    protected override void OnEnable()
    {
        //base? 부모 클래스의 메서드 호출
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = health;

        playerMove.enabled = true;
        playerShoot.enabled = true;
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (!isDead)
        {
            playerAudio.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPosition, hitNormal);
        healthSlider.value = health;
    }

    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        playerAudio.PlayOneShot(deathClip);
        playerAni.SetTrigger("Die");

        playerMove.enabled = false;
        playerShoot.enabled = false;
    }

    //
}
