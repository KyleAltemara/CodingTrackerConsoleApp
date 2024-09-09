using Spectre.Console;

namespace CodingTrackerConsoleApp;

/// <summary>
/// Represents the menu of the coding tracker console application.
/// </summary>
public static class Menu
{
    /// <summary>
    /// Displays the main menu and handles user input.
    /// </summary>
    internal static void MainMenu()
    {
        var codingTrackerDatabase = new CodingTrackerDatabase();
        var menuOptions = new Dictionary<string, Action>
            {
                { "Log Coding Time", () => LogCodingTime(codingTrackerDatabase) },
                { "Start Coding Session", () => StartStopwatch(codingTrackerDatabase) },
                { "Print Coding Time Logs", () => PrintLogs(codingTrackerDatabase) },
                { "Exit", () => Environment.Exit(0) },
            };

        var menu = new SelectionPrompt<string>()
            .Title("[bold]Coding Tracker Menu[/]")
            .AddChoices(menuOptions.Keys);

        while (true)
        {
            string choice = AnsiConsole.Prompt(menu);
            menuOptions[choice]();
        }
    }

    /// <summary>
    /// Logs the coding time by prompting the user for start and end date and time.
    /// </summary>
    /// <param name="codingTrackerDatabase">The coding tracker database.</param>
    public static void LogCodingTime(CodingTrackerDatabase codingTrackerDatabase)
    {
        AnsiConsole.MarkupLine("[bold]Log Coding Time[/]");
        AnsiConsole.MarkupLine("Enter the start date and time:");
        DateTime? startTime = GetDateTime();
        if (startTime is null || !startTime.HasValue)
        {
            return;
        }

        AnsiConsole.MarkupLine("Enter the end date and time:");
        DateTime? endTime = GetDateTime();
        if (endTime is null || !endTime.HasValue)
        {
            return;
        }

        while (endTime < startTime)
        {
            AnsiConsole.MarkupLine("[bold red]End time must be after start time.");
            endTime = GetDateTime();
            if (endTime is null || !endTime.HasValue)
            {
                return;
            }
        }

        codingTrackerDatabase.LogCodingTime(new CodingSession { StartTime = startTime.Value, EndTime = endTime.Value });
    }

    /// <summary>
    /// Prompts the user to enter a date and time and returns the parsed DateTime value.
    /// </summary>
    /// <returns>The parsed DateTime value or null if the input is invalid or empty.</returns>
    public static DateTime? GetDateTime()
    {
        DateTime? date = null;
        while (!date.HasValue)
        {
            try
            {
                var input = AnsiConsole.Ask<string>("Please enter a date (format: yyyy-MM-dd): ");
                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                date = ParseDate(input);
            }
            catch (Exception)
            {
                PrintInvalidDateFormat();
            }
        }

        DateTime? hoursAndMinutes = null;
        while (!hoursAndMinutes.HasValue)
        {
            try
            {
                var input = AnsiConsole.Ask<string>("Please enter the time (format: HH:mm): ");
                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                hoursAndMinutes = ParseTime(input);
                if (hoursAndMinutes.HasValue)
                {
                    date = date.Value.AddHours(hoursAndMinutes.Value.Hour).AddMinutes(hoursAndMinutes.Value.Minute);
                }
            }
            catch (Exception)
            {
                PrintInvalidDateFormat();
            }
        }

        return date.Value;
    }

    /// <summary>
    /// Parses the input string to a DateTime value.
    /// </summary>
    /// <param name="input">The input string to parse.</param></param>
    /// <returns>The parsed DateTime value.</returns>
    /// <exception cref="FormatException"></exception>
    public static DateTime ParseDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length != 10 || input[4] != '-' || input[7] != '-')
        {
            throw new FormatException("Invalid date format.");
        }

        return DateTime.Parse(input);
    }

    /// <summary>
    /// Parses the input string to a DateTime value representing a time.
    /// </summary>
    /// <param name="input">The input string to parse.</param></param>
    /// <returns>The parsed DateTime value representing a time.</returns>
    /// <exception cref="FormatException"></exception>
    public static DateTime ParseTime(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length != 5 || input[2] != ':')
        {
            throw new FormatException("Invalid time format.");
        }

        var time = DateTime.Parse(input);
        if (time.Hour > 23 || time.Minute > 59)
        {
            throw new FormatException("Invalid time value.");
        }

        return time;
    }

    /// <summary>
    /// Prints an error message for an invalid date format.
    /// </summary>
    private static void PrintInvalidDateFormat()
    {
        AnsiConsole.MarkupLine("[bold red]Invalid date format. Please try again.[/]");
    }

    /// <summary>
    /// Starts the stopwatch and logs the coding time when the user stops it.
    /// </summary>
    /// <param name="codingTrackerDatabase">The coding tracker database.</param>
    public static void StartStopwatch(CodingTrackerDatabase codingTrackerDatabase)
    {
        var startTime = GetCurrentDateTimeNoMilliseconds();
        AnsiConsole.MarkupLine("[bold green]Coding session started.[/]");
        AnsiConsole.MarkupLine("Press any key to stop the stopwatch and log the coding time.");
        while (!Console.KeyAvailable)
        {
            AnsiConsole.Markup($"\r[bold]Elapsed Time:[/] {DateTime.Now - startTime:hh\\:mm\\:ss}");
            Task.Delay(100);
        }

        Console.ReadKey(true); // Clear the key from the buffer
        AnsiConsole.WriteLine();
        var endTime = GetCurrentDateTimeNoMilliseconds();
        codingTrackerDatabase.LogCodingTime(new CodingSession { StartTime = startTime, EndTime = endTime });
        AnsiConsole.MarkupLine("[bold green]Coding time logged successfully.[/]");
        AnsiConsole.MarkupLine($"[bold]Duration:[/] {endTime - startTime:hh\\:mm\\:ss}");
    }

    /// <summary>
    /// Prints the coding time logs from the coding tracker database.
    /// </summary>
    /// <param name="codingTrackerDatabase">The coding tracker database.</param>
    public static void PrintLogs(CodingTrackerDatabase codingTrackerDatabase)
    {
        AnsiConsole.MarkupLine("[bold]Coding Time Logs[/]");
        var logs = codingTrackerDatabase.GetCodingTimeLogs();
        if (!logs.Any())
        {
            AnsiConsole.MarkupLine("[bold red]No logs found.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (var codingSession in logs)
        {
            var startTime = codingSession.StartTime;
            var endTime = codingSession.EndTime;
            TimeSpan duration = endTime - startTime;
            table.AddRow(startTime.ToString("yyyy-MM-dd HH:mm"), endTime.ToString("yyyy-MM-dd HH:mm"), duration.ToString());
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Gets the current DateTime value without milliseconds.
    /// </summary>
    /// <returns>The current DateTime value without milliseconds.</returns>
    private static DateTime GetCurrentDateTimeNoMilliseconds()
    {
        var currentTime = DateTime.Now;
        return new DateTime(currentTime.Ticks - (currentTime.Ticks % TimeSpan.TicksPerSecond), currentTime.Kind);
    }
}
