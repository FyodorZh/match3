using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEngine;

namespace Match3.View
{
    public class ViewFactory : MonoBehaviour, IViewFactory
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IObjectView Construct(IObject logicObject, IGameContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}