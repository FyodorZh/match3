using System.Collections.Generic;
using Match3.Features.Bomb;
using UnityEngine;

namespace Match3.View.Default.Objects
{
    public class BombView : CellObjectView<IBombCellObjectObserver>
    {
        public List<Texture2D> _colors;

        public GameObject _particles;

        protected override void OnInit()
        {
            base.OnInit();

            Debug.Log("Bomb color " + Observer.ColorId);
            gameObject.GetComponent<Renderer>().material.mainTexture = _colors[Observer.ColorId];
        }

        protected override void Update()
        {
            base.Update();
            _particles.SetActive(Observer.Health == 1);
        }
    }
}