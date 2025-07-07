## 📦 Features
- ✅ Command-based input system (e.g. `deposit 100`, `bet 10`, `withdraw 50`, `exit`)
- 🎰 Custom Slot Engine with realistic probability distribution:
  - 50% lose
  - 40% win ×2
  - 10% win ×2 to ×10 (random)
- 💰 Wallet service to manage player funds
- 🔌 DI-powered architecture via `Microsoft.Extensions.DependencyInjection`
-  🧪 Unit tests

### Description
1. Used 64 byte cryptographic algorithm to minimalize predictable of slot game and guarantee randomise
2. Added realistic Monte Carlo Statistical Test to test the probability of each win, big win, lose percentage is it in permissible range
3. Separate each command in SystemCommand interface guarantee Single Responsability (Could be made with delegates also)
6. AppSettings - json configuration, for easy manipulation of changes


## 🧠 Architecture
### 🗂️ Core Components:

| Component         | Responsibility                                 |
|-------------------|------------------------------------------------|
| `ICommand`        | Interface for executable commands              |
| `CommandRegistry` | Parses and routes input to commands            |
| `IWalletService`  | Handles deposit, withdraw, and balance logic   |
| `ISlotEngine`     | Encapsulates the slot game rules               |
| `IConsoleService` | Abstracted Console I/O for testability         |

### XUnit unit tests
### XUnit IntegrationTests
### XUnit Statistical test for statistical mistake

## 🪟 Available Commands
| Commands          | Responsibility                                 |
|-------------------|------------------------------------------------|
| deposit <amount>  | Deposit the amount to wallet balance           |
| withdraw <amount> | Withdraw the amount from wallet balance        |
| bet <amount>      | Place a bet to the slot machine                |
| exit              | End the game session                           |
