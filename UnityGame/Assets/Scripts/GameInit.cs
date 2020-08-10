using Match3;
using Match3.Core;
using Match3.Features;
using Match3.View;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public ViewFactory _viewFactory;
    public GameView _gameView;

    private void Awake()
    {  
        IGameRules rules = new GameRules(_viewFactory);
        
        rules.RegisterObjectFeature(ChipObjectFeature.Instance);
        
        rules.RegisterGameFeature(new Emitters());
        rules.RegisterGameFeature(new Gravity());
        
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

    // Start is called before the first frame update
    void Start()
    {

    }

    class EmitterData : EmitterObjectFeature.IEmitterObjectData, EmitterComponentFeature.IEmitterData
    {
        public string TypeId => EmitterObjectFeature.Name;
        public EmitterComponentFeature.IEmitterData Data => this;
        public ICellObjectData ObjectToEmit { get; }

        public EmitterData(ICellObjectData objectToEmit)
        {
            ObjectToEmit = objectToEmit;
        }
    }
    
    private TrivialGridData ConstructGridData()
    {
        var data = new TrivialGridData(10, 10);
        for (int x = 0; x < 10; ++x)
            for (int y = 0; y < 10; ++y)
                if ((x + y) % 2 == 0)
                    data.ActivateCell(x, y);

        for (int x = 0; x < 10; ++x)
        {
            data.AddCellContent(x, 9, new EmitterData(new ChipData(x)));
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

        public string TypeId => ChipObjectFeature.Name;
        public ColorComponentFeature.IColorData Color { get; }

        public MassComponentFeature.IMassData Mass { get; }
        
        public int BodyType => 0;

        public int ColorId => Color.ColorId;

        public ChipData(int colorId)
        {
            Color = new ColorData(colorId % 5);
            Mass = new MassData();
        }
    }
}
