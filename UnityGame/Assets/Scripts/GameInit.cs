using System;
using System.Collections.Generic;
using Match3;
using Match3.Core;
using Match3.Features;
using Match3.Utils;
using Match3.View;
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
        gameView.Setup(gamePresenter, gameController, _viewFactory);

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
                HealthCellComponentFeature.Instance
            },
            new IObjectFeature[]
            {
                ChipObjectFeature.Instance,
                ChainObjectFeature.Instance,
                TileObjectFeature.Instance,
                BombObjectFeature.Instance
            },
            new IObjectComponentFeature[]
            {
            });

        GameFactory.Construct(rules, gridData, out game, out gameController);
    }

    private Match ConstructMathFeature()
    {
        var match = new Match();

        // куб
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombObjectData(color));
            match.RegisterPatterns(new Offsets2D("*****"), bonusFactory, new Offsets2D("..*.."));
        }

        // бомба
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombObjectData(color));
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
            BonusFactory bonusFactory = new BonusFactory((color) => new BombObjectData(color));
            match.RegisterPatterns(new Offsets2D("****"), bonusFactory, new Offsets2D(".**."));
        }

        // ракета
        {
            BonusFactory bonusFactory = new BonusFactory((color) => new BombObjectData(color));
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

    class EmitterData : EmitterObjectFeature.IEmitterObjectData, EmitterObjectComponentFeature.IEmitterData
    {
        public string ObjectTypeId => EmitterObjectFeature.Name;
        public EmitterObjectComponentFeature.IEmitterData Data => this;
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

        data.AddCellContent(1, 5, new TileObjectData(2));
        data.AddCellContent(2, 5, new TileObjectData(1));
        data.AddCellContent(1, 4, new TileObjectData(1));

        data.AddCellContent(1, 1, new BombObjectData(2));


        ChipData[] chips = new ChipData[5];
        for (int i = 0; i < chips.Length; ++i)
            chips[i] = new ChipData(i);

        for (int x = 0; x < 10; ++x)
        {
            data.AddCellContent(x, 9, new EmitterData(chips));
        }

        return data;
    }

    private class ColorData : ColorObjectComponentFeature.IColorData
    {
        public int ColorId { get; }

        public ColorData(int colorId)
        {
            ColorId = colorId;
        }
    }

    private class ChipData : ChipObjectFeature.IChipData
    {
        public string ObjectTypeId => ChipObjectFeature.Name;
        public ColorObjectComponentFeature.IColorData Color { get; }
        public HealthObjectComponentFeature.IHealthData Health { get; }

        public int BodyType => 0;

        public int ColorId => Color.ColorId;

        public ChipData(int colorId)
        {
            Color = new ColorData(colorId);
            Health = new HealthData(1, 1, DamageType.Match | DamageType.Explosion, false);
        }
    }

    private class ChainData : ChainObjectFeature.IChainData
    {
        public string ObjectTypeId => ChainObjectFeature.Name;

        public HealthObjectComponentFeature.IHealthData Health { get; } = new HealthData(10, 3, DamageType.Match | DamageType.Explosion, false);
    }

    private class HealthData : HealthObjectComponentFeature.IHealthData
    {
        public int Priority { get; }
        public int HealthValue { get; }
        public DamageType Vulnerability { get; }

        public bool Fragile { get; }

        public HealthData(int priority, int value, DamageType vulnerability, bool fragile)
        {
            Priority = priority;
            HealthValue = value;
            Vulnerability = vulnerability;
            Fragile = fragile;
        }
    }

    public class TileObjectData : TileObjectFeature.ITileData
    {
        public string ObjectTypeId => TileObjectFeature.Name;

        public int Health { get; }

        public TileObjectData(int health)
        {
            Health = health;
        }
    }

    public class BonusFactory : Factory<ICellObjectData, int>, Match.IBonusFactory
    {
        public BonusFactory(Func<int, ICellObjectData> factory)
            : base(factory)
        {
        }
    }

    public class BombObjectData : BombObjectFeature.IBombData
    {
        public string ObjectTypeId => BombObjectFeature.Name;
        public ColorObjectComponentFeature.IColorData Color { get; }

        public BombObjectData(int colorId)
        {
            Color = new ColorData(colorId);
        }

    }

}
