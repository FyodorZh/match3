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
        IGameRules rules = new GameRules(new IFeature[] {});
        ICellGridData[] data = new ICellGridData[] { };
        IGame game = new Game(rules, data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
