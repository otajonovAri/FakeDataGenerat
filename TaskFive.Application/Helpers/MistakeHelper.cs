using System.Text;
using Bogus;
using TaskFive.Core.Entities;

namespace TaskFive.Application.Helpers;

public static class MistakeHelper
{
    const string Latin = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
     const string Cyrillic = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
     const string Numbers = "0123456789";

     static string _regionChars = String.Empty;

     delegate string Mistake(string text, Randomizer randomizer, string symbols); 
     static readonly Mistake[] Mistakes =
     [
         ChangeSymbol,
        AddSymbol,
        RemoveSymbol
     ];

    public static User MakeMistakes(this User data, double mistakeRate, Randomizer randomizer, string locale)
    {
        SetUpLocale(locale);

        for (int i = 0; i < (int)mistakeRate; i++)
        {
            MakeMistake(data, randomizer);
        }

        if (randomizer.Double() < mistakeRate - (int)mistakeRate)
        {
            MakeMistake(data, randomizer);
        }

        return data;
    }

    private static string ChangeSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        if (builder.Length > 0)
        {
            builder[randomizer.Number(0, text.Length - 1)] = randomizer.ArrayElement(symbols.ToArray());
        }

        return builder.ToString();
    }

    private static string AddSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        return builder.Insert(randomizer.Number(0, text.Length - 1), randomizer.ArrayElement(symbols.ToArray())).ToString();
    }

    private static string RemoveSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        if (text.Length > 0)
        {
            builder.Remove(randomizer.Number(0, text.Length - 1), 1);
            return builder.ToString();
        }

        return builder.ToString();
    }

    private static void MakeMistake(this User data, Randomizer randomizer)
    {
        var properties = data.GetType().GetProperties().Where(x => x.PropertyType == typeof(string) && x.CanWrite).ToArray();
        var currentProperty = randomizer.ArrayElement(properties);
        var symbols = _regionChars;
        if (currentProperty.Name is "PhoneNumber" or "Id")
        {
            symbols = Numbers;
        }
        var value = currentProperty.GetValue(data)?.ToString();
        if (value != null)
            currentProperty.SetValue(data, randomizer.ArrayElement(Mistakes)(value, randomizer, symbols));
    }

    static void SetUpLocale(string locale)
    {
        switch (locale)
        {
            case "Ru":
                _regionChars = Cyrillic;
                break;
            case "EnUs":
            case "De":
            case "Fr":
                _regionChars = Latin;
                break;
            default:
                _regionChars = Latin; 
                break;
        }
    }
}
