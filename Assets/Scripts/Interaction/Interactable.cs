using QuickOutline;
using UnityEngine;

namespace Interaction
{
    public abstract class Interactable : MonoBehaviour
    {
        public string interactInfo;

        [HideInInspector]
        public bool isInteractable;
        private Outline _outline;

        protected virtual void Awake()
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineColor = Color.white;
            _outline.OutlineWidth = 3;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.enabled = false;
            isInteractable = true;
        }

        protected virtual void Start()
        {
            if (interactInfo == "") interactInfo = "Interact";
        }

        public abstract void Interact();


        public virtual void Target()
        {
            _outline.enabled = true;
        }

        public virtual void RemoveTarget()
        {
            _outline.enabled = false;
        }
    }
}
