using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace UniversalTuringMachine
{
    public class TuringMachineFunction
    {
        public static int delay = 50;



        public TuringMachineFunction()
        {
            var f = new ProgramFunction("Load Turing Machine Files", RunProgram);
        }

        static List<string> fileNames = new List<string>();
        static List<TuringMachine> machines = new List<TuringMachine>();
        public static bool simulationsRunning = false;

        public static void Reset()
        {
            fileNames.Clear();
            machines.Clear();
        }

        void RunProgram()
        {
            
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Files selected: " + fileNames.Count);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enter the command \"RUN\" to begin the simulations");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.WriteLine("Enter file name of code and press enter:");
            string name = Console.ReadLine();
            if (name.EndsWith(".txt"))
            {
                if(File.Exists(name))
                {
                    fileNames.Add(Directory.GetCurrentDirectory() + "/" + name);
                    Console.WriteLine("File added. Press any key to add more...");
                    Console.ReadKey();
                    RunProgram();
                }
                else
                {
                    Console.WriteLine("Selected file does not exist. Please try again...");
                    Console.ReadKey();
                    RunProgram();
                }
            }
            else if(name.ToLower() == "run")
            {
                if (fileNames.Count > 0)
                {
                    FileLoader loader = new FileLoader();
                    for(int i = 0; i < fileNames.Count; i++)
                    {
                        TuringMachine m = new TuringMachine(loader.LoadTuringInstruction(fileNames[i]));
                        machines.Add(m);
                    }
                    var t = new Thread(StartSimulations);
                    t.Start();
                    //StartSimulations();
                    simulationsRunning = true;
                }
                else
                {
                    Console.WriteLine("No files have been added. Press any key to start adding them...");
                    Console.ReadKey();
                    RunProgram();
                }
            }
            else
            {
                Console.WriteLine("Entered text should end with .txt to indicate a file, or RUN to indicate the start of the simulations.");
                Console.ReadKey();
                RunProgram();
            }
        }
        void StartThread(TuringMachine machine)
        {
            var t = new Thread(() => machine.StartSimulation());
            t.Start();
        }
        bool AllSimulationsStopped()
        {
            for(int i = 0; i < machines.Count; i++)
            {
                if (!machines[i].GetCompleted()) return false;
            }
            return true;
        }
        void StartSimulations()
        {
            for(int i = 0; i < machines.Count; i++)
            {
                StartThread(machines[i]);
            }

            while(simulationsRunning)
            {
                Console.Clear();
                for(int i = 0; i < machines.Count; i++)
                {
                    for(int j = 0; j < machines[i].GetElementCharArray().Length; j++)
                    {
                        Console.Write(machines[i].GetSymbolAtPosition(j));
                    }
                    Console.Write("\n");
                    for (int j = 0; j < machines[i].GetElementCharArray().Length; j++)
                    {
                        if (j == machines[i].GetHeaderPosition()) Console.Write("^");
                        else Console.Write("_");
                    }
                    Console.WriteLine("\nHeader position: " + machines[i].GetHeaderPosition());
                    Console.Write("\n");
                }
                if(AllSimulationsStopped())
                {
                    simulationsRunning = false;
                    break;
                }
                Thread.Sleep(delay);
            }

        }
    }
}
