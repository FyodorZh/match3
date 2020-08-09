using System.Collections.Generic;
using System.Linq;

namespace Match3
{
    public class CellObject : Match3.Object, ICellObject
    {
        private readonly ICellObjectFeature[] _features;
        
        public CellObject(ObjectTypeId type, IEnumerable<ICellObjectFeature> features) 
            : this(type, features.ToArray())
        {
            _features = features.ToArray();
        }
        
        public CellObject(ObjectTypeId type, params ICellObjectFeature[] features) 
            : base(type)
        {
            _features = features.ToArray();
        }

        protected override void OnRelease()
        {
            // DO NOTHING
        }

        public TCellObjectFeature TryGetFeature<TCellObjectFeature>() 
            where TCellObjectFeature : ICellObjectFeature
        {
            foreach (var feature in _features)
            {
                if (feature is TCellObjectFeature typedFeature)
                {
                    return typedFeature;
                }
            }

            return default;
        }
    }
}