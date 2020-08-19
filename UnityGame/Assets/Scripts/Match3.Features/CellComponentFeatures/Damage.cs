namespace Match3.Features
{
    public enum DamageType
    {
        Invalid = 0,
        Match = 1,
        MatchSibling = 2,
        Explosion = 4,
    }

    public struct Damage
    {
        public readonly DamageType Type;
        public readonly int Value;

        public Damage(DamageType type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}