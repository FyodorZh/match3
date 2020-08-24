namespace Match3
{
    public interface IGameController
    {
        void Start();

        void Tick(DeltaTime dTime);

        void Action(string actionFeatureName, params CellId[] cells);
    }
}