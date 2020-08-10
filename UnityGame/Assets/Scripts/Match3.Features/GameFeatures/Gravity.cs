namespace Match3.Features
{
    public sealed class Gravity : GameFeature<Gravity.State>
    {
        public class State
        {
            
        }

        public Gravity() 
            : base("Gravity")
        {
        }

        protected override State ConstructData()
        {
            return new State();
        }

        protected override void Process(IGame game, State state, int dTimeMs)
        {
        }
    }
}