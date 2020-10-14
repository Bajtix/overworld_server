using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NativeJSMethods
{
    public Computer computerBind;

    public ConsoleLib console;

    public NativeJSMethods(Computer computer)
    {
        computerBind = computer;
        
    }
    public class JSLIB
    {
        public Computer computerBind;

        public JSLIB(Computer computerBind)
        {
            this.computerBind = computerBind;
        }
    }

    public class ConsoleLib : JSLIB
    {
        public ConsoleLib(Computer computerBind) : base(computerBind) { }
        
        public void printl(string text)
        {
            computerBind.actualConsoleLog += "\n" + text;
        }

        public void print(string text)
        {
            computerBind.actualConsoleLog += text;
        }

        public string readl()
        {
            while (!computerBind.inputBuffer.Contains("\n")) { };
            return computerBind.inputBuffer;
        }
    }
}
