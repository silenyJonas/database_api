using database_api.Documents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public class CommandProccesor
    {
        private CommandsList cmList { get; } = new CommandsList();
        public CommandProccesor() => cmList.FillList();
        public Package Execute(string data, string title)
        {
            Package p = ValidatePattern(data.Remove(0, title.Length));
            return new Package(title + p.Text, true, p.Color, p.Data); //tohle true je taky zprava o tom ze je to systemovy prikaz           
        }
        private Package ValidatePattern(string data)
        {            
            foreach (ICommands item in cmList.commands)
            {
                    Package result = item.Proceed(data);
                    if (result.systemValue) //pokud je result.item1 true neboli pokud se najde shoda tak se vrati stav + to ze se to naslo
                    {
                        return new Package(result.Text, result.systemValue, result.Color, result.Data);
                    }               
            }
            return new Package(cmList.IncorrectSyntax, false, ConsoleColor.Red); //nebo se vrati false coz zamena ze nebyla zadna schoda        
        }
    }
}
