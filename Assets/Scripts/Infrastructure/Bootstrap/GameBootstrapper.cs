using Cysharp.Threading.Tasks;
using Infrastructure.Input;
using Leopotam.EcsLite;
using UI;
using UI.Permanent;
using Zenject;

namespace Infrastructure.Bootstrap
{
    public class GameBootstrapper : IInitializable, ITickable, ILateDisposable
    {
        public bool CanBeDisposed { get; set; }
        public bool MustDisposed { get; private set; }

        private readonly AssetsProvider _assetsProvider;
        private readonly SceneLoader _sceneLoader;
        private readonly EcsService _ecsService;
        private readonly RandomService _randomService;
        private readonly CancellationTokenProvider _tokenProvider;
        private readonly GameEditorSystemsInitializer _editorSystemsInitializer;
        private readonly StaticDataService _dataService;
        private readonly InputService _inputService;
        private readonly UiAnimationService _uiAnimationService;
        private readonly PermanentUIBuilder _permanentUIBuilder;

        private EcsSystems _editorSystems;

        private bool _isInited;

        public GameBootstrapper(AssetsProvider assetsProvider, SceneLoader sceneLoader, EcsService ecsService, RandomService randomService,
            CancellationTokenProvider tokenProvider, GameEditorSystemsInitializer editorSystemsInitializer, StaticDataService dataService,
            InputService inputService, UiAnimationService uiAnimationService, PermanentUIBuilder permanentUIBuilder)
        {
            _assetsProvider = assetsProvider;
            _sceneLoader = sceneLoader;
            _ecsService = ecsService;
            _randomService = randomService;
            _tokenProvider = tokenProvider;
            _editorSystemsInitializer = editorSystemsInitializer;
            _dataService = dataService;
            _inputService = inputService;
            _uiAnimationService = uiAnimationService;
            _permanentUIBuilder = permanentUIBuilder;
        }

        public void Initialize()
            => InitInfrastructureAsync().Forget();

        public void Tick()
        {
            if (!_isInited)
                return;

            _editorSystems?.Run();
        }

        public void LateDispose()
        {
            _isInited = false;

            if (CanBeDisposed)
                OnDispose();
            else
                MustDisposed = true;
        }

        public void OnDispose()
        {
            _permanentUIBuilder.Clear();
            _uiAnimationService.Clear();
            _inputService.Clear();
            _tokenProvider.OnDispose();
            _assetsProvider.ClearAll();
            _ecsService.DestroyAll();
        }

        private async UniTaskVoid InitInfrastructureAsync()
        {
            _tokenProvider.Init();
            using var localCts = _tokenProvider.CreateLocalCts();
            
            _assetsProvider.Init();
            _ecsService.Init();
            _randomService.Init();
            _dataService.Init();
            _uiAnimationService.Init();
            await _permanentUIBuilder.InitAsync();
            await _permanentUIBuilder.BuildUIAsync();

            InitSystemsAsync();

            await _sceneLoader.LoadSceneAsync(Constants.BATTLE_SCENE_NAME, localCts);
            _isInited = true;
        }

        private void InitSystemsAsync()
        {
            _editorSystems = _ecsService.CreateSystems(false);

            _editorSystemsInitializer.InitSystems(_editorSystems);
        }
    }
}