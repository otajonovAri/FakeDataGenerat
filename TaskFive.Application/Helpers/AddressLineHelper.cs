using Bogus;
using TaskFive.Core.Entities;

namespace TaskFive.Application.Helpers;

public static class AddressLineHelper
{
    public static string GetRandomAddressString(this Address address, Randomizer randomizer)
    {
        var tempAddresses = new[]
        {
            $"{address.State}, {address.City}, {address.Street}, {address.SecondAddress}",
            $"{address.City}, {address.Street}",
            $"{address.City}, {address.Street}, {address.SecondAddress}",
            $"{address.State}, {address.City}, {address.Street}"
        };

        return randomizer.ArrayElement(tempAddresses);
    }
}
