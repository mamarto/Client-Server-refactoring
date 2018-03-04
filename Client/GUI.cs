using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class GUI
    {
        public void printUserInput(String text, List<KeyValuePair<string, string>> history, bool redrawUI)
        {
            if (redrawUI)
            {
                Console.Clear();
                Console.WriteLine("TYPE IN YOUR MESSAGE AND HIT ENTER: ");
                Console.WriteLine("====================================");
                Console.Write("> ");
                Console.WriteLine(text);

                // Print chat history
                Console.WriteLine("====================================");
                foreach (var entry in history)
                {
                    string paddedSender = (String.Format("{0} ", entry.Key)).PadRight(13, ' ');
                    Console.WriteLine(String.Format("{0}{1}", paddedSender, entry.Value));
                }
            }
        }
    }
}
