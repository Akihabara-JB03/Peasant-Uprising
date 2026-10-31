namespace PeasantUprising.Core;
class Piece {
    public int HP { get; set; }
    public int Level { get; set; }
    public int Side { get; set; }
    public int? Type { get; set; }
    public Piece(int y,int x,int? ctn /*Config Type Number*/)
    {
        if (x == 4)
        {
            HP = 0;
            Level = 0;
            Side = -2;
            Type = null;
        } else if (x < 4)
        {
            
        }
    }
}