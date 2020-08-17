﻿using System.Collections.Generic;
using Match3;
using Match3.Core;
using Match3.Features;
using Match3.Features.CellComponentFeatures;
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

        rules.RegisterCellComponentFeature(HealthCellComponentFeature.Instance);

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

    class EmitterData : EmitterObjectFeature.IEmitterObjectData, EmitterObjectComponentFeature.IEmitterData
    {
        public string TypeId => EmitterObjectFeature.Name;
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
        private class ColorData : ColorObjectComponentFeature.IColorData
        {
            public string TypeId => ColorObjectComponentFeature.Name;
            public int ColorId { get; }

            public ColorData(int colorId)
            {
                ColorId = colorId;
            }
        }

        private class MassData : MassObjectComponentFeature.IMassData
        {
            public string TypeId => MassObjectComponentFeature.Name;
        }

        private class MoveData : MoveObjectComponentFeature.IMoveData
        {
            public string TypeId => MoveObjectComponentFeature.Name;
        }

        public string TypeId => ChipObjectFeature.Name;
        public ColorObjectComponentFeature.IColorData Color { get; }
        public MoveObjectComponentFeature.IMoveData Movement { get; }

        public MassObjectComponentFeature.IMassData Mass { get; }
        public HealthObjectComponentFeature.IHealthData Health { get; }

        public int BodyType => 0;

        public int ColorId => Color.ColorId;

        public ChipData(int colorId)
        {
            Color = new ColorData(colorId);
            Mass = new MassData();
            Movement = new MoveData();
            Health = new HealthData(1, 1, DamageType.Match);
        }
    }

    private class ChainData : ChainObjectFeature.IChainData
    {
        public string TypeId => ChainObjectFeature.Name;

        public HealthObjectComponentFeature.IHealthData Health { get; } = new HealthData(10, 3, DamageType.Match);
    }

    private class HealthData : HealthObjectComponentFeature.IHealthData
    {
        public string TypeId => HealthObjectComponentFeature.Name;
        public int Priority { get; }
        public int HealthValue { get; }
        public DamageType Vulnerability { get; }

        public HealthData(int priority, int value, DamageType vulnerability)
        {
            Priority = priority;
            HealthValue = value;
            Vulnerability = vulnerability;
        }
    }

}
