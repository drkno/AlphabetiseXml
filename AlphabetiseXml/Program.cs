using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AlphabetiseXml
{
    public static class XExtensions
    {
        /// <summary>
        /// Sorts this XElement and its children using the specified ordering.
        /// </summary>
        /// <param name="element">XElement to sort.</param>
        /// <param name="sortPosition">Function to use for comparison of XElement children.</param>
        /// <returns>A sorted XElement.</returns>
        public static XElement Sort(this XElement element, Func<XElement, string> sortPosition)
        {
            if (!element.HasElements) return element;
            var elements = element.Elements();
            elements = elements.OrderBy(sortPosition);
            foreach (var child in elements)
            {
                Sort(child);
            }
            element.ReplaceNodes(elements);
            return element;
        }

        /// <summary>
        /// Sorts this XElement and its children using the default, name based alphabetical ordering.
        /// </summary>
        /// <param name="element">The XElement to be sorted.</param>
        /// <returns>A sorted XElement.</returns>
        public static XElement Sort(this XElement element)
        {
            return Sort(element, s => s.Name.ToString());
        }

        /// <summary>
        /// Sorts this XDocument and its children using the specified ordering.
        /// </summary>
        /// <param name="document">XDocument to sort.</param>
        /// <param name="sortPosition">Function to use for comparison of XDocument children.</param>
        /// <returns>A sorted XDocument.</returns>
        public static XDocument Sort(this XDocument document, Func<XElement, string> sortPosition)
        {
            return new XDocument(document.Root.Sort(sortPosition));
        }

        /// <summary>
        /// Sorts this XDocument and its children using the default, name based alphabetical ordering.
        /// </summary>
        /// <param name="document">XDocument to sort.</param>
        /// <returns>A sorted XDocument.</returns>
        public static XDocument Sort(this XDocument document)
        {
            return new XDocument(document.Root.Sort());
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string path = null;
            while (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("Input file path to XML to be sorted: ");
                path = Console.ReadLine();
            }
            var reader = new StreamReader(path);
            Console.WriteLine("Origional File:");
            Console.WriteLine(reader.ReadToEnd());
            reader.BaseStream.Position = 0;
            var element = XElement.Load(reader.BaseStream);
            element = element.Sort();
            Console.WriteLine("Alphabetized File:");
            Console.WriteLine(element.ToString());
            Console.ReadKey();
        }
    }
}
