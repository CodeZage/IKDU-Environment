using Assets.Scripts.Interaction;
using UnityEngine;

namespace Interaction
{
    public class ToggleLight : Pickup, IAltInteractable
    {
        private Light _light;
        private Material _material, _lightMaterial;
        private MeshRenderer _meshRenderer;

        protected override void Start()
        {
            base.Start();
            _light = GetComponent<Light>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _lightMaterial = _meshRenderer.material;
            _material = Resources.Load<Material>("Materials/BlueLightMaterial");
        }

        public void AltInteract()
        {
            _light.enabled = !_light.enabled;

            _meshRenderer.material = _meshRenderer.material == _material ? _lightMaterial : _material;
        }
    }
}