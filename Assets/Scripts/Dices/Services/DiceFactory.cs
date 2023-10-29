using Infrastructure;

namespace Dices
{
    public class DiceFactory
    {
        private readonly EcsService _ecsService;
        private readonly DiceService _diceService;

        public DiceFactory(EcsService ecsService, DiceService diceService)
        {
            _ecsService = ecsService;
            _diceService = diceService;

            _ecsService.World.GetPool<DiceComponent>();
        }

        public int CreateMainDice(int unit, DiceConfig config)
        {
            var dice = _ecsService.CreateEntity();
            _diceService.InitComponents(unit, config, dice);
            return dice;
        }
    }
}