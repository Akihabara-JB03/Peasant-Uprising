namespace PeasantUprising.Core;
using YamlDotNet.Serialization;
public class GA // Game Action
{
    private readonly IDeserializer D = new DeserializerBuilder().Build(); /*Deserializer*/
    public string ADD { get; } = Environment.GetFolderPath( /*AppData Directory*/
        Environment.SpecialFolder.LocalApplicationData
    );
        
    public Dictionary<string, object> gotorule()
    {
        string datadirectory = Path.Combine(ADD, "Programs","PeasantUprising","Data");
        string ymlpath = Path.Combine(datadirectory,"commute.yml");
        if (!File.Exists(ymlpath)) return -1;
        string YV = File.ReadAllText(ymlpath); /*Yaml Value*/
        var datas = D.Deserialize<Dictionary<string,object>>(YV);
        foreach (var data in datas)
        {
            
        }
        return 0;
    }
    public bool IsInside(int x, int y)
    {
        return 0 <= x && x < 9 && 0 <= y && y < 3;
    }
}