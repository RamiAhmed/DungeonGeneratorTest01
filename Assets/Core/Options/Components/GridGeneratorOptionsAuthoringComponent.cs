namespace Assets.Core.Options.Components
{
    public sealed class GridGeneratorOptionsAuthoringComponent : BaseOptionsAuthoringComponent<GridGeneratorOptions>
    {
        public int gridRows;
        public int gridCols;
        public int cellSize;
        public float sensitivity;
        public int innerloopBatchCount;
        public int startX;
        public int startY;

        protected override GridGeneratorOptions GetOptions(GameObjectConversionSystem conversionSystem)
        {
            return new GridGeneratorOptions
            {
                cellSize = cellSize,
                gridCols = gridCols,
                gridRows = gridRows,
                innerloopBatchCount = innerloopBatchCount,
                sensitivity = sensitivity,
                startX = startX,
                startY = startY
            };
        }
    }
}
