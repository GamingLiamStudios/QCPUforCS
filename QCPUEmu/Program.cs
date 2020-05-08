using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace QCPUEmu {
    class Program {
        static void Main(string[] args) {
            byte[] dataMem = new byte[32];
            byte[,] str = new byte[8, 32];
            byte[] iCache = new byte[32];
            ArrayList list = new ArrayList();
            int index = 0;
            bool next = false;
            Dictionary<string, string> ops = new Dictionary<string, string>() {
                { "MSC", "00000" }, { "SST", "00001" }, { "SLD", "00010" },
                { "SLP", "00011" }, { "PST", "00100" }, { "PLD", "00101" },
                { "CND", "00110" }, { "LIM", "00111" }, { "RST", "01000" },
                { "AST", "01001" }, { "INC", "01010" }, { "RSH", "01011" },
                { "ADD", "01100" }, { "SUB", "01101" }, { "XOR", "01110" },
                { "POI", "01111" }, { "NOP", "100" }, { "JMP", "101" },
                { "MST", "110" }, { "MLD", "111" }
            };
            foreach(string b in File.ReadAllText(args[0]).Split('\n')) {
                string[] vs = b.Split(' ');
                Console.WriteLine(b);
                if(b.Contains("NOP"))
                    list.Add((byte)0);
                else if(ops.ContainsKey(vs[0])) {
                    string bits = ops[vs[0]];
                    if(bits.StartsWith("1")) {
                        if(vs[1].Contains("$"))
                            list.Add((byte)(0b01111000 | byte.Parse(vs[1].Remove(0, 1))));
                        list.Add((byte)((Convert.ToInt32(bits, 2) << 5) | byte.Parse(vs[1].Replace("$", ""))));
                    } else
                        list.Add((byte)((Convert.ToInt32(bits, 2) << 3) | byte.Parse(vs[1].Remove(0, 1))));
                } else
                    list.Add(byte.Parse(vs[0]));
            }
            byte[] asm = list.ToArray(typeof(byte)) as byte[];
            string tasm = "";
            foreach(byte b in asm)
                tasm += Convert.ToString(b, 2).PadLeft(8, '0') + "\n";
            File.WriteAllText(Path.GetFileNameWithoutExtension(args[0]) + "ASM.txt", tasm);
            /*
            while(index < asm.Length) {
                byte op = asm[index++];
                
            }
            */
        }
    }
}
