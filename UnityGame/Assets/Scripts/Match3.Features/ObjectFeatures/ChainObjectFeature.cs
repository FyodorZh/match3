using System;
using System.Collections.Generic;
using Match3.Core;
using Match3.Utils;

namespace Match3.Features
{
    public class ChainObjectFeature : IObjectFeature
    {
        public const string Name = "Chain";
        public static readonly ChainObjectFeature Instance = new ChainObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IObjectComponentFeature> DependsOn { get; } = new IObjectComponentFeature[]
        {
            MassObjectComponentFeature.Instance,
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IChainData chainData))
                throw new InvalidOperationException();

            return new Chain(chainData);
        }

        public interface IChain : ICellObject
        {
            HealthObjectComponentFeature.IHealth Health { get; }
        }

        public interface IChainData : ICellObjectData
        {
            HealthObjectComponentFeature.IHealthData Health { get; }
        }

        private class Chain : CellObject, IChain
        {
            private ReleasableBoolAgent _lockForMass;

            private readonly HealthObjectComponentFeature.IHealth _healthComponent;

            public HealthObjectComponentFeature.IHealth Health { get; }

            public Chain(IChainData data)
                : this(data, HealthObjectComponentFeature.Instance.Construct(data.Health))
            {
            }

            private Chain(IChainData data, HealthObjectComponentFeature.IHealth healthComponent)
                : base(new ObjectTypeId(data.TypeId), healthComponent)
            {
                Health = healthComponent;
            }

            protected override void OnChangeOwner(ICell newOwner)
            {
                _lockForMass?.Release();
                if (newOwner != null)
                {
                    var mass = newOwner.FindObjectComponent<MassObjectComponentFeature.IMass>();
                    Debug.Assert(mass != null);
                    mass?.IsLocked.AddAgent(_lockForMass = new ReleasableLock());
                }

                base.OnChangeOwner(newOwner);
            }
        }
    }
}