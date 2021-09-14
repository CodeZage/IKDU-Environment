using System.Collections;
using Player;
using UnityEngine;

namespace Interaction
{
    public class Pickup : Interactable
    {
        private bool interactable = true;
        private Transform playerHand;
        protected Rigidbody Rigidbody;

        protected override void Start()
        {
            UseInfo = "Pick Up";
            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;

            playerHand = FindObjectOfType<PlayerController>().transform.Find("Camera").transform
                .Find("PickupContainer");
        }

        public override void Interact()
        {
            if (!interactable) return;
            GetComponent<Collider>().enabled = false;
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;

            StartCoroutine(LerpPosition(playerHand, 0.05f));
        }

        public void Drop()
        {
            GetComponent<Collider>().enabled = true;
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            Rigidbody.AddForce(playerHand.forward * 200f);
        }

        protected IEnumerator LerpPosition(Transform target, float duration)
        {
            float time = 0;
            interactable = false;
            var startPosition = transform.position;

            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, target.position, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            var thisTransform = transform;
            thisTransform.SetParent(target);
            thisTransform.position = target.position;
            interactable = true;
        }
    }
}