using System;

namespace DigitalWalletAPI.Domain.Exceptions;

public class WalletException : DomainException
{
    public Guid WalletId { get; }

    public WalletException(string message) : base(message, 400)
    {
        WalletId = Guid.Empty;
    }

    public WalletException(Guid walletId, string message) : base(message, 400)
    {
        WalletId = walletId;
    }
}

public class WalletNotFoundException : DomainException
{
    public Guid WalletId { get; }

    public WalletNotFoundException() 
        : base("Carteira não encontrada", 404)
    {
        WalletId = Guid.Empty;
    }

    public WalletNotFoundException(Guid walletId) 
        : base($"Carteira com ID {walletId} não encontrada", 404)
    {
        WalletId = walletId;
    }
}

public class InvalidWalletOperationException : DomainException
{
    public InvalidWalletOperationException(string message) 
        : base(message, 400)
    {
    }
}

public class InsufficientBalanceException : DomainException
{
    public Guid WalletId { get; }

    public InsufficientBalanceException(Guid walletId, string message) 
        : base(message, 422)
    {
        WalletId = walletId;
    }
}

public class TransactionNotFoundException : DomainException
{
    public Guid TransactionId { get; }

    public TransactionNotFoundException(Guid transactionId, string message) 
        : base(message, 404)
    {
        TransactionId = transactionId;
    }
} 