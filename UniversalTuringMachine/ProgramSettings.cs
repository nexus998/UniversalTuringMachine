using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine
{
    public class ProgramSettings
    {
        static List<ProgramFunction> programFunctions = new List<ProgramFunction>();
        public static void AddProgramFunction(ProgramFunction function)
        {
            programFunctions.Add(function);
        }
        public static List<ProgramFunction> GetProgramFunctions()
        {
            return programFunctions;
        }
        public static ProgramFunction GetProgramFunction(int index)
        {
            if(index >= 0 && index < programFunctions.Count)
            {
                return programFunctions[index];
            }
            return null;
        }
        public static void RemoveProgramFunction(ProgramFunction function)
        {
            programFunctions.Remove(function);
        }
    }
}
