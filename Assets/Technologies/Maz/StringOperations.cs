
namespace Maz.String
{
    public class StringOperations
    {
        /// <summary>
        /// Itterations to left string, when left string is the same like right return true. 
        /// </summary>
        /// <param name="left"> Itteration to string.</param>
        /// <param name="right"> string to check.</param>
        /// <returns>Returns an bool based on values two strings.</returns>
        public static bool CheckStrings(string left,string right)
        {
            if (left.Length > right.Length) return false;
        
            for (int i = 0; i < left.Length; i++)
                if (left[i] != right[i]) return false;

            return true;
        }
    }
}
