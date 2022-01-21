using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Compilador.Clases
{
    /// <summary>
    /// The class that contains a list of the possible keywords (identifiers), and has them indexed.
    /// </summary>
    class Keyword
    {
        /// <summary>
        /// The approximate total number of keywords.  It isn't necessary that this number be exact (it currently is).
        /// This helps in memory optimization during compile time.
        /// </summary>
        private const int NUMKEYWORDS = 61;

        /// <summary>
        /// A list of all of the keywords loaded from disk.
        /// </summary>
        private static List<Keyword> keywords;

        /// <summary>
        /// A hash of the keywords, allowing quick access to them after initial loading sequence.
        /// </summary>
        public static Hashtable hashIndex;

        /// <summary>
        /// The keyword (reserved word or preprogrammed method) loaded from disk.
        /// </summary>
        public string keyword;
        /// <summary>
        /// The symbol or method code that references this keyword.
        /// </summary>
        public string reference;


        /// <summary>
        /// The main Keyword constructor, used when loading keywords from disk.
        /// </summary>
        /// <param name="keyword">The keyword (reserved word or preprogrammed method) loaded from disk.</param>
        /// <param name="reference">The symbol or method code that references this keyword.</param>
        public Keyword(string keyword, string reference)
        {
            this.keyword = keyword;
            this.reference = reference;
            hashIndex.Add(keyword, reference);
        }

        /// <summary>
        /// Reads all of the keywords from disk from a CSV file.
        /// </summary>
        /// <param name="path">The path where the CSV file is located.
        /// The .csv extension is not required.</param>
        public static void ReadKeywordsCSV(string path)
        {
            hashIndex = new Hashtable(NUMKEYWORDS);
            StreamReader read = new StreamReader(path + ".csv");
            keywords = new List<Keyword>(NUMKEYWORDS);
            do
            {
                string[] line = read.ReadLine().Split(',');
                line[0] = LexicalAnalyzer.ConvertEnglishAlphabet(line[0]);
                keywords.Add(new Keyword(line[0], line[1]));
            } while (!read.EndOfStream);
            read.Close();
        }

        /// <summary>
        /// Determines if the keyword exists or not.
        /// </summary>
        /// <param name="keyword">The lexical attribute to analyze, and see whether it is a keyword or not.</param>
        /// <returns>True: Keyword exists. (Word entered is a reserved word)
        /// False: Keyword does not exist. (Word entered is not a reserved word)</returns>
        public static bool FindKeyword(string keyword)
        {
            return hashIndex.Contains(keyword);
        }

        /// <summary>
        /// Obtains all of the keywords and sorts them to display in autocompletion window.
        /// </summary>
        /// <returns>Sorted keyword string.</returns>
        public static string GetAllKeywords()
        {
            string s = "";
            Keyword[] keywords = SortKeywords();
            foreach(Keyword k in keywords)
            {
                s += k.keyword + " ";
            }
            return s.Substring(0, s.Length - 1);
        }

        /// <summary>
        /// Applies a simple sorting method on keywords.
        /// </summary>
        /// <returns>Returns an array of sorted Keywords.</returns>
        private static Keyword[] SortKeywords()
        {
            Keyword[] keywords = Keyword.keywords.ToArray();
            Keyword tempKeyword;
            for (int i = 0; i < keywords.Length; i++)
            {
                for (int j = i + 1; j < keywords.Length; j++)
                {
                    if (keywords[i].keyword.CompareTo(keywords[j].keyword) > 0)
                    {
                        tempKeyword = keywords[i];
                        keywords[i] = keywords[j];
                        keywords[j] = tempKeyword;
                    }
                }
            }
            return keywords;
        }
    }
}
