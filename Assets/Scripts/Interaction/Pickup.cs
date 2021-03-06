using System.Collections;
using Assets.Scripts.Interaction;
using Player;
using UnityEngine;

namespace Interaction
{
    public class Pickup : Interactable
    {
        private bool _interactable = true;
        private Transform _playerHand;
        protected Rigidbody Rigidbody;

        protected override void Start()
        {
            if (interactInfo == "") interactInfo = "Pick Up";
            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;

            _playerHand = FindObjectOfType<PlayerController>().transform.Find("Camera").transform
                .Find("PickupContainer");
        }

        public override void Interact()
        {
            if (!_interactable) return;
            GetComponent<Collider>().enabled = false;
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;

            StartCoroutine(LerpPosition(_playerHand, 0.05f));
        }

        public void Drop()
        {
            GetComponent<Collider>().enabled = true;
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            Rigidbody.AddForce(_playerHand.forward * 200f);
        }

        protected IEnumerator LerpPosition(Transform target, float duration)
        {
            float time = 0;
            _interactable = false;
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
            _interactable = true;
        }
    }
}
