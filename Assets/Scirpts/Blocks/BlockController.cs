using ObjectsPool;
using System;
using System.Buffers.Text;
using UnityEngine;

namespace Blocks
{
    public class BlockController : PooledObject
    {
        public static event System.Action<Vector3> BlockDestroyed;

        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        private int baseHP;
        private int currentHP;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Ball"))
            {
                Hit();
            }
        }

        public void Hit()
        {
            _animator.SetTrigger("Flash");

            if (baseHP == 3)
                return;

            currentHP--;
            if(currentHP <= 0)
            {
                ReturnToPool();
                BlockDestroyed?.Invoke(transform.position);
            }
            else
            {
                _animator.SetTrigger("Flash");
            }
        }

        internal void Setup(int blockHP, Color blockColor)
        {
            baseHP = blockHP;
            currentHP = blockHP;
            _spriteRenderer.color = blockColor;
        }
    }
}