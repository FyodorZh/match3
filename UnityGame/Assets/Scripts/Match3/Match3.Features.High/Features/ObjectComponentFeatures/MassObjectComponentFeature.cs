using System;
using Match3.Utils;

namespace Match3.Features
{
    public class MassObjectComponentFeature : ICellObjectObjectComponentFeature
    {
        public const string Name = "Mass";
        public static readonly MassObjectComponentFeature Instance = new MassObjectComponentFeature();

        public string FeatureId => Name;

        public IMass Construct()
        {
            return new Mass();
        }

        public interface IMass : ICellObjectComponent
        {
            BoolStack IsLocked { get; }
        }

        private class Mass : CellObjectComponent, IMass
        {
            public override string TypeId => Name;

            public Mass()
            {
                IsLocked = new BoolStack();
            }

            public BoolStack IsLocked { get; }
        }
    }
}