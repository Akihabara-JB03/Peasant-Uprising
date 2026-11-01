namespace PeasantUprising.Core;
class Piece {
    public int? HP { get; set; }
    public int? Level { get; set; }
    public int Side { get; set; }
    public int? Type { get; set; }
    public Piece(int x,int? ctn /*Config Type Number*/)
    {
        if (x == 4)
        {
            HP = null;
            Level = null;
            Side = 0;
            Type = null;
        }
         else if (x < 4)
        {
            HP = ctn;
            if (ctn == null)
            {
                Level = null;
            }
             else
            {
                Level = 1;
            }
            Side = -1;
            Type = ctn;
        } else
        {
            HP = ctn;
            if (ctn == null)
            {
                Level = null;
            }
             else
            {
                Level = 1;
            }
            Side = 1;
            Type = ctn;
        }
    }
}