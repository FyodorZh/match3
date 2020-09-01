using System.Collections.Generic;
using Match3.Features;
using UnityEngine;

namespace Match3.View.Default.Objects
{
    public class BombView : CellObjectView
    {
        public List<Texture2D> _colors;

        public GameObject _particles;

        private BombObjectFeature.IBomb _bomb;

        protected override void OnInit()
        {
            base.OnInit();

            _bomb = (BombObjectFeature.IBomb)Owner;
            Debug.Log("Bomb color " + _bomb.Color.ColorId);
            gameObject.GetComponent<Renderer>().material.mainTexture = _colors[_bomb.Color.ColorId];
        }

        protected override void Update()
        {
            base.Update();
            _particles.SetActive(_bomb.Health.HealthValue == 1);
        }
    }
}