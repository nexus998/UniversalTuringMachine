using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UniversalTuringMachine
{
    public class FileLoader
    {
        public List<string> LoadTuringInstruction(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            List<string> lineList = new List<string>(lines);
            return lineList;
        }
    }
}
