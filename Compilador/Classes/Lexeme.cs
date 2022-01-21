using System.Collections.Generic;
using System.IO;

namespace Compilador.Clases
{
    /// <summary>
    /// This class represents the available lexemes that the language accepts during a lexical analyisis.
    /// </summary>
    public class Lexeme
    {
        /// <summary>
        /// The ID of the identifier lexeme (this includes reserved words, preprogrammed methods, and variable names)
        /// </summary>
        public const int IDENTIFIER = 1;

        /// <summary>
        /// The list of accepted lexemes, loaded from disk.
        /// </summary>
        public static List<Lexeme> lexemes;

        /// <summary>
        /// The lexeme's ID.
        /// </summary>
        public int id;
        /// <summary>
        /// The symbol that represents the lexeme.
        /// </summary>
        public char symbol;
        /// <summary>
        /// Indicates whether or not the lexeme requires an attribute (the associated text).
        /// -True: The lexeme does require an attribute (identifier, literal, etc.)
        /// -False: The lexeme does not require an attribute (operators)
        /// </summary>
        public bool requiresAttribute;


        /// <summary>
        /// Locally used constructor to create new (blank) lexemes.
        /// </summary>
        private Lexeme()
        {

        }

        /// <summary>
        /// Reads the lexeme CSV file from disk into memory.
        /// </summary>
        /// <param name="path">The path where the lexeme CSV file is located.
        /// The .csv extension is not required.</param>
        public static void ReadLexemeCSV(string path)
        {
            StreamReader read = new StreamReader(path + ".csv");
            lexemes = new List<Lexeme>(29);
            int counter = 0;
            do
            {
                string line = read.ReadLine();
                string[] division = line.Split(',');
                Lexeme l = new Lexeme();
                l.id = ++counter;
                l.symbol = division[0].ToCharArray()[0];
                l.requiresAttribute = (int.Parse(division[1]) == 1);
                lexemes.Add(l);
            } while (!read.EndOfStream);
            read.Close();
        }

        /// <summary>
        /// Obtains the end of code lexme.
        /// </summary>
        /// <returns>A lexeme object that represents the end of the code.</returns>
        public static Lexeme GetTerminalLexeme()
        {
            Lexeme lexeme = new Lexeme();
            lexeme.symbol = '$';
            return lexeme;
        }
    }
}
