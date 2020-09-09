using System;
using System.Collections.Generic;

namespace Match3.Logic
{
    class ObjectFactory : IObjectFactory
    {
        private readonly Dictionary<string, Func<IObjectData, IObject>> _table =
            new Dictionary<string, Func<IObjectData, IObject>>();

        public void Append(string typeId, Func<IObjectData, IObject> ctor)
        {
            _table.Add(typeId, ctor);
        }

        public IObject Construct(IObjectData data)
        {
            if (data?.ObjectTypeId != null && _table.TryGetValue(data.ObjectTypeId, out var ctor))
            {
                return ctor(data);
            }

            return null;
        }
    }
}