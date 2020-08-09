using System.Collections;
using System.Collections.Generic;
using Match3;
using Match3.Core;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IObjectFactory objectFactory = null;
        IViewFactory viewFactory = null;
        IGameRules rules = new GameRules(objectFactory, viewFactory, new IFeature[] {});
        ICellGridData[] data = new ICellGridData[] { };
        IGame game = new Game(rules, data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
