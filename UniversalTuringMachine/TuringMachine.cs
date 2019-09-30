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
        string name;
        volatile char[] elements;
        List<string> instructionLines;
        string currentState = "0";
        public volatile Header header = new Header();
        bool completed = false;
        bool paused = false;
        public volatile string[] currentInstructions;
        int delay;

        public TuringMachine(List<string> instructions, string name)
        {
            paused = false;
            completed = false;
            delay = TuringMachineFunction.delay;
            this.name = name;
            List<string> inst = new List<string>(instructions);
            inst.RemoveAll(string.IsNullOrWhiteSpace);
            this.elements = inst[1].ToCharArray();
            header.SetPosition(int.Parse(inst[0])-1);
            instructionLines = instructions.Skip(2).ToList();
            for(int i = 0; i < instructionLines.Count; i++)
            {
                string trimmed = Regex.Replace(instructionLines[i], @"s", "");
                instructionLines[i] = trimmed;
            }
            instructionLines.RemoveAll(string.IsNullOrWhiteSpace);
            currentState = "0";
        }

        public void SetDelay(int value)
        {
            delay = value;
        }

        public string GetName()
        {
            return name;
        }

        //Pausing----------
        public void TogglePause()
        {
            paused = !paused;
            DoInstructions(FindInstructionsForSymbolAndState(GetSymbolAtPosition(header.GetPosition()), currentState));
        }
        public void SetPause(bool value)
        {
            paused = value;
        }
        public bool GetPaused() => paused;
        //------------------

        public char GetSymbolAtPosition(int position)
        {
            return elements[position];
        }
        public int GetHeaderPosition() { return header.GetPosition(); }

        public void StartSimulation()
        {
            DoInstructions(FindInstructionsForSymbolAndState(GetSymbolAtPosition(header.GetPosition()), currentState));
            currentInstructions = FindInstructionsForSymbolAndState(GetSymbolAtPosition(header.GetPosition()), currentState);
        }
        public void StopSimulation()
        {
            completed = true;
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
        public string[] FindInstructionsForSymbolAndState(char symbol, string state)
        {
            for(int i = 0; i < instructionLines.Count; i++)
            {
                string[] splitted = instructionLines[i].Split(' ');
                if(splitted[0] == state)
                {
                    if(splitted[1] == symbol.ToString())
                    {
                        return splitted;
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

        public void DoInstructions(string[] instructionLine)
        {
            if (!GetCompleted())
            {
                if (!GetPaused())
                {
                    elements[header.GetPosition()] = instructionLine[2].ToCharArray()[0];
                    if (instructionLine[3].ToCharArray()[0] == 'L') header.MoveHeaderLeft();
                    else header.MoveHeaderRight();

                    if (instructionLine[4].ToCharArray()[0] == 'X')
                    {
                        //Console.Clear();
                        StopSimulation();
                    }
                    else
                    {
                        
                        string[] newInstructions = FindInstructionsForSymbolAndState(GetSymbolAtPosition(header.GetPosition()), instructionLine[4]);
                        currentInstructions = newInstructions;
                        Thread.Sleep(TuringMachineFunction.delay);
                        if (newInstructions != null) DoInstructions(newInstructions);
                        else
                        {
                            StopSimulation();
                            Console.WriteLine();
                            Console.WriteLine("Cannot continue the simulation. Press any key to return to the menu...");
                            Console.ReadKey();
                            Program.DoWelcomeScreen();
                            TuringMachineFunction.Reset();
                        }
                    }
                }
            }
        }

    }
    public class Header
    {
        volatile int position;
        public Header()
        {
            position = 0;
        }
        public int GetPosition()
        {
            return position;
        }
        public void SetPosition(int value)
        {
            position = value;
        }
        public void MoveHeaderRight()
        {
            position++;
        }
        public void MoveHeaderLeft()
        {
            position--;
        }
    }
}
