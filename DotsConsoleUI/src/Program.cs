using Dots.Core;

namespace Dots
{
    class Program
    {
        static void Main()
        {
            //noting too interesting here
            
            Field field = new Field(5, 5, 15);

            var ui = new ConsoleUI.ConsoleUI(field);
            
            ui.Play();
                
            
        }
    }
}