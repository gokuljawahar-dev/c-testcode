namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface IUpdatePasswordService
{
    Task<bool> UpdatePassword(UpdatePassword updatePassword);
}
