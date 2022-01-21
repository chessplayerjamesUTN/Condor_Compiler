using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador.Clases
{
    /// <summary>
    /// This class contains two main lists:
    ///     -All of the possible recorded errors the compiler displays (divided by type, shown by number)
    ///     -All of the errors detected when scanning or compiling the program
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The approximate total number of errors to load from disk.  This number need not be exact,
        /// but it allows the compiler to know how large the list will be during compile time.
        /// (The number currently is exact)
        /// </summary>
        private const int numErrors = 79;

        /// <summary>
        /// This array divides the error type names in order.
        /// -1XX Errors - File IO
        /// -2XX Errors - Lexical
        /// ...
        /// </summary>
        public static readonly string[] TYPES = { "Archivo", "Léxico", "Sintáctico", "Semántico",
            "Tiempo de ejecución", "Misceláneo" };

        /// <summary>
        /// The list of errors found while scanning or compiling.
        /// </summary>
        public static List<Error> errorsFound;

        /// <summary>
        /// The list of errors loaded from disk.
        /// </summary>
        public static List<Error> loadedErrorList;

        /// <summary>
        /// The ID of the error loaded from disk (200,303, 410, etc.).
        /// </summary>
        public int id;
        /// <summary>
        /// The length of the word found (used for underlining lexical errors).
        /// </summary>
        public int length;
        /// <summary>
        /// Saves the line number, indicating where the error was found.
        /// </summary>
        public int lineNum;
        /// <summary>
        /// Saves the error type (0-5).
        /// </summary>
        public int type;
        /// <summary>
        /// The error description (related to the error ID, from the errors loaded from disk).
        /// </summary>
        public string description;
        /// <summary>
        /// The offending text that causes the error (when possible).
        /// </summary>
        public string incorrectText;

        /// <summary>
        /// The constructor used when loading the error list from disk.
        /// </summary>
        /// <param name="id">The error code.</param>
        /// <param name="description">The error description, explaining what's wrong and how to
        /// fix it.</param>
        public Error(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Loads the error list from a TXT file on disk into memory.
        /// Errors are then saved into the loadedErrorList list variable.
        /// </summary>
        /// <param name="path">The file path where the error list is found.
        /// The .txt extension is not required.</param>
        public static void ReadErrorsTXT(string path)
        {
            StreamReader read = new StreamReader(path + ".txt");
            loadedErrorList = new List<Error>(numErrors);
            errorsFound = new List<Error>();
            do
            {
                string[] line = read.ReadLine().Split('\t');
                loadedErrorList.Add(new Error(int.Parse(line[0]), line[1]));
            } while (!read.EndOfStream);
            read.Close();
        }

        /// <summary>
        /// Obtains the lexical error based on the current state in the automaton.
        /// </summary>
        /// <param name="currentState">The current state in the automaton where an error
        /// has been found.</param>
        /// <returns>Returns the lexical error based off of the current automaton state.</returns>
        public static Error GetLexicalError(int currentState)
        {
            foreach (Error error in loadedErrorList)
            {
                if ((error.id >= 200) && (error.id < 300) && (error.id == 200 + currentState))
                {
                    return error;
                }
            }
            throw new Exception(loadedErrorList[loadedErrorList.Count - 1].id + ": "
                + loadedErrorList[loadedErrorList.Count - 1].description);
        }

        /// <summary>
        /// Obtains the syntactic error based on the transition found in the stack automaton.
        /// </summary>
        /// <param name="ruleNum">The current transition (error state) in the stack automaton.</param>
        /// <returns>Returns the syntactic error found.</returns>
        public static Error GetSyntaxisError(int ruleNum)
        {
            foreach (Error error in loadedErrorList)
            {
                if ((error.id >= 300) && (error.id < 400) && (error.id == 300 + ruleNum))
                {
                    return error;
                }
            }
            throw new Exception(loadedErrorList[loadedErrorList.Count - 1].id + ": "
                + loadedErrorList[loadedErrorList.Count - 1].description);
        }

        /// <summary>
        /// Obtains the semantic error based on the data type entered.
        /// </summary>
        /// <param name="type">A negative number showing the data type or error status.</param>
        /// <returns>Returns the data type based on the data type or error status entered.</returns>
        public static Error GetSemanticError(int type)
        {
            type *= -1;
            foreach (Error error in loadedErrorList)
            {
                if ((error.id >= 400) && (error.id < 500) && (error.id == 400 + type))
                {
                    return error;
                }
            }
            throw new Exception(loadedErrorList[loadedErrorList.Count - 1].id + ": "
                + loadedErrorList[loadedErrorList.Count - 1].description);
        }

        /// <summary>
        /// Obtains the run-time error based on the error code entered.
        /// </summary>
        /// <param name="errorCode">The run-time error code.</param>
        /// <returns>The error related to the error code entered.</returns>
        public static Error GetRuntimeError(int errorCode)
        {
            foreach (Error error in loadedErrorList)
            {
                if ((error.id >= 500) && (error.id < 600) && (error.id == 500 + errorCode))
                {
                    return error;
                }
            }
            throw new Exception(loadedErrorList[loadedErrorList.Count - 1].id + ": "
                + loadedErrorList[loadedErrorList.Count - 1].description);
        }
    }
}
