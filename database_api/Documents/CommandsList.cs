using Npgsql;
using database_api.Components;
using database_api.DatabaseController;
using database_api.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace database_api.Documents
{
    public class CommandsList : ResponseList
    {
        public string keyWord { get; set; }
        public Regex regex { get; set; }
        public List<ICommands> commands = new List<ICommands>();
        public void FillList()
        {
            commands.Add(new Logout());
            commands.Add(new Help());
            commands.Add(new DbConnect());
            commands.Add(new DbDissonnect());
            commands.Add(new Insert());
            commands.Add(new Delete());
            commands.Add(new Update());
            commands.Add(new CreateTable());
            commands.Add(new DropTable());
            commands.Add(new CreateDatabase());
            commands.Add(new DropDatabase());
            commands.Add(new CreateUser());
            commands.Add(new DropUser());
            commands.Add(new Select());
            commands.Add(new FastConn());

        }
        public class Logout : CommandsList, ICommands
        {
            public Logout()
            {
                keyWord = "logout";
                regex = new Regex("^logout$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                Application.Window = new LoginWindow();
                return new Package(Success, true, ConsoleColor.Green);
            }
        }
        public class Help : CommandsList, ICommands
        {
            public Help()
            {
                keyWord = "help";
                regex = new Regex("^help$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);


                string[] sendData = {
                    "logout - odhlásí uživatele",
                    "help - Seznam příkazů",
                    "dbcon [Host] [Port] [NázevDatabase] [UživatelJméno] [UživatelHeslo] - připojení k databázi",
                    "dbdc - odpojení od databáze",
                    "insert [Tabulka] [Sloupec]->[Hodnota]->[DatovýTyp] - insert do tabulky",
                    "delete [Tabulka] [Sloupec]->[Hodnota]->[DatovýTyp] - delete podle hodnoty ve sloupci",
                    "update [Tabulka] [IdentifikačníSloupec]->[Hodnota]->[DatovýTyp] [Sloupec]->[NováHodnota]->[DatovýTyp] - update záznamu podle identifikátoru",
                    "newtb [NázevTebulky] [NázevSloupce]->[DatovýTyp] - vytvoří novou tabulku",
                    "droptb [NázevTabulky] - Smaže tabulku",
                    "newdb [NázevDatabáze] - Vytvoří databázi",
                    "dropdb [NázevDatabáze] - Smaže databázi",
                    "crusr [NázevUživatele]->[Heslo] - Vytvoří uživatele",
                    "dropusr [NázevUživatele] - Smaže uživatele"
                };
                return new Package(Success, true, ConsoleColor.Green, sendData);
            }
        }
        public class DbConnect : CommandsList, ICommands
        {
            public DbConnect()
            {
                keyWord = "dbcon";
                regex = new Regex(@"^dbcon(\s.*){5}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                try
                {
                    AppInterface.OpenConnection(
                    $"Host={data.Split(' ')[1]};" +
                    $"Port={data.Split(' ')[2]};" +
                    $"Database={data.Split(' ')[3]};" +
                    $"User Id={data.Split(' ')[4]};" +
                    $"Password={data.Split(' ')[5]};"
                    );
                }
                catch
                {
                    return new Package(DbConError, true);
                }

                return new Package(Success, true, ConsoleColor.Green);
            }
        }
        public class DbDissonnect : CommandsList, ICommands
        {
            public DbDissonnect()
            {
                keyWord = "dbdc";
                regex = new Regex("^dbdc$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);
                try
                {
                    AppInterface.CloseConnection();
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class Insert : CommandsList, ICommands
        {
            public Insert()
            {
                keyWord = "insert";
                regex = new Regex(@"^insert\s\w{1,}(\s\w{1,}->\w{1,}->\w{1,}){1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                var data2 = new Dictionary<string, object>();

                var data3 = new List<string[]>();

                for (int i = 2; i <= pole.Length - 1; i++)
                {                   
                    data3.Add(new string[] {
                        pole[i].Split("->")[0],
                        pole[i].Split("->")[1],
                        pole[i].Split("->")[2]
                    } );
                }
                try
                {
                    AppInterface.Insert(pole[1], data3);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class Delete : CommandsList, ICommands
        {
            public Delete()
            {
                keyWord = "delete";
                regex = new Regex(@"^delete\s\w{1,}(\s\w{1,}->\w{1,}->\w{1,}){1}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                try
                {
                    AppInterface.Delete(pole[1], pole[2].Split("->")[0], pole[2].Split("->")[1], pole[2].Split("->")[2]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class Update : CommandsList, ICommands
        {
            public Update()
            {
                keyWord = "update";
                regex = new Regex(@"^update\s\w{1,}(\s\w{1,}->\w{1,}->\w{1,}){1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                var data2 = new Dictionary<string, object>();

                for (int i = 3; i < pole.Length; i++)
                {
                    data2.Add(pole[i].Split("->")[0], pole[i].Split("->")[1]);
                }

                try
                {
                    AppInterface.Update(pole[1], pole[2].Split("->")[0], pole[2].Split("->")[1], pole[2].Split("->")[2], data2);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class Select : CommandsList, ICommands
        {
            public Select()
            {
                keyWord = "select";
                regex = new Regex(@"^select\s\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);
                try
                {
                    string[] SendData = AppInterface.Select(data.Split(' ')[1]);
                    return new Package(Success, true, ConsoleColor.Green, SendData);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }

        public class CreateTable : CommandsList, ICommands
        {
            public CreateTable()
            {
                keyWord = "newtb";
                regex = new Regex(@"^newtb\s\w+(?:\s\w+->[\w()]+)*$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                var data2 = new Dictionary<string, object>();

                if (pole.Length > 2)
                {
                    for (int i = 2; i < pole.Length; i++)
                    {
                        data2.Add(pole[i].Split("->")[0], pole[i].Split("->")[1]);
                    }
                }
                try
                {
                    AppInterface.CreateTable(pole[1], data2);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }
            }
        }
        public class DropTable : CommandsList, ICommands
        {
            public DropTable()
            {
                keyWord = "droptb";
                regex = new Regex(@"^droptb\s\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                try
                {
                    AppInterface.DropTable(pole[1]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class CreateDatabase : CommandsList, ICommands
        {
            public CreateDatabase()
            {
                keyWord = "newdb";
                regex = new Regex(@"^newdb\s\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                try
                {
                    AppInterface.CreateDatabase(pole[1]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class DropDatabase : CommandsList, ICommands
        {

            public DropDatabase()
            {
                keyWord = "dropdb";
                regex = new Regex(@"^dropdb\s\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                try
                {
                    AppInterface.DropDatabase(pole[1]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class CreateUser : CommandsList, ICommands
        {
            public CreateUser()
            {
                keyWord = "crusr";
                regex = new Regex(@"^crusr\s\w{1,}->\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');

                try
                {
                    AppInterface.CreateUser(pole[1].Split("->")[0], pole[1].Split("->")[1]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class DropUser : CommandsList, ICommands
        {
            public DropUser()
            {
                keyWord = "dropusr";
                regex = new Regex(@"^dropusr\s\w{1,}$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                string[] pole = data.Split(' ');
                try
                {
                    AppInterface.DropUser(pole[1]);
                    return new Package(Success, true, ConsoleColor.Green);
                }
                catch
                {
                    return new Package(DBQuerryError, true, ConsoleColor.Red);
                }

            }
        }
        public class FastConn : CommandsList, ICommands
        {
            public FastConn()
            {
                keyWord = "fc";
                regex = new Regex(@"^fc$", RegexOptions.IgnoreCase);
            }
            Package ICommands.Proceed(string data)
            {
                if (!regex.IsMatch(data))
                    return new Package("", false, ConsoleColor.Red);

                AppInterface.OpenConnection(
                   $"Host=localhost;" +
                   $"Port=5432;" +
                   $"Database=test_database;" +
                   $"User Id=postgres;" +
                   $"Password=123456Ab;"
                   );

                return new Package(Success, true, ConsoleColor.Green);

            }
        }
    }
}
