using System;

namespace Match3.Features
{
    public class HealthObjectComponentFeature : ICellObjectObjectComponentFeature
    {
        public const string Name = "Health";
        public static readonly HealthObjectComponentFeature Instance = new HealthObjectComponentFeature();

        public string FeatureId => Name;

        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IHealthData healthData))
                throw new InvalidOperationException();

            return Construct(healthData);
        }

        public IHealth Construct(IHealthData data)
        {
            return new Health(data);
        }

        public interface IHealth : ICellObjectComponent
        {
            int Priority { get; }
            int HealthValue { get; }
            Damage ApplyDamage(Damage damage);
        }

        public interface IHealthData : ICellObjectComponentData
        {
            int Priority { get; }
            int HealthValue { get; }
            DamageType Vulnerability { get; }
        }

        private class Health : CellObjectComponent, IHealth
        {
            public override string TypeId => Name;

            public int Priority { get; }
            public int HealthValue { get; private set; }

            public DamageType Vulnerability { get; }

            public Health(IHealthData data)
            {
                HealthValue = data.HealthValue;
                Vulnerability = data.Vulnerability;
                Priority = data.Priority;
            }

            public Damage ApplyDamage(Damage damage)
            {
                if ((Vulnerability & damage.Type) != 0)
                {
                    int d = System.Math.Min(HealthValue, damage.Value);
                    if (d > 0)
                    {
                        HealthValue -= d;
                        return new Damage(damage.Type, damage.Value - d);
                    }
                }

                return damage;
            }
        }
    }
}