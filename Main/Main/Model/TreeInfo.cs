namespace Main.Model
{
    class TreeInfo
    {
        private string _ident;
        public string Ident { get { return _ident; } }
        private int _depth;
        public int Depth { get { return _depth; } }
        public TreeInfo(string ident, int depth)
        {
            _ident = ident;
            _depth = depth;
        }
    }
}