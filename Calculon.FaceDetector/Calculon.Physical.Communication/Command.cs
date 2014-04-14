using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculon.Physical.Communication
{
    public class Command
    {
        mySerial serial;
        Dictionary<string, char> commands;
        public Command()
        {
            serial = new mySerial();
            commands = new Dictionary<string, char>();
            commands.Add("up", '5');
            commands.Add("down", '6');
            commands.Add("left", '7');
            commands.Add("right", '8');
            commands.Add("stop", '9');

        }

        public void Execute(string command)
        {
            if (String.Compare(command, "up") == 0)
            {
                serial.writeSerial("6");
            }
            else if (String.Compare(command, "down") == 0)
            {
                serial.writeSerial("5");
            }
            else if (String.Compare(command, "left") == 0)
            {
                serial.writeSerial("8");
            }
            else if (String.Compare(command, "right") == 0)
            {
                serial.writeSerial("7");
            }
            else if (String.Compare(command, "stop") == 0)
            { 
                
            }

        }
    }
}
