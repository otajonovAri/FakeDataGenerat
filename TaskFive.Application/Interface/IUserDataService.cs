using TaskFive.Core.Entities;

namespace TaskFive.Application.Interface;

public interface IUserDataService
{
    public IEnumerable<User> GetUserData(int seed, double mistakeRate, string locale);
}
