namespace Match3.Features
{
    public class Emitters : StatelessFeature
    {
        protected override void Process(IGame game, int dTimeMs)
        {
            foreach (var grid in game.Board.Grids)
            {
                foreach (var cell in grid.AllCells)
                {
                    foreach (var obj in cell.Content)
                    {
                        var emitter = obj.TryGetFeature<Emitter>();
                        if (emitter != null)
                        {
                            if (cell.Content.Count == 1)
                            {
                                // TODO EMIT
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}