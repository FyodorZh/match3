namespace Match3.ViewBinding.Default
{
    public class CellObjectViewBinding : ObjectViewBinding
    {
        protected ICellObjectObserver CellObject { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            CellObject = (ICellObjectObserver)Owner;
        }
    }
}