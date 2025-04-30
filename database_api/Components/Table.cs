using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public class Table : IComponent
    {       
        public RowController RowController { get; set; }            
        
        public Table(string _title, bool _login)
        {               
            RowController = new RowController(_title, _login);
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            RowController.AddChar(info);
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 0);            
            foreach (Row item in RowController.GetRows())
            {
                if (item == null) break;
                if(item.SystemValue)
                {
                    Console.ForegroundColor = item.Color;                    
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;

                }
                Console.WriteLine(item.Value);  
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(RowController.GetLastRow());
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
