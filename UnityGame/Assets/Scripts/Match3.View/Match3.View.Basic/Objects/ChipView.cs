using System.Collections.Generic;
using Match3.Features;
using Match3.Features.Move;
using UnityEngine;

namespace Match3.View.Default.Objects
{
    public class ChipView : CellObjectView<ChipObjectFeature.IChip>
    {
        public List<Texture2D> _colors;

        public GameObject _moveStateView;

        protected override void OnInit()
        {
            base.OnInit();

            gameObject.GetComponent<Renderer>().material.mainTexture = _colors[Observer.Color.ColorId];
        }

        protected override void Update()
        {
            base.Update();
            var move = Observer.TryGetComponent<IMoveCellObjectComponent>();
            _moveStateView.SetActive(move.IsMoving);
        }
    }
}