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
        public ChainView _chainPrefab;
        public TileView _tilePrefab;
        public BombView _bombPrefab;

        public IObjectView Construct(IObjectObserver logicObject)
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
                case ChainObjectFeature.Name:
                    view = Instantiate(_chainPrefab);
                    break;
                case TileObjectFeature.Name:
                    view = Instantiate(_tilePrefab);
                    break;
                case BombObjectFeature.Name:
                    view = Instantiate(_bombPrefab);
                    break;
                default:
                    throw new Exception();
            }
            view.SetOwner(logicObject);
            return view;
        }
    }
}