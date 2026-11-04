using ObjectsPool;
using System;
using System.Buffers.Text;
using UnityEngine;

namespace Blocks
{
    public class BlockController : PooledObject
    {
        public static event System.Action<BlockController> BlockDestroyed;

        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        private int baseHP;
        private int currentHP;

        private bool isHitted = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Ball"))
            {
                Hit();
            }
        }

        public void Hit()
        {
            if (isHitted)
                return;

            _animator.SetTrigger("Flash");

            if (baseHP == 3)
                return;

            currentHP--;
            if(currentHP <= 0)
            {
                ReturnToPool();
                BlockDestroyed?.Invoke(this);
            }
            else
            {
                _animator.SetTrigger("Flash");
            }

            isHitted = true;
        }

        internal void Setup(int blockHP, Color blockColor)
        {
            baseHP = blockHP;
            currentHP = blockHP;
            _spriteRenderer.color = blockColor;
            isHitted = false;
        }
    }
}