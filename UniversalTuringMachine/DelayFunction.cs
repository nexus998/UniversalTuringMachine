using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine
{
    public class DelayFunction
    {
        public DelayFunction()
        {
            var f = new ProgramFunction("Set Delay", SetDelay);
        }

        void SetDelay()
        {
            Console.Clear();
            Console.WriteLine("Enter the amount of delay you want the Turing Machines to have (in milliseconds)");
            Console.WriteLine("Default: 50 ms");
            string input = "";
            int selection;
            while(!int.TryParse(input, out selection))
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out selection)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(input + " is not a number.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (int.TryParse(input, out selection))
            {
                TuringMachineFunction.delay = selection;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Delay set to " + selection + " ms. Press any key to go back to the menu...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Program.DoWelcomeScreen();
            }

        }
    }
}
