using System;
using Match3.Features;
using Match3.View.Objects;
using UnityEngine;

namespace Match3.View
{
    public class ViewFactory : MonoBehaviour, IViewFactory
    {
        public CellObjectView _emitterPrefab;
        public ChipView _chipPrefab;
        
        public IObjectView Construct(IObject logicObject)
        {
            ObjectView view;
            switch (logicObject.TypeId.Id)
            {
                case EmitterObjectFeature.Name:
                    view = Instantiate(_emitterPrefab);
                    break;
                case ChipObjectFeature.Name:
                    view = Instantiate(_chipPrefab);
                    break;
                default:
                    throw new Exception();
            }
            view.SetOwner(logicObject);
            return view;
        }
    }
}