using System;
using System.Collections;
using System.Collections.Generic;
using Match3;
using Match3.Core;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private IGame _game;

    private void Awake()
    {
        ObjectFactory objectFactory = new ObjectFactory();
        ViewFactory viewFactory = new ViewFactory();
        
        
        IGameRules rules = new GameRules(objectFactory, viewFactory, new IFeature[] {});
        ICellGridData[] data = new ICellGridData[]
        {
            new TrivialGridData(1, 1), 
        };
        _game = new Game(rules, data);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _game.Tick((int)(Time.deltaTime * 1000));
    }
}
