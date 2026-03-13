namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class ProfilePasswordHistoryRepository(LXPDbContext context)
    : IProfilePasswordHistoryRepository
{
    private readonly LXPDbContext _lXPDbContext = context;

    public void AddPasswordHistory1(PasswordHistory passwordHistory)
    {
        this._lXPDbContext.PasswordHistories.Add(passwordHistory);

        this._lXPDbContext.SaveChanges();
    }
}
