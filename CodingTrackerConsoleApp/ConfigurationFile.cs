using System.Xml.Serialization;

namespace CodingTrackerConsoleApp;

[XmlRoot("Configuration")]
public class ConfigurationFile
{
    [XmlElement("DatabasePath")]
    public string DatabasePath { get; set; } = string.Empty;

    [XmlElement("ConnectionString")]
    public string ConnectionString { get; set; } = string.Empty;

    public ConfigurationFile()
    {
    }

    public ConfigurationFile(string databasePath, string connectionString)
    {
        DatabasePath = databasePath;
        ConnectionString = connectionString;
    }

    public void Save(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var serializer = new XmlSerializer(typeof(ConfigurationFile));
        using var stream = new FileStream(fullPath, FileMode.Create);
        serializer.Serialize(stream, this);
    }

    public static ConfigurationFile? Load(string path)
    {
        var fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath))
        {
            return null;
        }

        var serializer = new XmlSerializer(typeof(ConfigurationFile));
        using var stream = new FileStream(fullPath, FileMode.Open);
        return serializer.Deserialize(stream) as ConfigurationFile;
    }
}