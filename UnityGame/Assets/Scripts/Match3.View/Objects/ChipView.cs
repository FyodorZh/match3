using System.Collections.Generic;
using Match3.Features;
using UnityEngine;

namespace Match3.View.Objects
{
    public class ChipView : CellObjectView
    {
        public List<Texture2D> _colors;
        protected override void OnInit()
        {
            base.OnInit();

            ChipObjectFeature.IChip chip = (ChipObjectFeature.IChip)Owner;
            gameObject.GetComponent<Renderer>().material.mainTexture = _colors[chip.Color.ColorId];
        }
    }
}