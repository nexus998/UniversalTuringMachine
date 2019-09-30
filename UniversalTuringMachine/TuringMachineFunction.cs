using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Reflection;

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
        private TuringMachine selectedMachine;
        public static bool simulationsRunning = false;

        public static void Reset()
        {
            fileNames.Clear();
            machines.Clear();
        }

        void RunProgram()
        {
            Reset();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Files selected: " + fileNames.Count);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enter the command \"RUN\" to begin the simulations.");
            Console.WriteLine("Enter the command \"BACK\" to return to the main menu.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.WriteLine("Enter file name of code and press enter:");
            while (!simulationsRunning)
            {
                string name = Console.ReadLine();
                if (name.EndsWith(".txt"))
                {
                    if (File.Exists(name))
                    {
                        fileNames.Add(Directory.GetCurrentDirectory() + "\\" + name);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(name + " added.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(name + " does not exist.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (name.ToLower() == "run")
                {
                    if (fileNames.Count > 0)
                    {
                        FileLoader loader = new FileLoader();
                        for (int i = 0; i < fileNames.Count; i++)
                        {
                            TuringMachine m = new TuringMachine(loader.LoadTuringInstruction(fileNames[i]), fileNames[i].Substring(fileNames[i].LastIndexOf('\\')+1));
                            machines.Add(m);
                        }
                        var t = new Thread(StartSimulations);
                        t.Start();
                        //ThreadPool.QueueUserWorkItem((obj) => StartSimulations());
                        //StartSimulations();
                        simulationsRunning = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No files have been added.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if(name.ToLower() == "back")
                {
                    Console.Clear();
                    Reset();
                    Program.DoWelcomeScreen();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entered text should end with .txt to indicate a file, or RUN to indicate the start of the simulations.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        void StartThread(TuringMachine machine)
        {
            ThreadPool.QueueUserWorkItem((obj) => machine.StartSimulation());
        }
        void ReadInputWhileRunning()
        {
            while (simulationsRunning)
            {
                var key = Console.ReadKey();
                EnterCommand(key.KeyChar.ToString());
                //Console.Clear();
            }

        }

        void EnterCommand(string command)
        {

            int selection;
            if(int.TryParse(command, out selection))
            {
                if (selection == 0) PauseAllMachines();
                else
                    SelectMachine(selection);
            }
            else
            {
                if(selectedMachine != null)
                {
                    if(command == "p")
                    {
                        selectedMachine.TogglePause();
                    }
                }

                if(command == "q")
                {
                    GoBackToMainMenu();
                }
            }
            Console.Clear();
        }

        void PauseAllMachines()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < machines.Count; i++)
            {
                machines[i].SetPause(true);
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }

        void SelectMachine(int machineIndex)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            selectedMachine = machines[machineIndex - 1];
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
        void GoBackToMainMenu()
        {
            var fileName = Assembly.GetExecutingAssembly().Location;
            System.Diagnostics.Process.Start(fileName);
            Environment.Exit(0);
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
            Console.Clear();
            simulationsRunning = true;
            for(int i = 0; i < machines.Count; i++)
            {
                StartThread(machines[i]);
            }
            var t = new Thread(() => ReadInputWhileRunning());
            t.Start();

            while (simulationsRunning)
            {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                for (int i = 0; i < machines.Count; i++)
                {
                    Console.Write("╔");
                    Console.Write(machines[i].GetName());

                    for (int j = 1; j < machines[i].GetElementCharArray().Length - machines[i].GetName().Length + 1; j++)
                    {
                        Console.Write("═");
                    }
                    Console.Write("╗");
                    Console.Write("\n");
                    Console.Write("║");
                    if (!machines[i].GetCompleted())
                    {
                        for (int j = 0; j < machines[i].GetElementCharArray().Length; j++)
                        {

                            if (j == machines[i].header.GetPosition())
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(machines[i].GetSymbolAtPosition(j));
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else Console.Write(machines[i].GetSymbolAtPosition(j));
                        }
                    }
                    Console.Write("║");
                    Console.Write("\n");
                    Console.Write("║");
                    for (int j = 0; j < machines[i].GetElementCharArray().Length; j++)
                    {
                        if (j == machines[i].header.GetPosition()) Console.Write("^");
                        else Console.Write("-");
                    }
                    Console.Write("║");
                    //Console.WriteLine("\nHeader position: " + machines[i].header.GetPosition());
                    /*Console.Write("\n║Paused: " + machines[i].GetPaused());
                    for (int j = 0; j < machines[i].GetElementCharArray().Length - (machines[i].GetPaused() ? 12 : 13); j++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("║");*/
                    Console.WriteLine();
                    Console.Write("╚");
                    for (int j = 0; j < machines[i].GetElementCharArray().Length; j++)
                    {
                        Console.Write("═");
                    }
                    Console.Write("╝");
                    Console.Write("\n");

                    Console.Write("\n");
                }
                
            }

        }
    }
}
