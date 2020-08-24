using System;

namespace Match3.Features
{
    public class HealthObjectComponentFeature : ICellObjectObjectComponentFeature
    {
        public const string Name = "Health";
        public static readonly HealthObjectComponentFeature Instance = new HealthObjectComponentFeature();

        public string FeatureId => Name;

        public IHealth Construct(IHealthData data)
        {
            return new Health(data);
        }

        public interface IHealth : ICellObjectComponent
        {
            event Action<IHealth, Damage> DamageApplied;
            int Priority { get; }
            int HealthValue { get; }
            Damage ApplyDamage(Damage damage);
        }

        public interface IHealthData : ICellObjectComponentData
        {
            int Priority { get; }
            int HealthValue { get; }
            DamageType Vulnerability { get; }
            bool Fragile { get; }
        }

        private class Health : CellObjectComponent, IHealth
        {
            public event Action<IHealth, Damage> DamageApplied;

            public override string TypeId => Name;

            public int Priority { get; }
            public int HealthValue { get; private set; }

            public DamageType Vulnerability { get; }

            public bool Fragile { get; }

            public Health(IHealthData data)
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
        }
    }
}