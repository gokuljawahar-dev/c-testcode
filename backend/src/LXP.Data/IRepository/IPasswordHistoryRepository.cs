namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IPasswordHistoryRepository
{
    Task<PasswordHistory> GetPasswordHistory(Guid learnerId);
    Task UpdatePasswordHistory(PasswordHistory passwordHistory);
}
