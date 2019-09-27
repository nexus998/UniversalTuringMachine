using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize needed functions
            var t = new TuringMachineFunction();



            DoWelcomeScreen();
        }

        public static void DoWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Welcome to a Turing machine simulator! Please select a function by entering the number of the function and pressing enter.");
            Console.WriteLine("(made by Mindaugas Morkunas)");
            Console.WriteLine();

            LoadPrograms();
            string input = Console.ReadLine();
            ProgramFunction selectedFunction = ProgramSettings.GetProgramFunction(int.Parse(input)-1);
            if(selectedFunction != null)
            {
                Console.Clear();
                selectedFunction.RunProgram();
                
            }
            else
            {
                Console.WriteLine("Command not found. Press any key to try again...");
                Console.ReadKey();
                DoWelcomeScreen();
            }
        }
        static void LoadPrograms()
        {
            for(int i = 0; i < ProgramSettings.GetProgramFunctions().Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + ProgramSettings.GetProgramFunctions()[i].GetName());
            }
        }
    }
}
