using Spectre.Console;

namespace CodingTrackerConsoleApp;

internal class Program
{
    static void Main()
    {
        var codingTrackerDatabase = new CodingTrackerDatabase();
        var menuOptions = new Dictionary<string, Action>
        {
            { "Log Coding Time", () => LogCodingTime(codingTrackerDatabase) },
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

    private static void LogCodingTime(CodingTrackerDatabase codingTrackerDatabase)
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

        codingTrackerDatabase.LogCodingTime(startTime.Value, endTime.Value);
    }

    private static DateTime? GetDateTime()
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

                if (input.Length != 10 || input[4] != '-' || input[7] != '-')
                {
                    PrintInvalidDateFormat();
                    continue;
                }

                date = DateTime.Parse(input);
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
                if (string.IsNullOrWhiteSpace(input.ToString()))
                {
                    return null;
                }

                if (input.Length != 5 || input[2] != ':')
                {
                    PrintInvalidDateFormat();
                    continue;
                }

                hoursAndMinutes = DateTime.Parse(input);
                if (hoursAndMinutes.Value.Hour > 23 || hoursAndMinutes.Value.Minute > 59)
                {
                    PrintInvalidDateFormat();
                    hoursAndMinutes = null;
                    continue;
                }

                date = date.Value.AddHours(hoursAndMinutes.Value.Hour).AddMinutes(hoursAndMinutes.Value.Minute);
            }
            catch (Exception)
            {
                PrintInvalidDateFormat();
            }
        }

        return date.Value;

        static void PrintInvalidDateFormat()
        {
            AnsiConsole.MarkupLine("[bold red]Invalid date format. Please try again.[/]");
        }
    }

    private static void PrintLogs(CodingTrackerDatabase codingTrackerDatabase)
    {
        AnsiConsole.MarkupLine("[bold]Coding Time Logs[/]");
        var logs = codingTrackerDatabase.GetCodingTimeLogs();
        if (!logs.Any())
        {
            AnsiConsole.MarkupLine("[bold red]No logs found.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach ((int id, DateTime startTime, DateTime endTime) in logs)
        {
            TimeSpan duration = endTime - startTime;
            table.AddRow(id.ToString(), startTime.ToString("yyyy-MM-dd HH:mm"), endTime.ToString("yyyy-MM-dd HH:mm"), duration.ToString());
        }

        AnsiConsole.Write(table);
    }
}
