using System;
using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public class BombObjectFeature : IObjectFeature
    {
        public const string Name = "Bomb";
        public static readonly BombObjectFeature Instance = new BombObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IObjectComponentFeature> DependsOn { get; } = new IObjectComponentFeature[]
        {
            ColorObjectComponentFeature.Instance,
            MassObjectComponentFeature.Instance,
            MoveObjectComponentFeature.Instance,
            HealthObjectComponentFeature.Instance,
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IBombData bombData))
                throw new InvalidOperationException();

            return new Bomb(bombData);
        }

        public interface IBomb : ICellObject
        {
            ColorObjectComponentFeature.IColor Color { get; }
            HealthObjectComponentFeature.IHealth Health { get; }
        }

        public interface IBombData : ICellObjectData
        {
            ColorObjectComponentFeature.IColorData Color { get; }
        }

        private class Bomb : CellObject, IBomb
        {
            private class HealthData : HealthObjectComponentFeature.IHealthData
            {
                public int Priority => 1;
                public int HealthValue => 2;
                public DamageType Vulnerability => DamageType.Match | DamageType.Explosion;
                public bool Fragile => false;
            }

            public ColorObjectComponentFeature.IColor Color { get; }

            public HealthObjectComponentFeature.IHealth Health { get; }

            private bool _countDownMode;
            private Fixed _timeTillDestroy;

            public Bomb(IBombData data)
                : this(new ObjectTypeId(data.ObjectTypeId),
                    ColorObjectComponentFeature.Instance.Construct(data.Color),
                    MassObjectComponentFeature.Instance.Construct(),
                    MoveObjectComponentFeature.Instance.Construct(),
                    HealthObjectComponentFeature.Instance.Construct(new HealthData()))
            {
            }

            private Bomb(
                ObjectTypeId typeId,
                ColorObjectComponentFeature.IColor color,
                MassObjectComponentFeature.IMass mass,
                MoveObjectComponentFeature.IMove move,
                HealthObjectComponentFeature.IHealth health)
                : base(typeId, color, mass, move, health)
            {
                Color = color;
                Health = health;
            }

            protected override void OnTick(Fixed dTimeSeconds)
            {
                base.OnTick(dTimeSeconds);
                if (!_countDownMode)
                {
                    if (Health.HealthValue == 1)
                    {
                        _countDownMode = true;
                        _timeTillDestroy = 2; // sec

                        Explode();
                    }
                }
                else
                {
                    _timeTillDestroy -= dTimeSeconds;
                    if (_timeTillDestroy <= 0)
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