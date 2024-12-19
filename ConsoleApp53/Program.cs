using System;
using System.Data.Linq;
using System.IO;
using System.Linq;

namespace CountriesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=Country;Database=Countries;Trusted_Connection=True;";
            DataContext dataContext = null;

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 - Під'єднатися до бази даних");
                Console.WriteLine("2 - Від'єднатися від бази даних");
                Console.WriteLine("3 - Вивести звіт про всі країни");
                Console.WriteLine("4 - Вивести звіт про часткову інформацію про країни");
                Console.WriteLine("5 - Вивести звіт про конкретну країну");
                Console.WriteLine("6 - Вивести звіт про міста країни");
                Console.WriteLine("0 - Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            dataContext = new DataContext(connectionString);
                            Console.WriteLine("Підключення успішно виконано!");
                            Console.WriteLine($"Інформація про базу даних: {dataContext.Connection.Database}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка підключення: {ex.Message}");
                        }
                        break;

                    case "2":
                        try
                        {
                            if (dataContext != null)
                            {
                                dataContext.Dispose();
                                Console.WriteLine("Від'єднання успішно виконано!");
                                dataContext = null;
                            }
                            else
                            {
                                Console.WriteLine("Ви не під'єднані до бази даних.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка від'єднання: {ex.Message}");
                        }
                        break;

                    case "3":
                        ShowFullCountryReport(dataContext);
                        break;

                    case "4":
                        ShowPartialCountryReport(dataContext);
                        break;

                    case "5":
                        ShowSpecificCountryReport(dataContext);
                        break;

                    case "6":
                        ShowCitiesInCountryReport(dataContext);
                        break;

                    case "0":
                        Console.WriteLine("Вихід із програми.");
                        return;

                    default:
                        Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void ShowFullCountryReport(DataContext dataContext)
        {
            var countries = from country in dataContext.GetTable<Country>()
                            select country;

            foreach (var country in countries)
            {
                Console.WriteLine($"Країна: {country.Name}, Населення: {country.Population}, Континент: {country.Continent}");
            }
        }

        static void ShowPartialCountryReport(DataContext dataContext)
        {
            Console.Write("Введіть кількість записів для відображення: ");
            int limit = int.Parse(Console.ReadLine());

            Ant_io., [19.12.2024 20:20]
var countries = (from country in dataContext.GetTable<Country>()
                 select country).Take(limit);

            foreach (var country in countries)
            {
                Console.WriteLine($"Країна: {country.Name}, Населення: {country.Population}");
            }
        }

        static void ShowSpecificCountryReport(DataContext dataContext)
        {
            Console.Write("Введіть назву країни: ");
            string countryName = Console.ReadLine();

            var country = (from c in dataContext.GetTable<Country>()
                           where c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase)
                           select c).FirstOrDefault();

            if (country != null)
            {
                Console.WriteLine($"Країна: {country.Name}, Населення: {country.Population}, Континент: {country.Continent}");
            }
            else
            {
                Console.WriteLine("Країна не знайдена.");
            }
        }

        static void ShowCitiesInCountryReport(DataContext dataContext)
        {
            Console.Write("Введіть назву країни: ");
            string countryName = Console.ReadLine();

            var cities = from city in dataContext.GetTable<City>()
                         join country in dataContext.GetTable<Country>() on city.CountryId equals country.CountryId
                         where country.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase)
                         select city;

            if (cities.Any())
            {
                Console.WriteLine($"Міста в країні {countryName}:");
                foreach (var city in cities)
                {
                    Console.WriteLine($"Місто: {city.Name}, Населення: {city.Population}");
                }
            }
            else
            {
                Console.WriteLine("Міста не знайдено.");
            }
        }
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public long Population { get; set; }
        public string Continent { get; set; }
    }

    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public long Population { get; set; }
        public int CountryId { get; set; }
    }
}