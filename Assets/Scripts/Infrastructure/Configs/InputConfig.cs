using UnityEngine;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(InputConfig), fileName = nameof(InputConfig), order = 1)]
    public class InputConfig : ScriptableObject
    {
        [Header("Selection raycast")]
        [Tooltip("Число объектов, фиксируемых рейкастом при клике системой выбора")]
        [SerializeField] private int _selectionRaycastHitsCount = 8;

        [Tooltip("Длина рейкаста при клике системой выбора")]
        [SerializeField] private float _selectionRaycastLength = 10f;

        [Tooltip("Слой для рейкаста при клике системой выбора")]
        [SerializeField] private LayerMask _selectionLayerMask;

        public int SelectionRaycastHitsCount => _selectionRaycastHitsCount;
        public float SelectionRaycastLength => _selectionRaycastLength;
        public LayerMask SelectionLayerMask => _selectionLayerMask;
    }
}