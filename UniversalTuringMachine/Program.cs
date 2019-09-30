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
            var d = new DelayFunction();
            var e = new ExitFunction();


            DoWelcomeScreen();
        }
        static bool correctSelection = false;
        public static void DoWelcomeScreen()
        {
            correctSelection = false;
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("TURING MACHINE SIMULATOR");
            Console.WriteLine("(made by Mindaugas Morkunas)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            LoadPrograms();

            while (!correctSelection)
            {
                var input = Console.ReadKey();
                char res = '0';
                try
                {
                    string inp = input.Key.ToString();
                    if (inp.StartsWith("NumPad") || inp.StartsWith("D"))
                    {
                        res = inp.Last();
                    }
                    var selectedFunction = ProgramSettings.GetProgramFunction(int.Parse(res.ToString()) - 1);
                    
                    selectedFunction.RunProgram();
                    correctSelection = true;
                    Console.Clear();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - Command not found. Press try again...");
                    Console.ForegroundColor = ConsoleColor.White;
                    //Console.ReadKey();
                    //DoWelcomeScreen();
                }
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
