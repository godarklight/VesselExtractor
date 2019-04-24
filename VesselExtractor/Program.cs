using System;
using System.IO;
using ConfigNodeParser;

namespace VesselExtractor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (!File.Exists("persistent.sfs"))
            {
                Console.WriteLine("Place persistent.sfs next to this program.");
                return;
            }
            Console.WriteLine("Extracting vessels!");
            ConfigNode[] vessels = ConfigNodeReader.FileToConfigNode("persistent.sfs").GetNode("GAME").GetNode("FLIGHTSTATE").GetNodes("VESSEL");
            if (Directory.Exists("Vessels"))
            {
                Directory.Delete("Vessels", true);
            }
            Directory.CreateDirectory("Vessels");
            foreach (ConfigNode vesselnode in vessels)
            {
                string vesselID = ConvertConfigStringToGUIDString(vesselnode.GetValue("pid"));
                string vesselName = vesselnode.GetValue("name");
                Console.WriteLine("Saving vessel: " + vesselName);
                string fileText = ConfigNodeWriter.ConfigNodeToString(vesselnode);
                File.WriteAllText(Path.Combine("Vessels", vesselID + ".txt"), fileText);
            }
        }

        public static string ConvertConfigStringToGUIDString(string configNodeString)
        {
            if (configNodeString == null || configNodeString.Length != 32)
            {
                return null;
            }
            string[] returnString = new string[5];
            returnString[0] = configNodeString.Substring(0, 8);
            returnString[1] = configNodeString.Substring(8, 4);
            returnString[2] = configNodeString.Substring(12, 4);
            returnString[3] = configNodeString.Substring(16, 4);
            returnString[4] = configNodeString.Substring(20);
            return String.Join("-", returnString);
        }
    }
}
