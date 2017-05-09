using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Control
{
    public class KeyController
    {

        public static bool IsValideLetter(char l)
        {
            foreach (var key in Keys.All)
            {
                foreach (char letter in key.Letters)
                {
                    if (letter == l)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsValideKey(char k)
        {
            return (GetKeyByName(k) != default(Key));
        }

        public static Key GetKeyByName(char name)
        {
            return Keys.All.FirstOrDefault(k => k.Name == name);
        }

        public static Key GetKeyToLetter(char letter)
        {
            var key = Keys.All.First(k => k.Letters.Contains(letter));
            if(key == null){
                key = Keys.Six;
            }
            return key;
        }
    }
}
