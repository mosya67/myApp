using Microsoft.EntityFrameworkCore;

namespace myApp;
public partial class DatabaseServices
{
    public static int GetAge(DateTime birthDate)
    {
        int age = DateTime.Today.Year - birthDate.Year;
        if (birthDate > DateTime.Today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// вывод всех записей с уникальным значением ФИО + дата рождения с сортировкой по ФИО
    /// </summary>
    public void Output()
    {
        using (MyDbContext db = new())
        {
            var persons = db.persons
                .AsNoTracking()
                .GroupBy(p => new { p.name, p.dateofbirth })
                .Select(g => g.First()).ToArray();
    
            Array.Sort(persons.Select(p => p.name).ToArray(), persons);
    
            foreach (var person in persons)
            {
                WriteLine($"{person.name}, {person.dateofbirth}, {person.gender}, {GetAge(person.dateofbirth.ToDateTime(new TimeOnly(0, 0)))}");
            }
        }
    }

    /// <summary>
    /// выводит все элементы передаваемой коллекции типа Person
    /// </summary>
    /// <param name="persons">коллекция для вывода</param>
    public void Output(IEnumerable<Person> persons)
    {
        foreach (var p in persons)
        {
            WriteLine($"{p.name}, {p.dateofbirth}, {p.gender}, {GetAge(p.dateofbirth.ToDateTime(new TimeOnly(0, 0)))}");
        }
    }

    /// <summary>
    /// создает заданное количество рандомных записей в базе данных, но не сохраняет их
    /// </summary>
    /// <param name="recordsNumber">количество записей</param>
    /// <param name="db">экземпляр вашего контекста базы данных</param>
    public void GenerateRandomRecords(int recordsNumber, MyDbContext db)
    {
        for (int i = 0; i < recordsNumber; i++)
        {
            AddRecord($"{RandomName()} {RandomName()} {RandomName()}", RandomDate(), (Gender)new Random().Next(0, 2), db);
        }
    }

    /// <summary>
    /// создает заданное количество рандомных записей с указанием начального символа ФИО и пола(они не будут рандомно сгенерированны), но в самой базе данных их не сохраняет
    /// </summary>
    /// <param name="recordsNumber">количество записей</param>
    /// <param name="initialLetter">символ, с которого ,будет начинаться ФИО</param>
    /// <param name="gender">пол</param>
    /// <param name="db">экземпляр вашего контекста базы данных</param>
    public void GenerateRandomRecords(int recordsNumber, char initialLetter, Gender gender, MyDbContext db)
    {
        for (int i = 0; i < recordsNumber; i++)
        {
            AddRecord($"{RandomName(initialLetter)} {RandomName(initialLetter)} {RandomName(initialLetter)}", RandomDate(), gender, db);
        }
    }

    /// <summary>
    /// делает выборку из таблицы по мужскому полу и начальныз букв ФИО
    /// </summary>
    /// <param name="symbol">начальная буква ФИО</param>
    /// <param name="db">экземпляр вашего контекста базы данных</param>
    /// <returns>возвращает новую коллекцию, которая содержит элементы, у которых мужской пол и ФИО начинается с заданной буквы</returns>
    public IEnumerable<Person> Selection(char symbol, MyDbContext db)
    {
        var persons = db.persons.AsNoTracking();

        var filtredPersons = persons.Where(p => p.gender == Convert.ToString(Gender.Male)).ToArray();

        for (int i = 0; i < filtredPersons.Length; i++)
        {
            var names = filtredPersons[i].name.Split(' ');

            if (names[0][0] == symbol && names[1][0] == symbol && names[2][0] == symbol)
            {
                yield return filtredPersons[i];
            }
        }
    }

    /// <summary>
    /// предназначен для сравнения времени выполнения
    /// </summary>
    public IEnumerable<Person> Selection2(char symbol, MyDbContext db)
    {
        var persons = db.persons;

        var filtredPersons = persons.Where(p => p.gender == Convert.ToString(Gender.Male)).ToArray();

        for (int i = 0; i < filtredPersons.Length; i++)
        {
            var names = filtredPersons[i].name.Split(' ');

            if (names[0][0] == symbol && names[1][0] == symbol && names[2][0] == symbol)
            {
                yield return filtredPersons[i];
            }
        }
    }
}