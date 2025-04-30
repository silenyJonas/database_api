using database_api.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace database_api
{
    public class Application
    {
        public static Window Window { get; set; }       

        public static void HandleKey(ConsoleKeyInfo info)
        {
            Window.HandleKey(info);
        }
        public static void Draw()
        {
            Window.Draw();
        }
    }
}
