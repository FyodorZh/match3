using Match3;
using Match3.Core;
using Match3.View;
using UnityEngine;
using ViewFactory = Match3.ViewFactory;

public class GameInit : MonoBehaviour
{
    public ViewFactory _viewFactory;
    public GameView _gameView;

    private void Awake()
    {
        ObjectFactory objectFactory = new ObjectFactory();

        IGameRules rules = new GameRules(objectFactory, _viewFactory, new IFeature[] {});
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
