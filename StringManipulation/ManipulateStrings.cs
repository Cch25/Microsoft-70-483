using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;

namespace StringManipulation
{
    public class ManipulateStrings
    {
        readonly Strings strings = new Strings();
        public void StringInterning() => strings.StringInterning();
        public void StringWriter() => strings.StringWriterExample();
        public void StringReader() => strings.StringReaderExample();
        public void SearchStrings() => strings.SearchStringsExample();
        public void StringComparisonAndCulture() => strings.StringComparisonAndCulture();
        public void FormatString() => strings.FormatStringsExample();
        public void FormattableString() => strings.FormattableString();
        public void MusicTrackFormatter()
        {
            MusicTrack musicTrack = new MusicTrack("Billy Raffoul", "Acoustic");
            Console.WriteLine($"Track  {musicTrack:G}");
            Console.WriteLine($"Artist {musicTrack:A}");
            Console.WriteLine($"Title  {musicTrack:T}");
        }
    }

    public class Strings
    {
        public void StringInterning()
        {
            string s1 = "Hi there!";
            string s2 = $"{string.Intern(s1)} How are you?";

            Console.WriteLine(s2);

            // See if a string literal is interned.
            string value1 = "cat";
            string value2 = string.IsInterned(value1);
            Console.WriteLine(value2);

            // See if a dynamically constructed string is interned.
            string value3 = "cat" + 1.ToString();
            string value4 = string.IsInterned(value3);
            Console.WriteLine(value4 == null);
        }
        public void StringWriterExample()
        {
            string[] arr = new string[] { "Bălării", "Târnacop", "Tălălău" };
            CultureInfo ci = new CultureInfo("fr-FR", false);
            StringWriter stringWriter = new StringWriter(ci);
            using HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
            foreach (string item in arr)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                WriteText(item, stringWriter.GetStringBuilder());
                writer.RenderEndTag();
            }
            Console.WriteLine(stringWriter.ToString());
            stringWriter.Flush();
        }
        private void WriteText(string item, StringBuilder stringBuilder)
        {
            stringBuilder.Append($"{item}");
        }
        public void StringReaderExample()
        {
            string myMultilineText = $"Hi, I'm born on the\n" +
                $"25\n" +
                $"th";
            StringReader sr = new StringReader(myMultilineText);
            string firstLine = sr.ReadLine();
            int secondLine = int.Parse(sr.ReadLine());
            string thirdLine = sr.ReadLine();
            Console.WriteLine($"{firstLine} {secondLine}{thirdLine}");

        }
        public void SearchStringsExample()
        {
            string text = " Hi there, my name is Culai";
            if (text.Contains("Hi"))
            {
                Console.WriteLine("Hi! back");
            }
            string trimSpaces = text.TrimStart(' ');
            if (trimSpaces.StartsWith("Hi"))
            {
                Console.WriteLine("Hi! again");
            }
            int firstIndexOfI = trimSpaces.IndexOf('i');
            int lastIndexOfI = trimSpaces.LastIndexOf('i');

            Console.WriteLine($"First index of i is at position {firstIndexOfI}");
            Console.WriteLine($"Last index of i is at position {lastIndexOfI}");

            string subString = trimSpaces.Substring(trimSpaces.Length - 5);
            Console.WriteLine($"Substring for the last 5 characters in string {subString}");

            string replaceText = trimSpaces.Replace("Hi", "Hello");
            Console.WriteLine($"Replaced text:\n{replaceText}");
        }
        public void StringComparisonAndCulture()
        {
            // Default comparison fails because the strings are different
            if (!"encyclopædia".Equals("encyclopaedia"))
                Console.WriteLine("Unicode encyclopaedias are not equal");
            // Set the curent culture for this thread to EN-US
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ro-RO");
            // Using the current culture the strings are equal
            if ("encyclopædia".Equals("encyclopaedia", StringComparison.CurrentCulture))
                Console.WriteLine("Culture comparison encyclopaedias are equal");
            // We can use the IgnoreCase option to perform comparisions that ignore case
            if ("encyclopædia".Equals("ENCYCLOPAEDIA", StringComparison.CurrentCultureIgnoreCase))
                Console.WriteLine("Case culture comparison encyclopaedias are equal");
            if (!"encyclopædia".Equals("ENCYCLOPAEDIA", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine("Ordinal comparison encyclopaedias are not equal");
        }
        public void FormatStringsExample()
        {
            int i = 99;
            double pi = 3.141592654;
            Console.WriteLine($" {i,-10:D}{i,-10:X}{pi,5:N2}");
        }
        public void FormattableString()
        {
            double bankAccount = 123.45;
            FormattableString fString = $"{bankAccount:C}";
            CultureInfo ciChina = CultureInfo.CreateSpecificCulture("cn-CN");
            Console.WriteLine($"Convert to Chinease yen: {fString.ToString(ciChina)}");
            CultureInfo ciRo = new CultureInfo("ro-RO");
            Console.WriteLine($"Convert to Ro lei: {fString.ToString(ciRo)}");
        }
    }


    public class MusicTrack : IFormattable
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public MusicTrack(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            switch (format)
            {
                case "A": return Artist;
                case "T": return Title;
                case "G":
                case "F": return Artist + " - " + Title;
                default:
                    throw new FormatException("Format specifier invalid.");
            }
        }
        public override string ToString()
        {
            return Artist + " - " + Title;
        }
    }
}
