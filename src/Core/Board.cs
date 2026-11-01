using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace PeasantUprising.Core;

class Board
{
    public Piece?[,] cell { get; set; } = new Piece?[3, 9];
    public int?[,] ctn { get; set; } = new int?[3, 9];
    SeedSettings seedsettings = new SeedSettings();
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
    static void Main() {
        Board board = new Board();
        byte[] seed = {
            0xA8, 0x3C, 0x09, 0xE5,
            0xB2, 0x04, 0x8D, 0x33,
            0x30, 0xC8, 0xE6, 0x91,
            0x5F, 0xAB, 0x04, 0x7D
        };
        board.seedsettings.SetSeed(seed);
        (Board? board,string message) som = board.seedsettings.CBFS(); // status or message
        Console.WriteLine(som.message);
    }
}