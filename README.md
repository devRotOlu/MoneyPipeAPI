# 💸 MoneyPipe — FinTech Platform (Built with ASP.NET Core Clean Architecture)

MoneyPipe is a modern FinTech platform that empowers users and businesses to manage finances effortlessly.  
It enables **invoice generation**, **virtual account management**, **multi-currency wallets**, and **secure payments** —  
all built using **ASP.NET Core 8** and following the principles of **Clean Architecture** for scalability, maintainability, and testability.

---

## 🚀 Project Overview

MoneyPipe provides users with a secure platform to:

- Create and manage invoices
- Generate and fund virtual accounts
- Manage wallets in multiple currencies
- Create and manage virtual cards
- Send and receive payments
- Track transactions and balances
- Perform currency conversion
- View account statements and analytics

The system integrates with **third-party FinTech APIs** (such as **Monnify**, **Paystack**, or **Flutterwave**)  
to provide real-world financial operations in a seamless and secure environment.

---

## 🧠 Core Features

### 💳 Payments & Virtual Accounts

- Generate unique virtual bank accounts for each user
- Send and receive payments in multiple currencies
- Automatically track payment statuses and balances

### 📄 Invoices

- Create professional invoices with payment links
- Track invoice statuses (Pending, Paid, Overdue)
- Allow clients to pay invoices directly through integrated gateways

### 🏦 Wallets & Currency Exchange

- Multi-currency wallets with real-time balance updates
- Exchange between currencies using live FX rates
- Transaction history and downloadable statements

### 🔒 Authentication & Security

- JWT-based authentication and refresh tokens
- Role-based access control (Admin, User, Business)
- Two-Factor Authentication (2FA) for critical actions
- AES-encrypted sensitive data

### 📊 Dashboard & Insights

- User and business dashboards
- Transaction summaries and analytics
- Notifications for transactions, invoices, and card usage

---

## 🧱 Architecture

MoneyPipe follows **Clean Architecture**, ensuring separation of concerns and scalability.

### 🧩 Layers Overview

| Layer              | Description                                                                 |
| ------------------ | --------------------------------------------------------------------------- |
| **Core**           | Domain models and business rules. Pure logic without dependencies.          |
| **Application**    | Use cases, DTOs, and service contracts. Depends only on Core.               |
| **Infrastructure** | Data access (EF Core, Dapper), external API integrations, and repositories. |
| **API**            | Controllers, request validation, authentication, and route definitions.     |

---

## ⚙️ Tech Stack

| Category                | Technology                                        |
| ----------------------- | ------------------------------------------------- |
| **Backend**             | ASP.NET Core 8 Web API                            |
| **Architecture**        | Clean Architecture + Repository + Unit of Work    |
| **Database**            | PostgreSQL                                        |
| **ORM**                 | Dapper, DbUp                                      |
| **Security**            | JWT, AES Encryption, 2FA, BCrypt Password Hashing |
| **API Integrations**    | Paystack / Monnify / Flutterwave                  |
| **Cloud Storage**       | Supabase / Cloudinary (for media/invoices)        |
| **Logging**             | Serilog                                           |
| **Testing**             | xUnit / Moq                                       |
| **Frontend (Optional)** | React.js / Next.js (future plan)                  |

---

## 🧩 API Endpoints (Sample)

| Module       | Endpoint               | Method | Description               |
| ------------ | ---------------------- | ------ | ------------------------- |
| Auth         | `/api/auth/register`   | `POST` | Register a new user       |
| Auth         | `/api/auth/login`      | `POST` | Login and get JWT         |
| Invoices     | `/api/invoices`        | `POST` | Create an invoice         |
| Payments     | `/api/payments/verify` | `POST` | Verify payment            |
| Wallets      | `/api/wallets`         | `GET`  | Get wallet balances       |
| Transactions | `/api/transactions`    | `GET`  | Fetch transaction history |

---

## 🛠️ Setup Instructions

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/<your-username>/MoneyPipe.git
cd MoneyPipe
```

---

## 👨‍💻 Author

**Olumide Rotimi**  
Backend Developer | .NET & FinTech Enthusiast  
📧 [rotimiolumide68@gmail.com](mailto:rotimiolumide68@gmail.com)  
🔗 [LinkedIn Profile](https://www.linkedin.com/in/olumide-rotimi/)
