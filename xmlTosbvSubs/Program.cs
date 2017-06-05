using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace xmlTosbvSubs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1) return;
            var path = args[0];

            using (StreamWriter sw = new StreamWriter(new FileStream(path + ".sbv", FileMode.OpenOrCreate)))
            {
               var allXml = File.ReadAllText(path, System.Text.Encoding.UTF8);
                XDocument quotesDoc = XDocument.Parse(allXml);
                var textElements = quotesDoc.Root.Elements("text");
                foreach (var item in textElements)
                {
                    var startTimeInSeconds = item.Attribute("start").Value;
                    var durationInSeconds = item.Attribute("dur").Value;

                    var startSecs = Convert.ToDouble(startTimeInSeconds);
                    var durationSecs = Convert.ToDouble(durationInSeconds);

                    var timeSpanStart = TimeSpan.FromSeconds(startSecs);
                    var timeSpanDuration = TimeSpan.FromSeconds(durationSecs);

                    sw.WriteLine("{0},{1}", timeSpanStart, timeSpanStart.Add(timeSpanDuration));
                    var text = System.Net.WebUtility.HtmlDecode(item.Value.Trim());
                    text = text.Replace('\r', ' ').Replace('\n',' ');
                    sw.WriteLine(text);
                    sw.WriteLine();
                }
            }

            Console.WriteLine("Hello World!");
        }
    }
}