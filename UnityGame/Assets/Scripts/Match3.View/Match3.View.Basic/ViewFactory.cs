using System;
using Match3.Features.Bomb;
using Match3.Features.Chain;
using Match3.Features.Chip;
using Match3.Features.Emitter;
using Match3.Features.Tile;
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
                case EmitterCellObjectFeature.Name:
                    view = Instantiate(_emitterPrefab);
                    break;
                case ChipCellObjectFeature.Name:
                    view = Instantiate(_chipPrefab);
                    break;
                case ChainCellObjectFeature.Name:
                    view = Instantiate(_chainPrefab);
                    break;
                case TileCellObjectFeature.Name:
                    view = Instantiate(_tilePrefab);
                    break;
                case BombCellObjectFeature.Name:
                    view = Instantiate(_bombPrefab);
                    break;
                default:
                    throw new Exception();
            }
            return view;
        }
    }
}