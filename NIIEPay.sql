-- ===========================================
-- DATABASE: NIIEPay
-- Author: PhongNguyen
-- Description: Cấu trúc database cho Web API ngân hàng NIIEPay
-- ===========================================

-- Tạo Database (nếu cần)
CREATE DATABASE NIIEPay;
GO

USE NIIEPay;
GO

-- ===========================================
-- 1. Bảng Accounts
-- ===========================================
CREATE TABLE Accounts (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    account_number NVARCHAR(50) UNIQUE NOT NULL,
    account_holder_name NVARCHAR(100) NOT NULL,
    phone_number NVARCHAR(15) UNIQUE NOT NULL,
    citizen_id NVARCHAR(20) NOT NULL,
    id_expiry_date DATE NOT NULL,
    available_balance DECIMAL(18,2) NOT NULL CHECK (available_balance >= 50000),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

-- ===========================================
-- 2. Bảng InterestRates (bảng tĩnh)
-- ===========================================
CREATE TABLE InterestRates (
    term_months INT PRIMARY KEY,
    interest_rate DECIMAL(5,2) NOT NULL
);
GO

-- Insert sẵn các mức lãi suất
INSERT INTO InterestRates(term_months, interest_rate) VALUES
(1, 3.5),
(2, 3.7),
(3, 3.8),
(6, 4.8),
(9, 4.9),
(12, 5.2),
(18, 5.5),
(24, 5.8),
(36, 5.8);
GO

-- ===========================================
-- 3. Bảng Transactions (Giao dịch chuyển khoản)
-- ===========================================
CREATE TABLE Transactions (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    transaction_id NVARCHAR(50) UNIQUE NOT NULL,
    transaction_type NVARCHAR(20) NOT NULL CHECK (transaction_type IN ('internal', 'external')),
    from_account BIGINT NULL,
    to_account BIGINT NULL,
    to_phone NVARCHAR(15) NULL,
    to_bank_code NVARCHAR(10) NULL,
    amount DECIMAL(18,2) NOT NULL CHECK (amount > 0),
    note NVARCHAR(255),
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (from_account) REFERENCES Accounts(id),
    FOREIGN KEY (to_account) REFERENCES Accounts(id)
);
GO

-- ===========================================
-- 4. Bảng TransactionHistory (Lịch sử biến động)
-- ===========================================
CREATE TABLE TransactionHistory (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    transaction_id BIGINT NOT NULL FOREIGN KEY REFERENCES Transactions(id),
    account_id BIGINT NOT NULL FOREIGN KEY REFERENCES Accounts(id),
    amount DECIMAL(18,2) NOT NULL,
    balance_after DECIMAL(18,2) NOT NULL,
    note NVARCHAR(255),
    transaction_time DATETIME NOT NULL DEFAULT GETDATE(),
    is_sender BIT NOT NULL
);
GO

-- ===========================================
-- 5. Bảng SavingsAccounts (Tiết kiệm có kỳ hạn)
-- ===========================================
CREATE TABLE SavingsAccounts (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    account_id BIGINT NOT NULL FOREIGN KEY REFERENCES Accounts(id),
    amount DECIMAL(18,2) NOT NULL CHECK (amount > 0),
    term_months INT NOT NULL CHECK (term_months IN (1, 2, 3, 6, 9, 12, 18, 24, 36)),
    interest_rate DECIMAL(5,2) NOT NULL,
    auto_renew BIT NOT NULL,
    start_date DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    maturity_date DATE NOT NULL,
    status NVARCHAR(20) NOT NULL DEFAULT 'open'
);
GO
