using System;
using Match3.Features;
using Match3.View.Default.Objects;
using Match3.ViewBinding.Default;
using UnityEngine;

namespace Match3.View.Default
{
    public class ViewFactory : MonoBehaviour, IViewFactory
    {
        public EmitterView _emitterPrefab;
        public ChipView _chipPrefab;
        public ChainView _chainPrefab;
        public TileView _tilePrefab;
        public BombView _bombPrefab;

        public IObjectViewBinding Construct(ObjectTypeId typeId)
        {
            IObjectViewBinding view;
            switch (typeId.Id)
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
            return view;
        }
    }
}