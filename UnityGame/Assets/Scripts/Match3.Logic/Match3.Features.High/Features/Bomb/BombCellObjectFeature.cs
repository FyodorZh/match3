using System;
using Match3.Core;
using Match3.Features.Color;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Features.Move;

namespace Match3.Features.Bomb
{
    public interface IBombCellObject : ICellObject, IBombCellObjectObserver
    {
        //IColorCellObjectComponent Color { get; }
        //IHealthCellObjectComponent Health { get; }
    }

    public interface IBombCellObjectObserver : ICellObjectObserver
    {
        int Health { get; }
        int ColorId { get; }
    }

    public interface IBombCellObjectData : ICellObjectData
    {
        IColorCellObjectComponentData Color { get; }
    }

    public class BombCellObjectData : IBombCellObjectData
    {
        public string ObjectTypeId => BombCellObjectFeature.Name;
        public IColorCellObjectComponentData Color { get; }

        public BombCellObjectData(int colorId)
        {
            Color = new ColorCellObjectComponentData(colorId);
        }
    }

    public abstract class BombCellObjectFeature : CellObjectFeature
    {
        public const string Name = "Bomb";

        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class BombCellObjectFeatureImpl : BombCellObjectFeature
        {
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
                if (!(data is IBombCellObjectData bombData))
                    throw new InvalidOperationException();

                return new BombCellObject(
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

            private class BombCellObject : CellObject, IBombCellObject
            {
                public IColorCellObjectComponent Color { get; }

                public IHealthCellObjectComponent Health { get; }

                int IBombCellObjectObserver.ColorId => Color.ColorId;

                int IBombCellObjectObserver.Health => Health.HealthValue;

                private bool _countDownMode;
                private DeltaTime _timeTillDestroy;

                public BombCellObject(
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
}