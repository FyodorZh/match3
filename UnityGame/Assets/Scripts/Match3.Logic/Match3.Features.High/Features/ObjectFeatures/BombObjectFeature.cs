using System;
using Match3.Core;
using Match3.Features.Color;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Features.Move;

namespace Match3.Features
{
    public class BombObjectFeature : CellObjectFeature
    {
        public const string Name = "Bomb";
        public static readonly BombObjectFeature Instance = new BombObjectFeature();

        public override string FeatureId => Name;

        private ColorCellObjectComponentFeature _colorComponentFeature;
        private MassCellObjectComponentFeature _massComponentFeature;
        private MoveCellObjectComponentFeature _moveComponentFeature;
        private HealthCellObjectComponentFeature _healthComponentFeature;

        public override void Init(IGameRules rules)
        {
            _colorComponentFeature = rules.GetCellObjectComponentFeature<ColorCellObjectComponentFeature>(ColorCellObjectComponentFeature.Name);
            _massComponentFeature = rules.GetCellObjectComponentFeature<MassCellObjectComponentFeature>(MassCellObjectComponentFeature.Name);
            _moveComponentFeature = rules.GetCellObjectComponentFeature<MoveCellObjectComponentFeature>(MoveCellObjectComponentFeature.Name);
            _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
        }

        public override IObject Construct(IObjectData data)
        {
            if (!(data is IBombData bombData))
                throw new InvalidOperationException();

            return new Bomb(
                new ObjectTypeId(bombData.ObjectTypeId),
                _colorComponentFeature.Construct(bombData.Color),
                _massComponentFeature.Construct(),
                _moveComponentFeature.Construct(),
                _healthComponentFeature.Construct(
                    new HealthCellObjectComponentData(
                        1,
                        2,
                        DamageType.Match | DamageType.Explosion,
                        false
                        )
                    )
                );
        }

        public interface IBomb : ICellObject
        {
            IColorCellObjectComponent Color { get; }
            IHealthCellObjectComponent Health { get; }
        }

        public interface IBombData : ICellObjectData
        {
            IColorCellObjectComponentData Color { get; }
        }

        private class Bomb : CellObject, IBomb
        {
            public IColorCellObjectComponent Color { get; }

            public IHealthCellObjectComponent Health { get; }

            private bool _countDownMode;
            private DeltaTime _timeTillDestroy;

            public Bomb(
                ObjectTypeId typeId,
                IColorCellObjectComponent color,
                IMassCellObjectComponent mass,
                IMoveCellObjectComponent move,
                IHealthCellObjectComponent health)
                : base(typeId, color, mass, move, health)
            {
                Color = color;
                Health = health;
            }

            protected override void OnTick(DeltaTime dTime)
            {
                base.OnTick(dTime);
                if (!_countDownMode)
                {
                    if (Health.HealthValue == 1)
                    {
                        _countDownMode = true;
                        _timeTillDestroy = new DeltaTime(2000);

                        Explode();
                    }
                }
                else
                {
                    _timeTillDestroy -= dTime;
                    if (_timeTillDestroy <= DeltaTime.Zero)
                    {
                        Explode();
                        Health.ApplyDamage(new Damage(DamageType.Match, 1));
                    }
                }
            }

            private void Explode()
            {
                foreach (var neighbour in Owner.ActiveNeighboursInBox())
                {
                    var health = neighbour.FindComponent<HealthCellComponentFeature.IHealth>();
                    health.ApplyDamage(new Damage(DamageType.Explosion, 1));
                }
            }
        }
    }
}