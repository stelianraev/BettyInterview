## 📦 Features
- ✅ Command-based input system (e.g. `deposit 100`, `bet 10`, `withdraw 50`, `exit`)
- 🎰 Custom Slot Engine with realistic probability distribution:
  - 50% lose
  - 40% win ×2
  - 10% win ×2 to ×10 (random)
- 💰 Wallet service to manage player funds
- 🧰 FluentValidation integration for robust command validation
- 🔌 DI-powered architecture via `Microsoft.Extensions.DependencyInjection`
-  🧪 Unit tests


## 🧠 Architecture
### 🗂️ Core Components:

| Component         | Responsibility                                 |
|-------------------|------------------------------------------------|
| `ICommand`        | Interface for executable commands              |
| `CommandService`  | Parses and routes input to commands            |
| `IWalletService`  | Handles deposit, withdraw, and balance logic   |
| `ISlotEngine`     | Encapsulates the slot game rules               |
| `FluentValidation`| Validates command input data                   |
| `IConsoleService` | Abstracted Console I/O for testability         |

## 🪟 Available Commands
| Commands          | Responsibility                                 |
|-------------------|------------------------------------------------|
| deposit <amount>  | Deposit the amount to wallet balance           |
| withdraw <amount> | Withdraw the amount from wallet balance        |
| bet <amount>      | Place a bet to the slot machine                |
| exit              | End the game session                           |

### 🗂️ Shema
![Schema](https://github.com/user-attachments/assets/85547423-d6a9-4dd1-af3e-705985d38761)

