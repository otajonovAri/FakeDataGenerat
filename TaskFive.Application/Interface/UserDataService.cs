using Bogus;
using TaskFive.Application.Helpers;
using TaskFive.Core.Entities;
using TaskFive.Core.Enums;

namespace TaskFive.Application.Interface;

public class UserDataService :  IUserDataService
{
    public IEnumerable<User> GetUserData(int seed, double mistakeRate, string locale)
    {
        return GenerateFakeUserData(seed, mistakeRate, locale).GenerateForever();
    }

    private Faker<User> GenerateFakeUserData(int seed, double mistakeRate, string locale)
    {
        var randomizer = new Randomizer(seed);

        var fakeAddresses = new Faker<Address>(locale).UseSeed(seed)
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.SecondAddress, f => f.Address.SecondaryAddress()); 

        var fakeUserData = new Faker<User>(locale).UseSeed(seed)
            .RuleFor(u => u.Id, f => f.Finance.Account())
            .RuleFor(u => u.Gender, f => f.Random.Enum<Gender>())
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.AddressString, f => fakeAddresses.Generate().GetRandomAddressString(randomizer))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumberFormat())
            .FinishWith((f, p) => p.MakeMistakes(mistakeRate, randomizer, locale));

        return fakeUserData;
    }
}
