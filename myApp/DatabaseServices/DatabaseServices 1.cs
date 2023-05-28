using Npgsql;

namespace myApp;
/// <summary>
/// Предоставляет методы, для взаимодействия с базой данных PostgreSql
/// </summary>
public partial class DatabaseServices
{
    public readonly string connectionString;

    public DatabaseServices(string connectionString)
    {
        this.connectionString = connectionString;
    }

    /// <summary>
    /// создает таблицу "Persons"
    /// </summary>
    public void CreateTablePersons()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = @"CREATE TABLE persons (
                        PersonId SERIAL PRIMARY KEY,
                        Name Text,
                        DateOfBirth DATE NOT NULL,
                        Gender TEXT NOT NULL
                        )";

                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Добавляет в таблицу Persons запись
    /// </summary>
    public void AddRecord(string Name, DateOnly dateOfBirth, Gender gender, MyDbContext db)
    {
        db.persons.Add(new()
        {
            dateofbirth = dateOfBirth,
            gender = Convert.ToString(gender),
            name = Name
        });
    }

    /// <summary>
    /// генерирует рандомную строку, которая начинается с заглавной буквы
    /// </summary>
    /// <returns></returns>
    public string RandomName()
    {
        Random rnd = new();
        string str = $"{(char)rnd.Next('A', 'Z' + 1)}";

        for (int i = 0; i < rnd.Next(3, 10); i++)
        {
            str += $"{(char)rnd.Next('a', 'z' + 1)}";
        }
        return str;
    }

    /// <summary>
    /// генерирует рандомную строку, которая начинается с заданной буквы
    /// </summary>
    /// <returns></returns>
    public string RandomName(char initialLetter)
    {
        Random rnd = new();
        string str = $"{initialLetter}";

        for (int i = 0; i < rnd.Next(3, 10); i++)
        {
            str += $"{(char)rnd.Next('a', 'z' + 1)}";
        }
        return str;
    }

    /// <summary>
    /// генерирует рандомную дату в диапозоне 1960-2000
    /// </summary>
    public DateOnly RandomDate()
    {
        Random rnd = new();

        int year = rnd.Next(1960, 2000);
        int month = rnd.Next(1, 13);
        int days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);

        return new DateOnly(year, month, days);
    }
}
