namespace Match3.Features
{
    public class Gravity : Feature<Gravity.State>
    {
        public class State
        {
            
        }

        protected override State ConstructData()
        {
            return new State();
        }

        protected override void Process(IGame game, State state)
        {
            throw new System.NotImplementedException();
        }
    }
}