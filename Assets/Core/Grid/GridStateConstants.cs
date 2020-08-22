namespace Assets.Core.Grid
{
    public static class GridStateConstants
    {
        public const byte FREE = 0;
        public const byte BLOCKED = 1;
        public const byte START = 2;
        public const byte EXIT = 3;

        public const byte FLOODED = byte.MaxValue - 1;
        public const byte UNSET = byte.MaxValue;
    }
}
