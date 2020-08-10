using System;
using Match3.Features;
using UnityEngine;

namespace Match3.View
{
    public class ViewFactory : MonoBehaviour, IViewFactory
    {
        public CellObjectView _emitterPrefab;
        
        public IObjectView Construct(IObject logicObject)
        {
            switch (logicObject.TypeId.Id)
            {
                case EmitterObjectFeature.Name:
                    return Instantiate(_emitterPrefab);
                default:
                    throw new Exception();
            }
        }
    }
}