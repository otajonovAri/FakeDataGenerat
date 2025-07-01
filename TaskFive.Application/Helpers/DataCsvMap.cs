using CsvHelper.Configuration;
using TaskFive.Core.Entities;

namespace TaskFive.Application.Helpers;

public sealed class DataCsvMap : ClassMap<User>
{
    public DataCsvMap()
    {
        Map(u => u.Id);
        Map(u => u.FirstName);
        Map(u => u.LastName);
        Map(u => u.Gender);
        Map(u => u.AddressString);
        Map(u => u.PhoneNumber);
    }
}
