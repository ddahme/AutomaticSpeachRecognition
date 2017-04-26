namespace Main.Model
{
    static class Keys
    {
        public static Key Zero = new Key('0', new char[] { '0' });
        public static Key One = new Key('1', new char[] { '1', '.', ',' });
        public static Key Two = new Key('2', new char[] { '2', 'a', 'A', 'b', 'B', 'c', 'C' });
        public static Key Three = new Key('3', new char[] { '3', 'd', 'D', 'e', 'E', 'f', 'F' });
        public static Key Four = new Key('4', new char[] { '4', 'g', 'G', 'h', 'H', 'i', 'I' });
        public static Key Five = new Key('5', new char[] { '5', 'j', 'J', 'k', 'K', 'l', 'L' });
        public static Key Six = new Key('6', new char[] { '6', 'm', 'M', 'n', 'N', 'o', 'O' });
        public static Key Seven = new Key('7', new char[] { '7', 'p', 'P', 'q', 'Q', 'r', 'R', 's', 'S' });
        public static Key Eight = new Key('8', new char[] { '8', 't', 'T', 'u', 'U', 'v', 'V' });
        public static Key Nine = new Key('9', new char[] { '9', 'w', 'W', 'x', 'X', 'y', 'Y', 'z', 'Z' });
        public static Key Star = new Key('*', new char[] { });
        public static Key Hash = new Key('#', new char[] { ' ' });
        public static Key[] All = new Key[] { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Star, Hash };
    }
}
