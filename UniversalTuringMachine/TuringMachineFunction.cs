using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine
{
    public class TuringMachineFunction
    {
        public TuringMachineFunction()
        {
            var f = new ProgramFunction("Load Turing Machine Files", RunProgram);
        }

        List<string> fileNames = new List<string>();

        void RunProgram()
        {
            Console.WriteLine();
            Console.WriteLine("Files selected: " + fileNames.Count);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enter the command \"RUN\" to begin the simulations");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.WriteLine("Enter file name of code and press enter:");
            string name = Console.ReadLine();

        }
    }
}
