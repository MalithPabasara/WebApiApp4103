﻿namespace WebApiApp4103.Models
{
    public class Books
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int YearPublished { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
    }
}
