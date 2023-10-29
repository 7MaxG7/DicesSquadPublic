using CustomTypes.Enums.Battle;
using CustomTypes.Enums.Infrastructure;
using Infrastructure;
using Infrastructure.Configs;
using Infrastructure.Input;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle
{
    public class BattleSelectionService
    {
        private readonly EcsService _ecsService;
        private readonly InputConfig _inputConfig;
        private readonly DebugService _debug;
        private readonly InputService _inputService;

        private readonly EcsFilter _selectedFilter;
        private readonly EcsPool<BattleSelectedComponent> _battleSelectedPool;
        private readonly EcsPool<BattleSelectEventComponent> _battleSelectEventPool;
        private readonly EcsPool<BattleDeselectEventComponent> _battleDeselectEventPool;

        private readonly RaycastHit[] _raycastHits;

        public BattleSelectionService(EcsService ecsService, InputConfig inputConfig, InputService inputService, DebugService debug)
        {
            _ecsService = ecsService;
            _inputConfig = inputConfig;
            _inputService = inputService;
            _debug = debug;

            _selectedFilter = ecsService.World.Filter<BattleSelectedComponent>().End();
            _battleSelectedPool = ecsService.World.GetPool<BattleSelectedComponent>();
            _battleSelectEventPool = ecsService.World.GetPool<BattleSelectEventComponent>();
            _battleDeselectEventPool = ecsService.World.GetPool<BattleDeselectEventComponent>();
            
            _raycastHits = new RaycastHit[_inputConfig.SelectionRaycastHitsCount];
        }

        public void Init()
        {
            _inputService.UserInputControls.BattleSelection.InfoClick.performed += ShowInfo;
            _inputService.UserInputControls.BattleSelection.Enable();
        }

        public void Clear()
        {
            _inputService.UserInputControls.BattleSelection.Disable();
            _inputService.UserInputControls.BattleSelection.InfoClick.performed -= ShowInfo;

        }

        public bool TryGetSelection(out int selected, out BattleSelectionType selectionType)
        {
            selected = -1;
            selectionType = BattleSelectionType.None;
            return TryRaycastSelection(out var hitsCount) && TryIdentifySelection(hitsCount, out selected, out selectionType);
        }

        private void ShowInfo(InputAction.CallbackContext _)
        {
            ClearSelection();
            
            if (!TryGetSelection(out var selected, out var selectionType))
                return;

            MarkSelected(selected, selectionType);
        }

        private void ClearSelection()
        {
            foreach (var selected in _selectedFilter)
                _battleDeselectEventPool.Add(selected);
        }

        private bool TryRaycastSelection(out int hitsCount)
        {
            // ReSharper disable once PossibleNullReferenceException
            var ray = Camera.main.ScreenPointToRay(_inputService.MousePosition);
            hitsCount = Physics.RaycastNonAlloc(ray, _raycastHits, _inputConfig.SelectionRaycastLength, _inputConfig.SelectionLayerMask);

            return hitsCount != 0;
        }

        private bool TryIdentifySelection(int hitsCount, out int selected, out BattleSelectionType selectionType)
        {
            selected = -1;
            selectionType = BattleSelectionType.None;
            
            var hit = _raycastHits[0].transform;
            if (!hit.TryGetComponent<BattleSelectView>(out var selectView))
            {
                _debug.Log(DebugType.Warning, $"Cannot get select component for {hit.gameObject.name}");
                return false;
            }

            if (!_ecsService.TryUnpack(selectView.Entity, out selected))
            {
                _debug.Log(DebugType.Warning, $"Cannot unpack entity for {hit.gameObject.name}");
                return false;
            }

            selectionType = selectView.SelectionType;
            return true;
        }

        private void MarkSelected(int selected, BattleSelectionType selectionType)
        {
            if (_battleSelectedPool.Has(selected))
                return;
            
            _battleSelectEventPool.Add(selected);
            ref var selectedComponent = ref _battleSelectedPool.Add(selected);
            selectedComponent.SelectionType = selectionType;
        }
    }
}