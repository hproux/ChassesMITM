namespace QuickTypeDM
{

    public class From
    {
        public int x { get; set; }
        public int y { get; set; }
        public string di { get; set; }
    }

    public class Hint
    {
        public int n { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int d { get; set; }
    }

    public class DofusMap
    {
        public From from { get; set; }
        public System.Collections.Generic.List<Hint> hints { get; set; }
    }
}