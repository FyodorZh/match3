using System.Collections.Generic;
using Match3.Features;
using UnityEngine;

namespace Match3.View.Objects
{
    public class TileView : CellObjectView
    {
        public List<Texture2D> _textures;

        private TileObjectFeature.ITile _tile;
        private int _health;

        protected override void OnInit()
        {
            base.OnInit();

            _tile = (TileObjectFeature.ITile)Owner;
            _health = _tile.Health.HealthValue;
            gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
        }

        protected override void Update()
        {
            base.Update();
            var newHealth = _tile.Health.HealthValue;
            if (newHealth != _health)
            {
                _health = newHealth;
                gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
            }
        }
    }
}