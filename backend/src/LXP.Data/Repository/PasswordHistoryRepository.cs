// using System.Data.Entity; // Remove this
namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore; // Use this

public class PasswordHistoryRepository(LXPDbContext context) : IPasswordHistoryRepository
{
    // Assume _context is your DbContext
    private readonly LXPDbContext _LXPDbContext = context;

    public async Task<PasswordHistory> GetPasswordHistory(Guid learnerId) =>
        await this._LXPDbContext.PasswordHistories.FirstOrDefaultAsync(x =>
            x.LearnerId == learnerId
        );

    public async Task UpdatePasswordHistory(PasswordHistory passwordHistory)
    {
        this._LXPDbContext.PasswordHistories.Update(passwordHistory);
        await this._LXPDbContext.SaveChangesAsync();
    }
}
