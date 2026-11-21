namespace PeasantUprising.Core;

public class Board
{
    public Piece?[,] cell { get; set; } = new Piece?[3, 9];
    public int?[,] ctn { get; set; } = new int?[3, 9];
    public int ctnlonocm(int ctn) // Count the number of pieces of a specified type owned by the player.
    {
        int count = 0;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (this.ctn[y, x] == ctn && 0 <= x && x <= 3)
                {
                    count++;
                }
            }
        }

        return count;
    }
    public int ctnlonoce(int ctn) // Count the number of pieces of a specified type owned by the enemy.
    {
        int count = 0;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (this.ctn[y, x] == ctn && 5 <= x && x <= 8)
                {
                    count++;
                }
            }
        }

        return count;
    }
    public int empty_ctn_count()
    {
        int count = 0;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (ctn[y, x] == null)
                {
                    count++;
                }
            }
        }

        return count;
    }
}