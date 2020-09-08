using System.Collections.Generic;
using Match3.Features;
using Match3.Logic;

namespace Match3
{
    public static class GameFactory
    {
        public static IGameRules ConstructRules(
            IEnumerable<IGameFeature> gameFeatures,
            IEnumerable<IActionFeature> actionFeatures,
            IEnumerable<ICellComponentFeature> cellComponentFeatures,
            IEnumerable<IObjectFeature> cellObjectFeature,
            IEnumerable<IObjectComponentFeature> objectComponentFeatures)
        {
            var rules = new GameRules();

            foreach (var feature in objectComponentFeatures)
            {
                rules.RegisterObjectComponentFeature(feature);
            }

            foreach (var feature in cellObjectFeature)
            {
                rules.RegisterObjectFeature(feature);
            }


            foreach (var feature in gameFeatures)
            {
                rules.RegisterGameFeature(feature);
            }

            foreach (var feature in actionFeatures)
            {
                rules.RegisterActionFeature(feature);
            }

            foreach (var feature in cellComponentFeatures)
            {
                rules.RegisterCellComponentFeature(feature);
            }

            rules.BakeAllFeatures();

            return rules;
        }

        public static void Construct(IGameRules rules, IEnumerable<IGridData> gridData, out IGameObserver game, out IGameController gameController)
        {
            var gameImpl = new Game(rules, gridData);
            game = gameImpl;
            gameController = gameImpl;
        }
    }
}