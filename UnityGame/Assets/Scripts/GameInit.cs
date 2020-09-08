using System;
using System.Collections.Generic;
using Match3;
using Match3.Features;
using Match3.Features.Bomb;
using Match3.Features.Bomb.Default;
using Match3.Features.Chain;
using Match3.Features.Chain.Default;
using Match3.Features.Chip;
using Match3.Features.Chip.Default;
using Match3.Features.Color.Default;
using Match3.Features.Default;
using Match3.Features.Emitter;
using Match3.Features.Emitter.Default;
using Match3.Features.Health.Default;
using Match3.Features.Mass.Default;
using Match3.Features.Move.Default;
using Match3.Features.Tile;
using Match3.Features.Tile.Default;
using Match3.Utils;
using Match3.View.Default;
using Replays;
using UnityEngine;
using Debug = Match3.Debug;

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

    private ReplayData _replayData;
    private ReplayPlayer _replayPlayer;

    private string _strPos = "0";

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 40), "Current tick: " + _replayWriter.TickId);

        if (GUI.Button(new Rect(10, 50, 100, 40), "Start Replay"))
        {
            _replayData = _replayWriter.GetReplay();

            ConstructGame(new IGridData[] { ConstructGridData() }, out var gamePresenter, out var gameController);

            _replayPlayer = new ReplayPlayer(_replayData, gameController);
            gameController = _replayPlayer;

            var go = StartGame(gamePresenter, gameController, "Replay");

            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.localPosition = new Vector3(-6, 0, 0);
        }

        if (_replayData != null)
        {
            GUI.Label(new Rect(10, 100, 200, 40), "Replay tick: " + _replayPlayer.TickId);

            if (GUI.Button(new Rect(10, 150, 100, 40), "Restart Replay"))
            {
                ConstructGame(new IGridData[] { ConstructGridData() }, out var gamePresenter, out var gameController);

                _replayPlayer = new ReplayPlayer(_replayData, gameController);
                gameController = _replayPlayer;

                var go = StartGame(gamePresenter, gameController, "Replay");

                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                go.transform.localPosition = new Vector3(-6, 0, 0);
            }

            _strPos = GUI.TextField(new Rect(220, 200, 50, 30), _strPos);

            if (GUI.Button(new Rect(10, 200, 200, 40), "Restart Replay From"))
            {
                ConstructGame(new IGridData[] { ConstructGridData() }, out var gamePresenter, out var gameController);

                _replayPlayer = new ReplayPlayer(_replayData, gameController);
                gameController = _replayPlayer;

                var go = StartGame(gamePresenter, gameController, "Replay");

                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                go.transform.localPosition = new Vector3(-6, 0, 1);

                _replayPlayer.ApplyNSteps(int.Parse(_strPos));
            }

            if (GUI.Button(new Rect(10, 250, 100, 40), "Stop Match3"))
            {
                var obj = GameObject.Find("Match3");
                obj.SetActive(!obj.activeInHierarchy);
            }
        }
    }

    public GameObject StartGame(IGameObserver gamePresenter, IGameController gameController, string name)
    {
        gameController.Start();

        var obj = GameObject.Find(name);
        if (obj != null)
            Destroy(obj);

        var gameView = Instantiate(_gameView).GetComponent<GameView>();
        gameView.name = name;
        gameView.transform.position = Vector3.zero;
        gameView.Init(gamePresenter, new DefaultViewContext(false, gameController, _viewFactory));

        return gameView.gameObject;
    }

    public void ConstructGame(IEnumerable<IGridData> gridData, out IGameObserver game, out IGameController gameController)
    {
        IGameRules rules = GameFactory.ConstructRules(new IGameFeature[]
            {
                new Emitters(),
                new Gravity(),
                ConstructMathFeature()
            },
            new IActionFeature[]
            {
                new KillActionFeature(),
                new SwapActionFeature()
            },
            new ICellComponentFeature[]
            {
                new HealthCellComponentFeatureImpl(),
            },
            new IObjectFeature[]
            {
                new ChipCellObjectFeatureImpl(),
                new ChainCellObjectFeatureImpl(),
                new TileCellObjectFeatureImpl(),
                new EmitterCellObjectFeatureImpl(),
                new BombCellObjectFeatureImpl()
            },
            new IObjectComponentFeature[]
            {
                new ColorCellObjectComponentFeatureImpl(),
                new EmitterCellObjectComponentFeatureImpl(),
                new HealthCellObjectComponentFeatureImpl(),
                new MassCellObjectComponentFeatureImpl(),
                new MoveCellObjectComponentFeatureImpl(),
            });

        GameFactory.Construct(rules, gridData, out game, out gameController);
    }

    private Match ConstructMathFeature()
    {
        var match = new Match();

        // куб
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombCellObjectData(color));
            match.RegisterPatterns(new Offsets2D("*****"), bonusFactory, new Offsets2D("..*.."));
        }

        // бомба
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombCellObjectData(color));
            match.RegisterPatterns(new Offsets2D(
                "***",
                ".*.",
                ".*."
            ), bonusFactory, new Offsets2D(
                ".*",
                ".."
            ));
            match.RegisterPatterns(new Offsets2D(
                "***",
                "*..",
                "*.."
            ), bonusFactory, new Offsets2D(
                "*.",
                ".."
            ));
            match.RegisterPatterns(new Offsets2D(
                ".*.",
                "***",
                ".*."
            ), bonusFactory, new Offsets2D(
                "..",
                ".*"
            ));
            match.RegisterPatterns(new Offsets2D(
                ".*.",
                "***",
                ".*.",
                ".*."
            ), bonusFactory, new Offsets2D(
                "..",
                ".*"
            ));
            match.RegisterPatterns(new Offsets2D(
                "****",
                ".*..",
                ".*.."
            ), bonusFactory, new Offsets2D(
                ".*",
                ".."
            ));
            match.RegisterPatterns(new Offsets2D(
                "*..",
                "***",
                "*..",
                "*.."
            ), bonusFactory, new Offsets2D(
                "..",
                "*."
            ));
            match.RegisterPatterns(new Offsets2D(
                ".*..",
                "****",
                ".*..",
                ".*.."
            ), bonusFactory, new Offsets2D(
                "..",
                ".*"
            ));
        }

        // шутиха
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombCellObjectData(color));
            match.RegisterPatterns(new Offsets2D("****"), bonusFactory, new Offsets2D(".**."));
        }

        // ракета
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombCellObjectData(color));
            match.RegisterPatterns(new Offsets2D(
                "**",
                "**"
            ), bonusFactory, new Offsets2D(
                "**",
                "**"
            ));
            match.RegisterPatterns(new Offsets2D(
                "***",
                "**."
            ), bonusFactory, new Offsets2D(
                "**.",
                "**."
            ));
        }

        // ничего
        match.RegisterPatterns(new Offsets2D("***"), null);


        return match;
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

        data.AddCellContent(7, 4, new ChipCellObjectData(3));
        data.AddCellContent(7, 4, new ChainCellObjectData(3));

        data.AddCellContent(1, 5, new TileCellObjectData(2));
        data.AddCellContent(2, 5, new TileCellObjectData(1));
        data.AddCellContent(1, 4, new TileCellObjectData(1));

        data.AddCellContent(1, 1, new BombCellObjectData(2));


        ChipCellObjectData[] chips = new ChipCellObjectData[5];
        for (int i = 0; i < chips.Length; ++i)
            chips[i] = new ChipCellObjectData(i);

        for (int x = 0; x < 10; ++x)
        {
            data.AddCellContent(x, 9, new EmitterCellObjectData(chips));
        }

        return data;
    }

    public class BonusFactory : Factory<ICellObjectData, int>, Match.IBonusFactory
    {
        public BonusFactory(Func<int, ICellObjectData> factory)
            : base(factory)
        {
        }
    }
}
