using Battle;
using Battle.Battlefield;
using Dices;
using Infrastructure.Configs;
using Infrastructure.Input;
using UI.Permanent;
using Units;
using Units.Factories;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Common
            Container.Bind<TeamService>().AsSingle();

            // Input
            Container.Bind<InputService>().AsSingle();
            Container.Bind<InputConfig>().FromScriptableObjectResource(nameof(InputConfig)).AsSingle();
            
            // Factories
            Container.Bind<UnitFactory>().AsSingle();
            Container.Bind<DiceFactory>().AsSingle();
            Container.Bind<CommonUIFactory>().AsSingle();

            // Battlefield
            Container.Bind<BattleCellService>().AsSingle();
            Container.Bind<BattleLocationService>().AsSingle();
            Container.Bind<BattlefieldConfig>().FromScriptableObjectResource(nameof(BattlefieldConfig)).AsSingle();
            
            // Units
            Container.Bind<UnitService>().AsSingle();
            
            // Dices
            Container.Bind<DiceService>().AsSingle();
        }
    }
}