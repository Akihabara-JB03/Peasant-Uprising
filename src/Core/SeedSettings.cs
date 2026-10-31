namespace PeasantUprising.Core;
using System.IO;
using System;
using System.Buffers.Binary;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.VisualBasic;

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
        dataDir = Path.Combine(SaveDirectory, "PeasantUprising");
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
    public UInt128? GetSeed()
    {
        if (File.Exists(dataDir))
        {
            byte[] data = File.ReadAllBytes(dataDir);
            if (data.Length == 12)
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
            nibble = seed >> 124 - 4*i & 0xf;
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
            board.ctn[j,i % 9] = pie;
            if (i % 9 == 0)
            {
                j++;
            }
            
        }
    }
}