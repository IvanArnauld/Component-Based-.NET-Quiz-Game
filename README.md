# Component-Based-.NET-Quiz-Game

## Overview

This project is a simple quiz game developed in C# using Windows Communication Foundation (WCF). The game allows players to join, answer questions, and displays the winner at the end.

## Components

### QuizGameLibrary (Class Library)

- **Player.cs:** Represents a player with a name and points.
- **Question.cs:** Represents a quiz question with options.
- **IGameQuestions.cs:** WCF service contract defining game-related operations.
- **ICallBack.cs:** WCF callback contract for sending updates to clients.
- **QuestionLibrary.cs:** Contains a list of hardcoded questions.

### QuizGameServiceHost (Console Application)

- **Program.cs:** Hosts the WCF service and initializes the game.

### QuizGameClient (Console Application)

- **Program.cs:** Allows players to join, answer questions, and displays the winner.

## Technologies

- **C#:** Main programming language.
- **WCF (Windows Communication Foundation):** Used for communication between the host, service, and client.
- **Visual Studio 2022:** Development environment.

## Communication Flow

1. **Host (QuizGameServiceHost):**
   - Hosts the WCF service.
   - Manages the game.
   - Initializes the game questions from the hardcoded question library.
   - Listens for players to join and manages their interactions.

2. **Service (QuizGameLibrary):**
   - Provides the IGameQuestions WCF service.
   - Generates and serves questions.
   - Manages the game state and player information.
   - Communicates with clients using callbacks (ICallBack).

3. **Client (QuizGameClient):**
   - Allows players to join the game.
   - Displays questions to players.
   - Collects and sends answers to the service.
   - Receives updates about other players and the game state.

## How to Run

1. **Visual Studio Setup:**
   - Open the solution in Visual Studio 2022.
   - Set `QuizGameServiceHost` as the startup project.

2. **Configure Service Endpoint:**
   - Open `App.config` in the `QuizGameServiceHost` project.
   - Ensure the service endpoint is correctly configured (net.tcp://localhost:13000/QuizGameLibrary/GameQuestionsService).

3. **Run QuizGameServiceHost:**
   - Press F5 to start hosting the service.

4. **Run QuizGameClient:**
   - Open a new instance of Visual Studio.
   - Set `QuizGameClient` as the startup project.
   - Press F5 to start the client.

5. **Game Interaction:**
   - Players can join the game by entering their names in the client.
   - Questions will be presented, and players can choose their answers.
   - The game host manages the flow and displays the winner at the end.

## Additional Notes

- Ensure that the WCF service is running before starting the client.
- Adjust firewall settings to allow communication on the specified port.

Feel free to reach out for any questions or assistance!

