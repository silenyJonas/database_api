using database_api.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Windows
{
    internal class LoginWindow : Window
    {
        private string _title = "[LOGIN]";
        public LoginWindow()
        {            
            components.Add(new Table(_title, true));
        }
    }
}
