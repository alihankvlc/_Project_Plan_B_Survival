using System;
using System.Collections.Generic;
using System.Linq;
using _Player_System_.Runtime.Common;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Sub;
using Sirenix.OdinInspector;
using Zenject;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject.SpaceFighter;
using Random = System.Random;

namespace _Zombie_System_.Runtime.Common
{
    //TODO: Bu stat şeysi hem player olsun hem de enemy olsun aynı olduğundan bir base class yapcam..
    public class ZombieStatHandler : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _healthBaseValue;

        [SerializeField, ReadOnly] private int _currentHealth;

        //Bind
        [Inject(Id = StatGroup.Enemy)] private Health _health;
        [Inject] private IPlayerExperienceHandler _playerExperienceHandler;

        private CapsuleCollider _capsuleCollider;
        private Animator _animator;

        private readonly int ENEMY_ALIVE_HASH_ID = Animator.StringToHash("IsAlive");
        private readonly int ENEMY_TAKE_HIT_HASH_ID = Animator.StringToHash("TakeHit");

        private void Start()
        {
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _animator = GetComponent<Animator>();

            _health.ModifyBaseValue(_healthBaseValue);
            _currentHealth = _health.BaseValue;
        }

        public void TakeDamage(int amount)
        {
            _health.Modify -= amount;
            _currentHealth = _health.GetStatus().health;
            
            _animator.SetTrigger(ENEMY_TAKE_HIT_HASH_ID);

            if (_health.GetStatus().isDead) Dead();
        }

        //TODO: EXP çarpanı yapıcam GameManager için.. Defaulth şimdilik 100
        private void Dead()
        {
            _playerExperienceHandler.GrantExperiencePoint += 100;

            _animator.SetBool(ENEMY_ALIVE_HASH_ID, false);
            _capsuleCollider.enabled = false;

            Invoke(nameof(DestroyGameObject), 3f); //TODO: Belki bunu coroutine çevirip içini dahada doldurabilirim....
        }

        private void DestroyGameObject() => Destroy(gameObject);
    }
}