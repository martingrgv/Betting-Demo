using static BettingGame.Constants.MessageConstants;

namespace BettingGame.Common;

public class GameEngine(CommandFactory commandFactory, IWalletService walletService) : IGameEngine
{
    private bool _isRunning = true;

    public async Task RunAsync()
    {
        Log.Information("Engine started.");

        var playerId = GetPlayerId();
        var wallet = await GetOrCreateWallet(playerId);
        
        while (_isRunning)
        {
            ICommand command = null;
            
            Console.WriteLine(EnterInputMessage);
            string[]? input = Console.ReadLine()?
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (input.Length == 0)
            {
                Console.WriteLine(NoCommandMessage);
                Console.WriteLine();
                continue;
            }
            
            string choice = input[0];
            if (choice == "exit")
            {
                Console.WriteLine(ExitMessage);
                Log.Information("{PlayerId} exited game.", playerId);
                _isRunning = false;
                continue;
            }

            if (input.Length == 2)
            {
                switch (choice)
                {
                    case "bet":
                        command = commandFactory.Create<BetCommand>(wallet);
                        break;
                    case "deposit":
                        command = commandFactory.Create<DepositCommand>(wallet);
                        break;
                    case "withdraw":
                        command = commandFactory.Create<WithdrawCommand>(wallet);
                        break;
                    default:
                        Console.WriteLine(InvalidCommandUsageMessage);
                        break;
                }
            }
            else
            {
                Console.WriteLine(InvalidCommandUsageMessage);
                Console.WriteLine();
            }

            if (command is null)
            {
                continue;
            }
            
            var result = await command.Execute(input);

            if (!result.IsSuccess)
            {
                Log.Fatal(result.Exception, $"{command.GetType().Name} resulted in error!");
            }

            Console.WriteLine(result.Message);
            Console.WriteLine();
        }
        
        Log.Information("Engine killed.");
    }

    private async Task<Wallet> GetOrCreateWallet(Guid playerId)
    {
        var wallet = await walletService.GetByPlayerIdAsync(playerId)
                     ?? await walletService.CreateWalletAsync(playerId);

        return wallet;
    }

    private static Guid GetPlayerId()
    {
        // This concept can be changed to fetch the player id from an API
        return Guid.NewGuid();
    }
}