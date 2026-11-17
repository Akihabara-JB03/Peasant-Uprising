using PeasantUprising.Core;
class Game
{
    static void Main()
    {
        SeedSettings seedsettings = new SeedSettings();
        var (board, message) = seedsettings.FGBGR();
        if (board != null)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board.cell[i,j] = new Piece(j,board.ctn[i,j]);
                }
            }
        }
    }
}