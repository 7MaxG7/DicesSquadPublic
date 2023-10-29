using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Configs/UI/" + nameof(UIConfig), fileName = nameof(UIConfig), order = 0)]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private float _defaultAnimationDuration;
        [Tooltip("Продолжительность анимации изменения значения шкал здоровья")]
        [SerializeField] private float _healthBarAnimationLength;

        public float DefaultAnimationDuration => _defaultAnimationDuration;
        public float HealthBarAnimationLength => _healthBarAnimationLength;
    }
}