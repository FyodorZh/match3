using System.Collections.Generic;
using Match3.Features.Tile;
using UnityEngine;

namespace Match3.View.Default.Objects
{
    public class TileView : CellObjectView<ITileCellObjectObserver>
    {
        public List<Texture2D> _textures;

        private int _health;

        protected override void OnInit()
        {
            base.OnInit();

            _health = Observer.Health;
            gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
        }

        protected override void Update()
        {
            base.Update();
            var newHealth = Observer.Health;
            if (newHealth != _health)
            {
                _health = newHealth;
                gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
            }
        }
    }
}