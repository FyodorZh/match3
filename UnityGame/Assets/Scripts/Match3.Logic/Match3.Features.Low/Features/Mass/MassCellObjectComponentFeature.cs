using Match3.Utils;

namespace Match3.Features.Mass
{
    public interface IMassCellObjectComponent : ICellObjectComponent
    {
        BoolStack IsLocked { get; }
    }

    public abstract class MassCellObjectComponentFeature : CellObjectComponentFeature
    {
        public const string Name = "Mass";

        public sealed override string FeatureId => Name;

        public abstract IMassCellObjectComponent Construct();
    }

    namespace Default
    {
        public class MassCellObjectComponentFeatureImpl : MassCellObjectComponentFeature
        {
            public override IMassCellObjectComponent Construct()
            {
                return new Component();
            }

            private class Component : CellObjectComponent, IMassCellObjectComponent
            {
                public override string TypeId => Name;

                public Component()
                {
                    IsLocked = new BoolStack();
                }

                public BoolStack IsLocked { get; }

                public override ICellObjectComponentData SaveAsData()
                {
                    return VoidCellObjectComponentData.Instance;
                }
            }
        }
    }
}