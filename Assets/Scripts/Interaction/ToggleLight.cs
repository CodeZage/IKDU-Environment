using Assets.Scripts.Interaction;
using UnityEngine;

namespace Interaction
{
    public class ToggleLight : Pickup, IAltInteractable
    {
        public Material lightMaterial;

        private Material _material;
        private Light _light;
        private MeshRenderer _meshRenderer;

        protected override void Start()
        {
            base.Start();
            _light = GetComponent<Light>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _material = _meshRenderer.material;
            if (!lightMaterial) lightMaterial = Resources.Load<Material>("Materials/BlueLightMaterial");
            _light.color = lightMaterial.color;
            _light.enabled = false;
        }

        public void AltInteract()
        {
            _light.enabled = !_light.enabled;

            _meshRenderer.material = _meshRenderer.material == _material ? lightMaterial : _material;
        }
    }
}