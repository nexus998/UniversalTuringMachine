using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine
{
    public class ProgramFunction
    {
        private readonly string functionName;
        private readonly Action selectAction;
        public ProgramFunction(string functionName, Action selectAction)
        {
            this.functionName = functionName;
            this.selectAction = selectAction;
            ProgramSettings.AddProgramFunction(this);
        }
        public string GetName()
        {
            return functionName;
        }
        public void RunProgram()
        {
            selectAction.Invoke();
        }
    }
}
