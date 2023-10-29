using Cysharp.Threading.Tasks;

namespace UI.Permanent
{
    public class PermanentUIBuilder
    {
        private readonly CommonUIFactory _commonUIFactory;
        private readonly CurtainUIController _curtainUIController;

        public PermanentUIBuilder(CommonUIFactory commonUIFactory, CurtainUIController curtainUIController)
        {
            _commonUIFactory = commonUIFactory;
            _curtainUIController = curtainUIController;
        }
        
        public async UniTask InitAsync()
            => await _commonUIFactory.PrepareCanvasAsync();

        public async UniTask BuildUIAsync()
        {
            await _commonUIFactory.CreateCurtain();
            _curtainUIController.ShowInstantly();
        }

        public void Clear()
            => _curtainUIController.Clear();
    }
}