using System;

namespace Match3.Features.Health
{
    public interface IHealthCellObjectComponent : ICellObjectComponent
    {
        event Action<IHealthCellObjectComponent, Damage> DamageApplied;
        int Priority { get; }
        int HealthValue { get; }
        Damage ApplyDamage(Damage damage);
    }

    public interface IHealthCellObjectComponentData : ICellObjectComponentData
    {
        int Priority { get; }
        int HealthValue { get; }
        DamageType Vulnerability { get; }
        bool Fragile { get; }
    }

    public class HealthCellObjectComponentData : IHealthCellObjectComponentData
    {
        public int Priority { get; }
        public int HealthValue { get; }
        public DamageType Vulnerability { get; }
        public bool Fragile { get; }

        public HealthCellObjectComponentData(int priority, int value, DamageType vulnerability, bool fragile)
        {
            Priority = priority;
            HealthValue = value;
            Vulnerability = vulnerability;
            Fragile = fragile;
        }
    }

    public abstract class HealthCellObjectComponentFeature : CellObjectComponentFeature
    {
        public const string Name = "Health";

        public sealed override string FeatureId => Name;

        public abstract IHealthCellObjectComponent Construct(IHealthCellObjectComponentData data);
    }

    namespace Default
    {
        public class HealthCellObjectComponentFeatureImpl : HealthCellObjectComponentFeature
        {
            public override IHealthCellObjectComponent Construct(IHealthCellObjectComponentData data)
            {
                return new Health(data);
            }

            private class Health : CellObjectComponent, IHealthCellObjectComponent
            {
                public event Action<IHealthCellObjectComponent, Damage> DamageApplied;

                public override string TypeId => Name;

                public int Priority { get; }
                public int HealthValue { get; private set; }

                public DamageType Vulnerability { get; }

                public bool Fragile { get; }

                public Health(IHealthCellObjectComponentData data)
                {
                    HealthValue = data.HealthValue;
                    Vulnerability = data.Vulnerability;
                    Priority = data.Priority;
                    Fragile = data.Fragile;
                }

                public Damage ApplyDamage(Damage damage)
                {
                    if ((Vulnerability & damage.Type) != 0)
                    {
                        int d = System.Math.Min(HealthValue, damage.Value);
                        if (d > 0)
                        {
                            HealthValue -= d;
                            DamageApplied?.Invoke(this, damage);
                            if (!Fragile)
                                damage = new Damage(damage.Type, damage.Value - d);

                            if (HealthValue == 0)
                            {
                                Owner.Release();
                            }
                        }
                    }

                    return damage;
                }

                public override ICellObjectComponentData SaveAsData()
                {
                    return new HealthCellObjectComponentData(Priority, HealthValue, Vulnerability, Fragile);
                }
            }
        }
    }
}