using System;
using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.Events;
namespace Player
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private PlayerCam playerCam;
        [SerializeField] private Grappling grappling;
        [SerializeField] private GameObject hitEffect;
        private GameObject target;
        private Vector3 targetPos;
        private bool targetIsEnemy;
        private Action defeatEnemyEvent;
        public Action setDefeatEnemyEvent { set { defeatEnemyEvent = value; } }

        public void Fire()
        {
            var hit = grappling.PlayerShot();
            if (hit.collider == null) return;

            SEManager.Instance.Play(SEPath.WIND);
            playerMove.Shot();
            target = hit.collider.transform.gameObject;
            targetIsEnemy = target.CompareTag("Enemy");
            targetPos = hit.point;
        }

        public void GrapplingMove()
        {
            if (target == null) return;
            var pos = targetIsEnemy ? target.transform.position : targetPos;
            playerMove.JumpToPosition(pos);
        }

        public bool canReticle()
        {
            return grappling.canGrapple();
        }

        public void Release()
        {
            SEManager.Instance.Stop(SEPath.WIND);
            target = null;
            playerMove.Release();
            grappling.StopGrapple();
        }

        public void Move(Vector2 value)
        {
            playerMove.Move(value);
        }

        public void Look(Vector2 value)
        {
            playerCam.Look(value);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var enemy = collision.gameObject.GetComponent<EnemyMgr>();
                var e = Instantiate(hitEffect);
                var def = enemy.transform.position - transform.position;
                e.transform.position = enemy.transform.position + new Vector3(def.x, 0, def.z) * 5;
                enemy.Blow();
                Release();
                SEManager.Instance.Play(SEPath.KILL);

                if (defeatEnemyEvent != null)
                {
                    defeatEnemyEvent.Invoke();
                }
                else
                {
                    Debug.LogAssertion("イベントを設定して!");
                }
            }
        }

    }
}