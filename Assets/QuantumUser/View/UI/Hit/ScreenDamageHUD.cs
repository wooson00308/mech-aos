
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Quantum.Mech
{
    public class ScreenDamageHUD : QuantumSceneViewComponent<CustomViewContext>//QuantumViewComponent<CustomViewContext>
    {
        [SerializeField, Tooltip("0 ~ 1")] 
        private float _threshold ;
        [SerializeField]
        private PostProcessVolume _postProcessVolume;

        [SerializeField] 
        private Vector2 _vignetteSmoothnessMinMax = new Vector2(0.2f,0.5f);
        [SerializeField] 
        private Vector2 _chromaticAberrationIntensityMinMax = new Vector2(0.3f,0.6f);
        
        // [SerializeField]
        private Vignette _vignette;
        // [SerializeField]
        private ChromaticAberration _chromaticAberration;

        private float _elapsedTime = 0;
        
        private void OnMechanicDeath(EventOnMechanicDeath e)
        {
            if (e.Mechanic != ViewContext.LocalEntityRef) return;
            _postProcessVolume.enabled = false;

        }
        private unsafe void OnMechanicTakeDamage(EventOnMechanicTakeDamage e)
        {
            if (e.Mechanic != ViewContext.LocalEntityRef) return;
            var f = QuantumRunner.DefaultGame.Frames.Predicted;
            var status = f.Unsafe.GetPointer<Status>(e.Mechanic);
            var statusData = f.FindAsset<StatusData>(status->StatusData.Id);
            var max = statusData.MaxHealth * (1 + (status->Level - 1) * FP._0_10);
            var ratio = FPMath.InverseLerp(FP._0, max, status->CurrentHealth);
            
            if (!(ratio.AsFloat <= _threshold)) return;
            _postProcessVolume.enabled = true;

        }

        public void Start()
        {
            _postProcessVolume.profile.TryGetSettings(out _vignette);
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            QuantumEvent.Subscribe<EventOnMechanicTakeDamage>(this, OnMechanicTakeDamage);
            QuantumEvent.Subscribe<EventOnMechanicDeath>(this, OnMechanicDeath);
            _postProcessVolume.enabled = false;
        }

        public override void OnUpdateView()
        {
            if (_vignette == null || _chromaticAberration == null) return;
            if (_vignette.active && _chromaticAberration.active)
            {
                var intensity = _chromaticAberrationIntensityMinMax.x + (_chromaticAberrationIntensityMinMax.y - _chromaticAberrationIntensityMinMax.x) * (Mathf.Sin(_elapsedTime) + 1) / 2.0f;
                _chromaticAberration.intensity.value = intensity;
                var smoothness = _vignetteSmoothnessMinMax.x + (_vignetteSmoothnessMinMax.y - _vignetteSmoothnessMinMax.x) * (Mathf.Sin(_elapsedTime) + 1) / 2.0f;
                _vignette.smoothness.value = smoothness;
                _elapsedTime += Time.deltaTime;
            }
        }
    }
}