using database_api.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Windows
{
    public class ListWindow : Window
    {
        private string _title = "[APP]";
        public ListWindow()
        {                          
            components.Add(new Table(_title, false));
        }       
    }
}
