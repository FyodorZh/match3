using System.Collections.Generic;
using Match3.Features;
using UnityEngine;

namespace Match3.View.Objects
{
    public class ChainView : CellObjectView
    {
        public List<Texture2D> _textures;

        private ChainObjectFeature.IChain _chain;
        private int _health;

        protected override void OnInit()
        {
            base.OnInit();

            _chain = (ChainObjectFeature.IChain)Owner;
            _health = _chain.Health.HealthValue;
            gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
        }

        protected override void Update()
        {
            base.Update();
            var newHealth = _chain.Health.HealthValue;
            if (newHealth != _health)
            {
                _health = newHealth;
                gameObject.GetComponent<Renderer>().material.mainTexture = _textures[_health - 1];
            }
        }
    }
}