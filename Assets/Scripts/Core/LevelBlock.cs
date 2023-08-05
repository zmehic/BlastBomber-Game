namespace BlastBomberV2.Core
{
    public class LevelBlock:ILevelBlock
    {
        public  bool IsWalkable { get; set; }
        public  LevelBlockType BlockType {get;set;}
    }
}
