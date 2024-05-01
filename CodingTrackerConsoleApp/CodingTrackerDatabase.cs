using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTrackerConsoleApp;

public class CodingTrackerDatabase : IDisposable
{
    private const string ConfigurationPath = "CodingTrackerConfiguration.xml";

    private SqliteConnection? _connection;

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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

    internal void LogCodingTime(DateTime startTime, DateTime endTime)
    {
        _connection?.Open();
        _connection?.Execute("INSERT INTO CodingTracker (StartTime, EndTime) VALUES (@StartTime, @EndTime)", new { StartTime = startTime, EndTime = endTime });
        _connection?.Close();
    }

    internal IEnumerable<(int id, DateTime startTime, DateTime endTime)> GetCodingTimeLogs()
    {
        _connection?.Open();
        var logs = _connection?.Query<(int id, DateTime startTime, DateTime endTime)>("SELECT * FROM CodingTracker");
        _connection?.Close();
        return logs ?? [];
    }
}
