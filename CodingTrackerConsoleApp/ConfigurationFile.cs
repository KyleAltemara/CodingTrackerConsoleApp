using System.Xml.Serialization;

namespace CodingTrackerConsoleApp;

/// <summary>
/// Represents the configuration file for the application.
/// </summary>
[XmlRoot("Configuration")]
public class ConfigurationFile
{
    /// <summary>
    /// Gets or sets the path to the database.
    /// </summary>
    [XmlElement("DatabasePath")]
    public string DatabasePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the connection string for the database.
    /// </summary>
    [XmlElement("ConnectionString")]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationFile"/> class.
    /// </summary>
    public ConfigurationFile()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationFile"/> class with the specified database path and connection string.
    /// </summary>
    /// <param name="databasePath">The path to the database.</param>
    /// <param name="connectionString">The connection string for the database.</param>
    public ConfigurationFile(string databasePath, string connectionString)
    {
        DatabasePath = databasePath;
        ConnectionString = connectionString;
    }

    /// <summary>
    /// Saves the configuration file to the specified path.
    /// </summary>
    /// <param name="path">The path to save the configuration file.</param>
    public void Save(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var serializer = new XmlSerializer(typeof(ConfigurationFile));
        using var stream = new FileStream(fullPath, FileMode.Create);
        serializer.Serialize(stream, this);
    }

    /// <summary>
    /// Loads the configuration file from the specified path.
    /// </summary>
    /// <param name="path">The path to load the configuration file from.</param>
    /// <returns>The loaded <see cref="ConfigurationFile"/> object, or null if the file does not exist.</returns>
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
