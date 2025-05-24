#  🎰 Betting Demo

A simple betting game demo written in C#. This project is intended as an educational example for basic logic implementation, console interaction, and Dockerized deployment of .NET applications.

### 🧩 Features

- Basic betting mechanics via console
- Simple odds and payout calculation
- Wallet interactions for deposit and withdraw
- Docker-compatible setup for isolated running

### 🧰 Tech Stack

<table>
  <thead>
    <tr>
      <th>Layer</th>
      <th>Technology</th>
      <th>Notes</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><strong>Language</strong></td>
      <td>C#</td>
      <td>Used for application logic</td>
    </tr>
    <tr>
      <td><strong>Runtime</strong></td>
      <td>.NET 8</td>
      <td>Cross-platform CLI and runtime support</td>
    </tr>
    <tr>
      <td><strong>UI</strong></td>
      <td>Console / CLI</td>
      <td>Text-based interaction with the betting engine</td>
    </tr>
    <tr>
      <td><strong>Project Structure</strong></td>
      <td>Single-project solution</td>
      <td>Clear separation of data related logic, core business logic and presentation logic</td>
    </tr>
    <tr>
      <td><strong>Containerization</strong></td>
      <td>Docker</td>
      <td>Ensures consistent build/runtime environment</td>
    </tr>
    <tr>
      <td><strong>Orchestration</strong></td>
      <td>Docker Compose</td>
      <td>Combines services, adds profiles for development/interactivity</td>
    </tr>
    <tr>
      <td><strong>Testing Framework</strong></td>
      <td>NUnit</td>
      <td>Tests behind src/BettingGame/BettingGame.Tests</code></td>
    </tr>
     <tr>
      <td><strong>Design Patterns</strong></td>
      <td>SOLID, Strategy pattern, Command pattern, Result pattern, Dependency Injection</td>
      <td>Design patterns improves effectiveness by making the code more readable, maintainable, scalable and testable</code></td>
    </tr>
  </tbody>
</table>


### Prerequisites
.NET SDK 7.0+ (for development) <br />
Docker (for containerized run)

### Running Locally
1. If running SQL Server locally change connection string in `appsettings.json` <br />
1.1 You can also use the SQL Server container by running `docker-compose up -d`
2. Run the command in terminal or start the app from IDE:
```
cd src/Betting.Game
dotnet run
```

### Running Containerized
1. Navigate to the project folder
2. Run the commands to start Docker services:
```
docker-compose up -d
```
```
docker-compose --profile interactive run betting.game
```
