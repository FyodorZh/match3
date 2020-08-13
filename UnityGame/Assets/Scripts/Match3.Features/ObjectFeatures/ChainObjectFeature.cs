﻿using System;
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
        
        public IEnumerable<IComponentFeature> DependsOn { get; } = new IComponentFeature[]
        {
            MassComponentFeature.Instance, 
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IChainData chainData))
                throw  new InvalidOperationException();

            return new Chain(chainData);
        }
        
        public interface IChain : ICellObject
        {
        }
        
        public interface IChainData : ICellObjectData
        {
        }

        private class Chain : CellObject, IChain
        {
            private ReleasableBoolAgent _lockOfMass;

            public Chain(IChainData data)
                : base(new ObjectTypeId(data.TypeId))
            {
            }

            protected override void OnChangeOwner(ICell newOwner)
            {
                _lockOfMass?.Release();
                if (newOwner != null)
                {
                    var mass = newOwner.FindComponent<MassComponentFeature.IMass>();
                    Debug.Assert(mass != null);
                    mass?.IsLocked.AddAgent(_lockOfMass = new ReleasableLock());
                }

                base.OnChangeOwner(newOwner);
            }
        }
    }
}