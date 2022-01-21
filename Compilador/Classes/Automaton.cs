using System.IO;
using System.Collections;
using Newtonsoft.Json;

namespace Compilador.Clases
{
    /// <summary>
    /// This class represents an object that contains the necessary data to store an Automaton,
    /// and the method required to load one into memory.
    /// </summary>
    public class Automaton
    {
        /// <summary>
        /// Array of allowed states in the Automaton.
        /// </summary>
        public int[] states;

        /// <summary>
        /// Array of main characters allowed in language alphabet.
        /// </summary>
        public char[] alphabet;

        /// <summary>
        /// Matrix of Automaton's transition states.
        /// </summary>
        public int[][] transitions;

        /// <summary>
        /// Quickly obtains the alphabet index of a character.
        /// </summary>
        public Hashtable hash;


        /// <summary>
        /// Loads the Automaton into memory from the JSON file on disk.
        /// Also populates the Hashtable with the indexes of each character in the alphabet array.
        /// </summary>
        /// <param name="path">The string path pointing to the location of the JSON file
        /// (should not include the file extension).</param>
        /// <returns>Returns the automaton loaded into memory from the JSON file.</returns>
        public static Automaton LoadAutomaton(string path)
        {
            StreamReader read = new StreamReader(path + ".json");
            Automaton automaton = JsonConvert.DeserializeObject<Automaton>(read.ReadToEnd());
            read.Close();
            automaton.hash = new Hashtable(automaton.alphabet.Length);
            for (int i = 0; i < automaton.alphabet.Length; i++)
            {
                automaton.hash.Add(automaton.alphabet[i], i);
            }
            return automaton;
        }
    }
}
