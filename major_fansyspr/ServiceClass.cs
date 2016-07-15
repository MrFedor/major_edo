

namespace major_fansyspr
{
    using System;

    public class ListCODEResult
    {
        public int num { get; set; }
        public string name { get; set; }
        public string dop { get; set; }
        public string alt { get; set; }
        public bool select { get; set; }
    }

    public class ListJsonResult
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class ListIndexResult
    {
        public Guid Id { get; set; }
        public string outNumber { get; set; }
        public DateTime outDate { get; set; }

        public string rubricaOut { get; set; }

        public string fond { get; set; }

        public string statusObrobotki { get; set; }
    }
    
}