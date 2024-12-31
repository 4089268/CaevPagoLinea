using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CAEV.PagoLinea.Helpers;

public class ProcessCSV
{
    public static readonly string csvPattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";

    public static IEnumerable<string[]> LoadFile(Stream stream)
    {
        var response = new List<string[]>();

        // * get the lines from the file
        var lines = new List<string>();
        using(var reader = new StreamReader(stream))
        {
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if(string.IsNullOrEmpty(line)) continue;

                string[] columns = Regex.Split(line, csvPattern)
                    .Select( item => item.Trim('\"').Trim('\t'))
                    .ToArray();
                response.Add(columns);
            }
        }
        return response;
    }

}
