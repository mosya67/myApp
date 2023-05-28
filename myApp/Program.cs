global using static System.Console;
using System.Diagnostics;

namespace myApp;
internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
            return;

        Stopwatch stopwatch = new Stopwatch();
        DatabaseServices dbServices = new(ProjectConstants.ConnectionString);
        try
        {
            using (MyDbContext db = new())
            {
                switch (args[0])
                {
                    case "1":
                        dbServices.CreateTablePersons();
                        break;
                    case "2":
                        string Name = $"{args[1]} {args[2]} {args[3]}";
                        DateOnly birthDate = DateOnly.Parse(args[4]);
                        Gender gender;

                        if (args[5] == "M" || args[5] == "Male" || args[5] == "m" || args[5] == "male")
                        {
                            gender = Gender.Male;
                            dbServices.AddRecord(Name, birthDate, gender, db);
                        }
                        else if (args[5] == "F" || args[5] == "Female" || args[5] == "f" || args[5] == "female")
                        {
                            gender = Gender.Female;
                            dbServices.AddRecord(Name, birthDate, gender, db);
                        }
                        else
                        {
                            WriteLine("the gender is entered incorrectly");
                        }
                        break;
                    case "3":
                        dbServices.Output();
                        break;
                    case "4":
                        dbServices.GenerateRandomRecords(1000000, db);
                        dbServices.GenerateRandomRecords(100, 'F', Gender.Male, db);
                        break;
                    case "5":
                        stopwatch.Start();

                        var filterdPersons = dbServices.Selection('F', db);
                        dbServices.Output(filterdPersons);

                        stopwatch.Stop();
                        WriteLine();
                        WriteLine(stopwatch.ElapsedMilliseconds);
                        break;
                    case "6":
                        stopwatch.Start();

                        var filterdPersons2 = dbServices.Selection('F', db);
                        dbServices.Output(filterdPersons2);

                        stopwatch.Stop();

                        WriteLine($"\n{new string('-', 20)}\n");
                        var test1 = stopwatch.ElapsedMilliseconds;

                        stopwatch.Restart();

                        filterdPersons2 = dbServices.Selection2('F', db);
                        dbServices.Output(filterdPersons2);

                        stopwatch.Stop();
                        var test2 = stopwatch.ElapsedMilliseconds;
                        WriteLine($"вывод номер 1: {test1}");
                        WriteLine($"вывод номер 2: {test2}");
                        break;
                }
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
        }
    }
}