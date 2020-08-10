using Match3;
using Match3.Core;
using Match3.Features;
using Match3.View;
using UnityEngine;
using ViewFactory = Match3.ViewFactory;

public class GameInit : MonoBehaviour
{
    public ViewFactory _viewFactory;
    public GameView _gameView;

    private void Awake()
    {
        IGameRules rules = new GameRules(_viewFactory);
        rules.RegisterGameFeature(new Emitters());
        rules.RegisterGameFeature(new Gravity());
        rules.BakeAllFeatures();
        
        ICellGridData[] data = new ICellGridData[]
        {
            ConstructGridData(), 
        };
        var game = new Game(rules, data);
        
        var gameView = Instantiate(_gameView).GetComponent<GameView>();
        gameView.name = "Match3";
        gameView.transform.position = Vector3.zero;
        gameView.Setup(game);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private TrivialGridData ConstructGridData()
    {
        var data = new TrivialGridData(10, 10);
        
        return data;
    }
}
