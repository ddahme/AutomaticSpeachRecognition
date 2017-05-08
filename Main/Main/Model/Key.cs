namespace Main.Model
{
    public class Key
    {
        private char[] _letters;
        public char[] Letters
        {
            get
            {
                return _letters;
            }
        }
        private char _name;
        public char Name
        {
            get
            {
                return _name;
            }
        }
        public Key(char name, char[] letters)
        {
            _letters = letters;
            _name = name;
        }
    }
}
