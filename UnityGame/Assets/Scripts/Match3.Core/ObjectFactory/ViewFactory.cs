using System;
using System.Collections.Generic;

namespace Match3
{
    public class ViewFactory : IViewFactory
    {
        private readonly Dictionary<string, Func<IObject, IGameContext, IObjectView>> _table =
            new Dictionary<string, Func<IObject, IGameContext, IObjectView>>();

        public void Append(string typeId, Func<IObject, IGameContext, IObjectView> ctor)
        {
            _table.Add(typeId, ctor);
        }
        
        public IObjectView Construct(IObject logicObject, IGameContext context)
        {
            string typeId = logicObject?.Type.Id;
            if (typeId != null && _table.TryGetValue(typeId, out var ctor))
            {
                return ctor(logicObject, context);
            }

            return null;
        }
    }
}