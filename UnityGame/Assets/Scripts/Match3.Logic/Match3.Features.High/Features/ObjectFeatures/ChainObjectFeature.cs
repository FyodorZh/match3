using System;
using Match3.Core;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Utils;

namespace Match3.Features
{
    public class ChainObjectFeature : CellObjectFeature
    {
        public const string Name = "Chain";
        public static readonly ChainObjectFeature Instance = new ChainObjectFeature();

        public override string FeatureId => Name;

        private HealthCellObjectComponentFeature _healthComponentFeature;

        public override void Init(IGameRules rules)
        {
            _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
        }

        public override IObject Construct(IObjectData data)
        {
            if (!(data is IChainData chainData))
                throw new InvalidOperationException();

            return new Chain(chainData, _healthComponentFeature.Construct(chainData.Health));
        }

        public interface IChain : ICellObject
        {
            IHealthCellObjectComponent Health { get; }
        }

        public interface IChainData : ICellObjectData
        {
            IHealthCellObjectComponentData Health { get; }
        }

        private class Chain : CellObject, IChain
        {
            private ReleasableBoolAgent _lockForMass;

            private readonly IHealthCellObjectComponent _healthComponent;

            public IHealthCellObjectComponent Health { get; }

            public Chain(IChainData data, IHealthCellObjectComponent healthComponent)
                : base(new ObjectTypeId(data.ObjectTypeId), healthComponent)
            {
                Health = healthComponent;
            }

            protected override void OnChangeOwner(ICell newOwner)
            {
                _lockForMass?.Release();
                if (newOwner != null)
                {
                    var mass = newOwner.FindObjectComponent<IMassCellObjectComponent>();
                    Debug.Assert(mass != null);
                    mass?.IsLocked.AddAgent(_lockForMass = new ReleasableLock());
                }

                base.OnChangeOwner(newOwner);
            }
        }
    }
}