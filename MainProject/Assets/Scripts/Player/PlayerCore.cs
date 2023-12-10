using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private PlayerCam playerCam;
        [SerializeField] private Grappling grappling;

        public void Fire()
        {
            grappling.PlayerShot();
        }

        public void Release()
        {
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
    }
}