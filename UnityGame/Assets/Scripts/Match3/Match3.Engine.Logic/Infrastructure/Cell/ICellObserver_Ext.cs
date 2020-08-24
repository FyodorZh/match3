namespace Match3
{
    public static class ICellObserver_Ext
    {
        public static TCellObjectComponent FindObjectComponent<TCellObjectComponent>(this ICellObserver cell)
            where TCellObjectComponent : class, ICellObjectComponentObserver
        {
            foreach (var obj in cell.Objects)
            {
                var component = obj.TryGetComponent<TCellObjectComponent>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}