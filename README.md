# CodingTrackerConsoleApp
 https://www.thecsharpacademy.com/project/13/coding-tracker

This is a console application that allows users to track their coding sessions. The application uses a SQLite database to store and retrieve coding session data. Users can log their coding time, view their coding time logs, and exit the application.

## Features

- SQLite Database: The application creates a SQLite database if one doesn't exist and creates a table to store coding session data. This is handled by the `CodingTrackerDatabase` class.
- Menu Options: The application presents a menu to the user with options to log coding time, start a coding session, print coding time logs, and exit the application. This is implemented using the `SelectionPrompt` class from the `Spectre.Console` library.
- Error Handling: The application handles possible errors to ensure it doesn't crash and provides appropriate error messages to the user.
- Termination: The application continues to run until the user chooses the "Exit" option.
- DateTime Input: The application prompts the user to enter dates and times for logging coding time or starting a coding session, and validates the input to ensure it is in the correct format.
- Reporting: The application allows users to view their coding time logs in a table format.

## Getting Started

To run the application, follow these steps:

1. Make sure you have the necessary dependencies installed, including Microsoft.Data.Sqlite and Spectre.Console.
2. Clone the repository to your local machine.
3. Open the solution in Visual Studio.
4. Build the solution to restore NuGet packages and compile the code.
5. Run the application.

## Dependencies

- Microsoft.Data.Sqlite: The application uses this package to interact with the SQLite database.
- Spectre.Console: The application uses this package to create a user-friendly console interface.
- Dapper: The application uses this package to simplify database CRUD operations.

## Usage

1. When the application starts, it will create a SQLite database if one doesn't exist and create a table to store coding session data.
2. The application will display a menu with options to log coding time, start a coding session, print coding time logs, or exit the application.
3. Select an option by entering the corresponding number.
4. Follow the prompts to perform the desired action.
5. The application will continue to run until you choose the "Exit" option.

## License

This project is licensed under the [MIT License](LICENSE).

## Resources Used
- [The C# Academy](https://www.thecsharpacademy.com/project/13/coding-tracker)
- GitHub Copilot to generate code snippets
