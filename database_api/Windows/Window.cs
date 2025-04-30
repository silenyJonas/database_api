using database_api.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Windows
{
    public abstract class Window
    {
        protected List<IComponent> components = new List<IComponent>();        

        public virtual void HandleKey(ConsoleKeyInfo info)
        {
            foreach (IComponent item in this.components)
            {                
                item.HandleKey(info);
            }
        }
        public virtual void Draw()
        {
            foreach (IComponent item in this.components)
            {
                item.Draw();
            }
        }
    }
}
