using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
namespace UniversalTuringMachine
{
    public class TuringMachine
    {
        volatile char[] elements;
        List<string> instructionLines;
        char currentState = '0';
        volatile int headerPosition;
        bool completed = false;

        public TuringMachine(List<string> instructions)
        {
            List<string> inst = new List<string>(instructions);
            this.elements = inst[1].ToCharArray();
            headerPosition = int.Parse(inst[0])-1;
            instructionLines = instructions.Skip(3).ToList();
            for(int i = 0; i < instructionLines.Count; i++)
            {
                string trimmed = Regex.Replace(instructionLines[i], @"s", "");
                instructionLines[i] = trimmed;
            }
            instructionLines.RemoveAll(string.IsNullOrWhiteSpace);
            currentState = '0';
        }

        public char GetSymbolAtPosition(int position)
        {
            return elements[position];
        }
        public int GetHeaderPosition() => headerPosition;

        public void StartSimulation()
        {
            DoInstructions(FindInstructionsForSymbolAndState(GetSymbolAtPosition(headerPosition), currentState), headerPosition);
        }
        public void StopSimulation()
        {
            completed = true;
            TuringMachineFunction.simulationsRunning = false;
        }
        public bool GetCompleted() => completed;
        public char[] GetElementCharArray()
        {
            return elements;
        }
        public void PrintElements()
        {
            for(int i = 0; i < GetElementCharArray().Length; i++)
            {
                Console.Write(elements[i]);
                
            }
            Console.WriteLine("");
        }
        public char[] FindInstructionsForSymbolAndState(char symbol, char state)
        {
            /*for(int i = 0; i < instructionLines.Count; i++)
            {
                Console.WriteLine(instructionLines[i]);
            }
            Console.ReadKey();*/
            for(int i = 0; i < instructionLines.Count; i++)
            {
                if((char)instructionLines[i][0] == (char)state)
                {
                    if(instructionLines[i].ToCharArray()[2] == symbol)
                    {
                        return instructionLines[i].ToCharArray();
                    }
                }
            }
            StopSimulation();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("No instructions found for state " + state + " and symbol " + symbol);
            Console.ReadKey();
            
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        public void DoInstructions(char[] instructionLine, int headerPosition)
        {
            for (int i = 0; i < instructionLine.Length; i++)
            {
                elements[headerPosition] = instructionLine[4];
                headerPosition = headerPosition + (instructionLine[6] == 'L' ? -1 : 1);

                if (instructionLine[8] == 'X')
                {
                    StopSimulation();
                }
                else
                {
                    char[] newInstructions = FindInstructionsForSymbolAndState(GetSymbolAtPosition(headerPosition), instructionLine[8]);
                    Thread.Sleep(TuringMachineFunction.delay);
                    if(newInstructions != null) DoInstructions(newInstructions, headerPosition);
                    else
                    {
                        StopSimulation();
                        Console.WriteLine();
                        Console.WriteLine("Can not continue the simulation. Press any key to return to the menu...");
                        Console.ReadKey();
                        Program.DoWelcomeScreen();
                        TuringMachineFunction.Reset();
                    }
                }
            }
        }

    }
}
