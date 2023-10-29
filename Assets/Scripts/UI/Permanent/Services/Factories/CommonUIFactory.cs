using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace UI.Permanent
{
    public class CommonUIFactory
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly UIAssetsDb _uiAssetsDb;
        private readonly CurtainUIController _curtainUIController;
        private readonly UiAnimationService _uiAnimationService;
        private readonly UIConfig _uiConfig;

        private Transform _rootCanvas;

        [Inject]
        public CommonUIFactory(AssetsProvider assetsProvider, UIAssetsDb uiAssetsDb, CurtainUIController curtainUIController,
            UiAnimationService uiAnimationService, UIConfig uiConfig)
        {
            _assetsProvider = assetsProvider;
            _uiAssetsDb = uiAssetsDb;
            _curtainUIController = curtainUIController;
            _uiAnimationService = uiAnimationService;
            _uiConfig = uiConfig;
        }

        public async UniTask PrepareCanvasAsync()
        {
            var permanentUI =
                await _assetsProvider.CreateInstanceAsync<PermanentUIView>(_uiAssetsDb.PermanentUIView, isDontDestroyAsset: true);
            _rootCanvas = permanentUI.Content;
            
            Object.DontDestroyOnLoad(permanentUI.gameObject);
        }

        public async UniTask CreateCurtain()
        {
            var curtainView = await _assetsProvider.CreateInstanceAsync<CurtainUIView>(_uiAssetsDb.CurtainView, _rootCanvas, true);
            curtainView.Init(_uiAnimationService, _uiConfig);
            _curtainUIController.Init(curtainView);
        }
    }
}