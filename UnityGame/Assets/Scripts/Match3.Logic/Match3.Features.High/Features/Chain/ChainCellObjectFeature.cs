using System;
using Match3.Core;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Utils;

namespace Match3.Features.Chain
{
    public interface IChainCellObject : ICellObject, IChainCellObjectObserver
    {
        //IHealthCellObjectComponent Health { get; }
    }

    public interface IChainCellObjectObserver : ICellObjectObserver
    {
        int Health { get; }
    }

    public interface IChainCellObjectData : ICellObjectData
    {
        IHealthCellObjectComponentData Health { get; }
    }

    public class ChainCellObjectData : IChainCellObjectData
    {
        public string ObjectTypeId => ChainCellObjectFeature.Name;

        public IHealthCellObjectComponentData Health { get; }

        public ChainCellObjectData(int level)
        {
            Health = new HealthCellObjectComponentData(
                10,
                level,
                DamageType.Match | DamageType.Explosion,
                false);
        }
    }

    public abstract class ChainCellObjectFeature : CellObjectFeature
    {
        public const string Name = "Chain";

        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class ChainCellObjectFeatureImpl : ChainCellObjectFeature
        {
            private HealthCellObjectComponentFeature _healthComponentFeature;

            public override void Init(IGameRules rules)
            {
                _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
            }

            public override IObject Construct(IObjectData data)
            {
                if (!(data is IChainCellObjectData chainData))
                    throw new InvalidOperationException();

                return new ChainCellObject(chainData, _healthComponentFeature.Construct(chainData.Health));
            }

            private class ChainCellObject : CellObject, IChainCellObject
            {
                private ReleasableBoolAgent _lockForMass;

                public IHealthCellObjectComponent Health { get; }

                int IChainCellObjectObserver.Health => Health.HealthValue;

                public ChainCellObject(IChainCellObjectData data, IHealthCellObjectComponent healthComponent)
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
}