using Npgsql;
using database_api.Windows;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace database_api
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Application.Window = new LoginWindow();
            Console.CursorVisible = false;

            while (true)
            {
                Application.Draw();
                ConsoleKeyInfo info = Console.ReadKey();
                Application.HandleKey(info);
            }

        }
    }
}
