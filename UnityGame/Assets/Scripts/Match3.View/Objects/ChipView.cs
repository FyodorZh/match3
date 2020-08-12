using System.Collections.Generic;
using Match3.Features;
using UnityEngine;

namespace Match3.View.Objects
{
    public class ChipView : CellObjectView
    {
        public List<Texture2D> _colors;

        public GameObject _moveStateView;
        protected override void OnInit()
        {
            base.OnInit();

            ChipObjectFeature.IChip chip = (ChipObjectFeature.IChip)Owner;
            gameObject.GetComponent<Renderer>().material.mainTexture = _colors[chip.Color.ColorId];
        }

        protected override void Update()
        {
            base.Update();
            var move = CellObject.TryGetComponent<MoveComponentFeature.IMove>();
            _moveStateView.SetActive(move.IsMoving);
        }
    }
}