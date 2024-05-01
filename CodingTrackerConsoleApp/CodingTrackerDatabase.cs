using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTrackerConsoleApp;

/// <summary>
/// Represents a database for tracking coding sessions.
/// </summary>
public class CodingTrackerDatabase : IDisposable
{
    private const string ConfigurationPath = "CodingTrackerConfiguration.xml";

    private SqliteConnection? _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodingTrackerDatabase"/> class.
    /// </summary>
    public CodingTrackerDatabase()
    {
        var config = ConfigurationFile.Load(ConfigurationPath);
        if (config is null)
        {
            var dbFileName = "CodingTrackerDatabase.db";
            config = new ConfigurationFile(dbFileName, $"Data Source={dbFileName}");
            config.Save(ConfigurationPath);
        }

        _connection = new SqliteConnection(config.ConnectionString);
        _connection.Open();
        _connection.Execute("CREATE TABLE IF NOT EXISTS CodingTracker (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime TEXT NOT NULL, Endtime TEXT NOT NULL)");
        _connection.Close();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="CodingTrackerDatabase"/> object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="CodingTrackerDatabase"/> object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }

    /// <summary>
    /// Logs the coding time for a coding session.
    /// </summary>
    /// <param name="codingSession">The coding session to log.</param>
    internal void LogCodingTime(CodingSession codingSession)
    {
        _connection?.Open();
        _connection?.Execute("INSERT INTO CodingTracker (StartTime, EndTime) VALUES (@StartTime, @EndTime)", codingSession);
        _connection?.Close();
    }

    /// <summary>
    /// Retrieves the coding time logs from the database.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="CodingSession"/> objects representing the coding time logs.</returns>
    internal IEnumerable<CodingSession> GetCodingTimeLogs()
    {
        _connection?.Open();
        var logs = _connection?.Query<CodingSession>("SELECT Id, StartTime, EndTime FROM CodingTracker");
        _connection?.Close();
        return logs ?? [];
    }
}
