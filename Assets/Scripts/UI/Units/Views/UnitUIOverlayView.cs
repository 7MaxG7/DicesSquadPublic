using UnityEngine;

namespace UI.Units
{
    public class UnitUIOverlayView : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBar;
        [SerializeField] private Transform _facetIconsContent;

        public HealthBarView HealthBar => _healthBar;
        public Transform FacetIconsContent => _facetIconsContent;
    }
}