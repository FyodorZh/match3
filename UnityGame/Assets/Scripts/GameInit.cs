using System;
using Match3;
using Match3.Core;
using Match3.Features;
using Match3.Math;
using Match3.View;
using UnityEngine;
using Debug = Match3.Core.Debug;

public class GameInit : MonoBehaviour
{
    public ViewFactory _viewFactory;
    public GameView _gameView;

    private void Awake()
    {
        // for (int i = 0; i < 100000; ++i)
        // {
        //     Fixed x = new Fixed(i, 100);
        //     Fixed xx = x * x;
        //     Fixed y = FixedMath.Sqrt(xx);
        //     if (y != x)
        //     {
        //         Fixed yy = FixedMath.Sqrt(xx);
        //         throw new Exception();
        //     }
        // }
        
        
        Debug.OnLog += UnityEngine.Debug.Log;
        Debug.OnWarning += UnityEngine.Debug.LogWarning;
        
        IGameRules rules = new GameRules(_viewFactory);
        
        rules.RegisterObjectFeature(ChipObjectFeature.Instance);
        rules.RegisterObjectFeature(ChainObjectFeature.Instance);
        
        rules.RegisterGameFeature(new Emitters());
        rules.RegisterGameFeature(new Gravity());
        rules.RegisterGameFeature(new Match());
        
        rules.RegisterActionFeature(new KillActionFeature());
        rules.RegisterActionFeature(new SwapActionFeature());

        rules.BakeAllFeatures();
        
        IGridData[] data = new IGridData[]
        {
            ConstructGridData(), 
        };
        var game = new Game(rules, data);
        
        game.Start();
        
        var gameView = Instantiate(_gameView).GetComponent<GameView>();
        gameView.name = "Match3";
        gameView.transform.position = Vector3.zero;
        gameView.Setup(game);
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
