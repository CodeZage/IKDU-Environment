using QuickOutline;
using UnityEngine;

namespace Interaction
{
    public abstract class Interactable : MonoBehaviour
    {
        public string interactInfo;
        private Outline outline;

        protected virtual void Awake()
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 3;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.enabled = false;
        }

        protected virtual void Start()
        {
            if (interactInfo == "") interactInfo = "Interact";
        }

        public abstract void Interact();


        public virtual void Target()
        {
            outline.enabled = true;
        }

        public virtual void RemoveTarget()
        {
            outline.enabled = false;
        }
    }
}