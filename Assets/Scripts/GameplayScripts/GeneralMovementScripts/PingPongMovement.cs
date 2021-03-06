﻿using UnityEngine;

namespace GameplayScripts.GeneralMovementScripts
{
    public class PingPongMovement : PlatformMovement
    {
        [Tooltip("Points to move in between")]
        public Transform pos1, pos2;

        private Vector3 nextPos;
        private float oldPosition;
        private Transform lastPos;

        /// <summary>
        /// Set all local values
        /// </summary>
        protected override void Start()
        {
            base.Start();

            nextPos = pos2.position;

            oldPosition = transform.position.x;

            lastPos = pos1;
        }
        /// <summary>
        /// Move towards next position
        /// </summary>
        private void Update()
        {
            if (!IsPlatformActive) return;
            
            InvertDirection();

            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        /// <summary>
        /// Checks to see if the ground or player is blocking the platforms path if so change direction
        /// </summary>
        /// <param name="collision"></param>
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (!usePressurePlate)
            {
                UpdateMovement();
            }
            else if (HasPlateBeenPressed)
            {
                UpdateMovement();
            }
        }
        /// <summary>
        /// Makes platform move in opposite direction when movement is blocked
        /// </summary>
        private void UpdateMovement()
        {
            if (TouchingGround || IsPathBlocked)
            {
                if (transform.position.x > oldPosition || transform.position.x < oldPosition)
                {
                    nextPos = lastPos.position;
                }
            }
        }
        /// <summary>
        /// Change movement direction to move in the opposite direction the platform is currently moving in
        /// </summary>
        private void InvertDirection()
        {
            if (Vector3.Distance(transform.position, pos1.position) <= 0.01f)
            {
                nextPos = pos2.position;
                lastPos = pos1;
            }
            else if (Vector3.Distance(transform.position, pos2.position) <= 0.01f)
            {
                nextPos = pos1.position;
                lastPos = pos2;
            }
        }
        /// <summary>
        /// Draw line between position 1 and 2 in editor
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(pos1.position, pos2.position);
        }
    }
}