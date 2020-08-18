using System;
using System.Collections.Generic;

namespace Match3
{
    public class ObjectFactory : IObjectFactory
    {
        private readonly Dictionary<string, Func<IObjectData, IGameContext, IObject>> _table =
            new Dictionary<string, Func<IObjectData, IGameContext, IObject>>();

        public void Append(string typeId, Func<IObjectData, IGameContext, IObject> ctor)
        {
            _table.Add(typeId, ctor);
        }

        public IObject Construct(IObjectData data, IGameContext context)
        {
            if (data?.ObjectTypeId != null && _table.TryGetValue(data.ObjectTypeId, out var ctor))
            {
                return ctor(data, context);
            }

            return null;
        }
    }
}