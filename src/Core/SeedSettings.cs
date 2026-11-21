namespace PeasantUprising.Core;
using System.IO;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration;

/*
  ____________    __________    __________    ________
 / ___________|  |  ________|  |  ________|  |  ____  \
/ /___________   | |________   | |________   | |     \ \
\ ___________ \  |  ________|  |  ________|  | |      | |
 ___________/ /  | |________   | |________   | |_____/ /
|____________/   |__________|  |__________|  |________/
*/
class SeedSettings {

    public string SaveDirectory { get; } = Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData
    );
    public string dataDir {get; }
    private IConfigurationRoot config;
    public SeedSettings()
    {
        dataDir = Path.Combine(SaveDirectory, "Programs","PeasantUprising");
        string path = Path.Combine(
            dataDir,
            "Data",
            "config.ini"
        );
        dataDir = Path.Combine(dataDir,"Data","seed.dat");
        config = new ConfigurationBuilder()
            .AddIniFile(path)
            .Build();
    }
    public int empty_seed(UInt128 seed)
    {
        ushort nibble;
        int empty_count = 3;
        for (int i = 0; i < 24; i++)
        {
            nibble = (ushort)((seed >> 124 - 4*i) & 0xfUL);
            if (nibble == 0 || nibble == 7)
            {
                empty_count++;
            }
        }
        return empty_count;
    }
    public UInt128? GetSeed()
    {
        if (File.Exists(dataDir))
        {
            byte[] data = File.ReadAllBytes(dataDir);
            if (data.Length == 16)
            {
                return BinaryPrimitives.ReadUInt128BigEndian(data);
            }
        }
        return null;
    }
    public (bool,string) SetSeed(byte[] bytes)
    {
        if (File.Exists(dataDir))
        {
            byte[] sb = new byte[16]; // Seed Bytes
            if (bytes.Length == 16)
            {
                UInt128 wd = BinaryPrimitives.ReadUInt128BigEndian(bytes); /*Writing data*/
                BinaryPrimitives.WriteUInt128BigEndian(sb,wd);
                File.WriteAllBytes(dataDir,sb);
                return (true, "シード値の書き込みに成功しました。");
            }
             else
            {
                return (false, "書き込んでいるバイト数が足りません。");
            }
        } else
        {
            return (false, "シード値用ファイルがありません。");
        }
        
    }
    public (Board?,string) CBFS() // Create Board From Seed
    {
        UInt128? sos = GetSeed(); /*status or seed*/
        int j = 0;
        if (sos == null)
        {
            return (null,"シード値がないか壊れています。");
        }
        UInt128 seed = sos.Value;
        Board board = new Board();
        ushort nibble;
        int? pie; //piece
        for (int i = 0; i <= 23; i++) {
            nibble = (ushort)((seed >> 124 - 4*i) & 0xfUL);
            pie = nibble switch
            {
                0 or 7 => null,
                1 or 10 => 4,
                2 or 11 => 5,
                3 or 12 => 6,
                4 or 13 => 7,
                5 or 14 => 8,
                6 or 15 => 9,
                8 => 2,
                9 => 3,
                _ => throw new ArgumentOutOfRangeException()
            };
            int x = i % 8;
            bool isLastColumn = x == 7;
            if (x >= 4)
            {
                x++;
            }
            board.ctn[j,x] = pie;
            if (isLastColumn)
            {
                j++;
            }
        }
        List<bool> status = [];
        for (int k = 2; k <= 9; k++)
        {
            int result1;
            int.TryParse(config[$"PL:{k}"],out result1);
            Console.WriteLine(
                $"Type {k}: Player={board.ctnlonocm(k)}, Enemy={board.ctnlonoce(k)}, Limit={result1}"
            );
            if (board.ctnlonocm(k) >= 0 && board.ctnlonocm(k) <= result1 && board.ctnlonoce(k) >= 0 && board.ctnlonoce(k) <= result1)
            {
                status.Add(true);
            }
             else
            {
              status.Add(false);
            }
        }
        int ebpl = int.Parse(config["EBPL:null"]!);
        bool noError = status.All(x => x) && status.Count >= 8;
        Console.WriteLine(
            $"Type null: Count={board.empty_ctn_count()}, Limit={ebpl}"
        );
        if (noError && board.empty_ctn_count() <= ebpl)
        {
            return (board, "駒の配置に成功しました！！");
        }
        return (null, "公平性の観点から偏っているとみなされました。シード値を変更してください。");
    }
    public (Board?, string) FGBGR() /*Fairness-Guaranteed Board Generation with Randomization*/
    {
        UInt128 seed = new UInt128(0x1111_1111_1111_1111UL, 0x1111_1111_0000_0000UL);
        Board board = new Board();
        Random random = new Random();
        Dictionary<int,int> enemy_count = new();
        Dictionary<int,int> player_count = new();
        for (int i = 2; i <= 9; i++)
        {
            enemy_count[i] = 0;
            player_count[i] = 0;
        }
        int? selected_Type;
        int candidate_count;
        for (int i = 0; i <= 23; i++)
        {   
            int y = i / 8;
            int x = i % 8;
            selected_Type = null;
            candidate_count = 0;
            bool isPlayer = i % 8 < 4;
            if (x >= 4) x++;
            
            if (!isPlayer) {
                int maxempt;
                int.TryParse(config[$"EBPL:null"],out maxempt);
                if (empty_seed(seed) < maxempt) {
                    if (random.Next(0,9) == 0)
                    {
                        selected_Type = null;
                        goto skip_enemy;
                    }
                }
                for (int j = 2; j <= 9; j++) {
                    int result;
                    int.TryParse(config[$"PL:{j}"],out result);
                    if (enemy_count[j] < result)
                    {
                        candidate_count++;
                        if (random.Next(candidate_count) == 0)
                        {
                            selected_Type = j;
                        }
                    }
                }
                skip_enemy:
                board.ctn[y, x] = selected_Type;
                if (selected_Type != null) {
                    enemy_count[(int)selected_Type]++;
                }
                
            }
             else
            {
                int maxempt;
                int.TryParse(config[$"EBPL:null"],out maxempt);
                if (empty_seed(seed) < maxempt)
                    if (random.Next(0,9) == 0)
                    {
                        selected_Type = null;
                        goto skip_player;
                    }
                for (int j = 2; j <= 9; j++) {
                    int result;
                    int.TryParse(config[$"PL:{j}"],out result);
                    if (player_count[j] < result)
                    {
                        candidate_count++;
                        if (random.Next(candidate_count) == 0)
                        {
                            selected_Type = j;
                        }
                    }
                }
                skip_player:
                board.ctn[y, x] = selected_Type;
                if (selected_Type != null) {
                    player_count[(int)selected_Type]++;
                }
            }
            UInt128 nibble = (UInt128)(selected_Type switch
            {
                null => 0,
                4 => 1,
                5 => 2,
                6 => 3,
                7 => 4,
                8 => 5,
                9 => 6,
                2 => 8,
                3 => 9,
                _ => 0
            });
            int shift = 124 - 4 * i;
            seed &= ~((UInt128)0xF << shift);
            seed |= nibble << (124 - 4 * i);
        }
        byte[] sb = new byte[16];
        BinaryPrimitives.WriteUInt128BigEndian(sb, seed);
        SetSeed(sb);
        return CBFS();
    }
}