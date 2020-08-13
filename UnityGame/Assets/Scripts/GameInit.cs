using System.Collections.Generic;
using Match3;
using Match3.Core;
using Match3.Features;
using Match3.View;
using Replays;
using UnityEngine;
using Debug = Match3.Core.Debug;

public class GameInit : MonoBehaviour
{
    public ViewFactory _viewFactory;
    public GameView _gameView;

    private ReplayWriter _replayWriter;

    private void Awake()
    {
        Debug.OnLog += UnityEngine.Debug.Log;
        Debug.OnWarning += UnityEngine.Debug.LogWarning;

        ConstructGame(new IGridData[] { ConstructGridData() }, out var gamePresenter, out var gameController);

        _replayWriter = new ReplayWriter(gameController);
        gameController = _replayWriter;

        StartGame(gamePresenter, gameController, "Match3");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 50), "Replay"))
        {
            ConstructGame(new IGridData[] { ConstructGridData() }, out var gamePresenter, out var gameController);

            gameController = new ReplayPlayer(_replayWriter.GetReplay(), gameController);

            var go = StartGame(gamePresenter, gameController, "Replay");

            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.localPosition = new Vector3(-6, 0, 1);

        }
    }



    public GameObject StartGame(IGame gamePresenter, IGameController gameController, string name)
    {
        gameController.Start();

        var obj = GameObject.Find(name);
        if (obj != null)
            Destroy(obj);

        var gameView = Instantiate(_gameView).GetComponent<GameView>();
        gameView.name = name;
        gameView.transform.position = Vector3.zero;
        gameView.Setup(gamePresenter, gameController);

        return gameView.gameObject;
    }

    public void ConstructGame(IEnumerable<IGridData> gridData, out IGame game, out IGameController gameController)
    {
        IGameRules rules = new GameRules(_viewFactory);

        rules.RegisterObjectFeature(ChipObjectFeature.Instance);
        rules.RegisterObjectFeature(ChainObjectFeature.Instance);

        rules.RegisterGameFeature(new Emitters());
        rules.RegisterGameFeature(new Gravity());
        rules.RegisterGameFeature(new Match());

        rules.RegisterActionFeature(new KillActionFeature());
        rules.RegisterActionFeature(new SwapActionFeature());

        rules.BakeAllFeatures();

        var gameInstance = new Game(rules, gridData);
        game = gameInstance;
        gameController = gameInstance;
    }

    class EmitterData : EmitterObjectFeature.IEmitterObjectData, EmitterComponentFeature.IEmitterData
    {
        public string TypeId => EmitterObjectFeature.Name;
        public EmitterComponentFeature.IEmitterData Data => this;
        public ICellObjectData[] ObjectsToEmit { get; }

        public EmitterData(ICellObjectData[] objectsToEmit)
        {
            ObjectsToEmit = objectsToEmit;
        }
    }

    private TrivialGridData ConstructGridData()
    {
        var data = new TrivialGridData(10, 10);
        for (int x = 0; x < 10; ++x)
            for (int y = 0; y < 10; ++y)
                if (!(x == 3 && y == 3 || x == 4 && y == 4 || x == 5 && y == 5))
                {
                    data.ActivateCell(x, y);
                }

        data.AddCellContent(7, 4, new ChipData(3));
        data.AddCellContent(7, 4, new ChainData());

        ChipData[] chips = new ChipData[5];
        for (int i = 0; i < chips.Length; ++i)
            chips[i] = new ChipData(i);

        for (int x = 0; x < 10; ++x)
        {
            data.AddCellContent(x, 9, new EmitterData(chips));
        }

        return data;
    }

    private class ChipData : ChipObjectFeature.IChipData
    {
        private class ColorData : ColorComponentFeature.IColorData
        {
            public string TypeId => ColorComponentFeature.Name;
            public int ColorId { get; }

            public ColorData(int colorId)
            {
                ColorId = colorId;
            }
        }

        private class MassData : MassComponentFeature.IMassData
        {
            public string TypeId => MassComponentFeature.Name;
        }

        private class MoveData : MoveComponentFeature.IMoveData
        {
            public string TypeId => MoveComponentFeature.Name;
        }

        public string TypeId => ChipObjectFeature.Name;
        public ColorComponentFeature.IColorData Color { get; }
        public MoveComponentFeature.IMoveData Movement { get; }

        public MassComponentFeature.IMassData Mass { get; }

        public int BodyType => 0;

        public int ColorId => Color.ColorId;

        public ChipData(int colorId)
        {
            Color = new ColorData(colorId);
            Mass = new MassData();
            Movement = new MoveData();
        }
    }

    private class ChainData : ChainObjectFeature.IChainData
    {
        public string TypeId => ChainObjectFeature.Name;
    }
}
