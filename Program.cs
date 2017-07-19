using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace MacChecker
{
    class Program
    {
        static void printBanner()
        {
            System.Console.WriteLine("{0}{1}{2}{3}",
                "\n-------------------------\n\n",
                "Author:   Steven Bremner\n",
                "Date:     June 23, 2014\n\n",
                "Thanks for using the MAC Checker tool from SteveInternals!"
            );
        }

        static void Main(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                System.Console.WriteLine("Error - MacChecker requires an input filename to be used");
                System.Console.WriteLine(" > MacChecker [MACs.txt]");

                printBanner();

                System.Environment.Exit(-1);
            }

            string inputFile = args[0]; //to be used later

            System.Console.WriteLine("Input File: {0}\n", inputFile);

            System.Console.Write("Requesting OUI List from WireShark - Please wait...");

            string[] OUI_LIST = OUIGrabber.getOUIList().Split( new Char[] {'\n'} );

            System.Console.WriteLine("OUI received!\n");

            System.Console.Write("Parsing OUI file...");

            string OUI_LIST_REGEX = @"^([0-9A-F]{2}[:-][0-9A-F]{2}[:-][0-9A-F]{2})\s+(.*?)\s(.*?)";
            string OUI_INPUT_REGEX = @"^([0-9A-F]{2}[:-][0-9A-F]{2}[:-][0-9A-F]{2})(?:.*?)";

            Regex regexpr = new Regex(OUI_LIST_REGEX);
            var OUIDictionary = new Dictionary<string, string>();

            //Building the dictionary from the data from our OUI list
            foreach (string s in OUI_LIST)
            {
                if (s.TrimStart().StartsWith("#"))
                {
                    //This is a comment and should be skipped
                    continue;
                }

                MatchCollection mc = regexpr.Matches(s);

                foreach (Match m in mc)
                {
                    //Create the OUI Dictionary we will use to lookup the values later
                    OUIDictionary.Add(m.Groups[1].Value.Replace('-',':'), m.Groups[2].Value);
                }
            }

            System.Console.WriteLine("done!\n");

            System.Console.WriteLine("Comparing to local input file...\n");

            string[] fileData = System.IO.File.ReadAllLines(inputFile);
            Regex inputregexpr = new Regex(OUI_INPUT_REGEX);

            System.Console.WriteLine("{0,-20}\t{1}", "MAC", "Description");

            foreach (string line in fileData)
            {
                if (inputregexpr.IsMatch(line))
                {
                    string inputOUI = inputregexpr.Match(line).Groups[1].Value.Replace('-',':');
                    if (OUIDictionary.ContainsKey(inputOUI))
                    {
                        System.Console.WriteLine("{0,-20}\t{1}", line, OUIDictionary[inputOUI]);
                    }
                }
            }

            printBanner();
        }
    }
}
