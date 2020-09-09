using System;
using Match3.Editor;
using Match3.Features;

namespace Match3
{
    class LevelEditor : ILevelEditor
    {
        private readonly IGameRules _rules;

        private IGameObserver _gameObserver;
        private IGameController _gameController;

        public LevelEditor(IGameRules rules)
        {
            _rules = GameFactory.ConstructRules(
                new IGameFeature[] {},
                new IActionFeature[] {},
                new ICellComponentFeature[] {},
                rules.ObjectFeatures,
                rules.ObjectComponentFeatures);

            _gameObserver = EmptyGameObserver.Instance;
            _gameController = null;
        }

        public IGameObserver View => _gameObserver;

        public event Action LevelReloaded;

        public bool LoadLevel(ILevelData levelData)
        {
            try
            {
                GameFactory.Construct(_rules, levelData.Boards, out _gameObserver, out _gameController);
                LevelReloaded?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                // TODO: LOG
                _gameObserver = EmptyGameObserver.Instance;
                _gameController = null;
                LevelReloaded?.Invoke();
                return false;
            }
        }

        public ILevelData SaveLevel()
        {
            return null;
        }
    }
}