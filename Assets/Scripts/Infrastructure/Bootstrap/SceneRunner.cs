using System;
using System.Threading;
using Abstractions.Infrastructure;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Zenject;

namespace Infrastructure.Bootstrap
{
    public abstract class SceneRunner : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private readonly GameBootstrapper _gameBootstrapper;
        private readonly EcsService _ecsService;
        private readonly AssetsProvider _assetsProvider;
        private readonly CancellationTokenProvider _tokenProvider;

        protected IUpdateSystemsInitializer UpdateSystemsInitializer;
        protected IUpdateSystemsInitializer FixedUpdateSystemsInitializer;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private bool _isInited;

        protected SceneRunner(GameBootstrapper gameBootstrapper, EcsService ecsService, AssetsProvider assetsProvider,
            CancellationTokenProvider tokenProvider)
        {
            _gameBootstrapper = gameBootstrapper;
            _ecsService = ecsService;
            _assetsProvider = assetsProvider;
            _tokenProvider = tokenProvider;
        }

        public void Initialize()
            => InitAsync().Forget();

        public void Dispose()
        {
            _isInited = false;

            OnDispose();

            _assetsProvider.ClearScene();
            _ecsService.DestroySceneSystems();

            DisposeInfrastructure();
        }

        public void Tick()
        {
            if (!_isInited)
                return;

            _updateSystems?.Run();
        }

        public void FixedTick()
        {
            if (!_isInited)
                return;

            _fixedUpdateSystems?.Run();
        }

        protected abstract void OnDispose();

        protected abstract UniTask OnInitAsync(CancellationTokenSource cts);

        private async UniTaskVoid InitAsync()
        {
            using var localCts = _tokenProvider.CreateLocalCts();
            
            InitSystems();

            await _assetsProvider.WarmUpCurrentSceneAsync();

            await OnInitAsync(localCts);

            _isInited = true;
            _gameBootstrapper.CanBeDisposed = false;
        }

        private void InitSystems()
        {
            _updateSystems = _ecsService.CreateSystems(false);
            _fixedUpdateSystems = _ecsService.CreateSystems(true);

            UpdateSystemsInitializer.InitSystems(_updateSystems);
            FixedUpdateSystemsInitializer.InitSystems(_fixedUpdateSystems);
        }

        private void DisposeInfrastructure()
        {
            if (_gameBootstrapper.MustDisposed)
                _gameBootstrapper.OnDispose();
            else
                _gameBootstrapper.CanBeDisposed = true;
        }
    }
}