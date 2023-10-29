using Infrastructure.Bootstrap;
using Infrastructure.Configs;
using Leopotam.EcsLite.UnityEditor;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Infrastructure
            Container.Bind<CancellationTokenProvider>().AsSingle();
            Container.Bind<AssetsProvider>().AsSingle();
            Container.Bind<AssetsProviderConfig>().FromScriptableObjectResource(nameof(AssetsProviderConfig)).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<EcsService>().AsSingle();
            Container.Bind<RandomService>().AsSingle();
            Container.Bind<DebugService>().AsSingle();
            Container.Bind<PlayerPrefsService>().AsSingle();
            Container.Bind<StaticDataService>().AsSingle();

            // Debug
            Container.Bind<EcsWorldDebugSystem>().AsSingle().WithArguments(false);
            
            // Initializers
            Container.Bind<GameEditorSystemsInitializer>().AsSingle();
            
        }
    }
}