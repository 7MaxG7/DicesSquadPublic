using Zenject;

namespace Units
{
    public class BattleUnitSelectionService
    {
        private readonly UnitViewService _unitViewService;

        [Inject]
        public BattleUnitSelectionService(UnitViewService unitViewService)
        {
            _unitViewService = unitViewService;
        }
        
        public void ToggleSelection(int unit, bool mustSelected)
            => _unitViewService.ToggleUnitDiceHighlight(unit, mustSelected);
    }
}