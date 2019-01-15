using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = @"http://book.sfacg.com/List/?ud=7";
            var web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            //var node = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[4]/ul[1]/li[1]/a");
            var nodes = htmlDoc.DocumentNode.SelectNodes("//li[@class='Conjunction']/a");
            List<Book> books = new List<Book>();
            foreach (var node in nodes)
            {
                string bookHerf = node.Attributes["href"].Value;
                var bookHtml = @"http://book.sfacg.com/Novel/";
                var bookWeb = new HtmlWeb();
                var bookHtmlDoc = bookWeb.Load(bookHtml);
                var bookNodes = bookHtmlDoc.DocumentNode.SelectNodes("");
            }
            //Console.WriteLine(node.InnerHtml);
            Console.ReadKey();
        }
    }
    struct Book
    {
        public uint bookID;
        public string title;
        public uint words;
        public DateTime updateTime;
        public Array tags;
        public uint pv;//click number
        public uint likesNum;
        public uint collectionNum;
        public uint passNum;
        public short score;
        public string coverUrl;
        public string author;
        public string subject;//theme
        public string description;

        public void GetValues(string bookHerf)
        {
            var bookHtml = @"http://book.sfacg.com"+ bookHerf;
            var bookWeb = new HtmlWeb();
            var bookHtmlDoc = bookWeb.Load(bookHtml);
            var bookNodes = bookHtmlDoc.DocumentNode.SelectNodes("");
        }
        
        public void GetValues(uint bookID, string title, uint words, DateTime updateTime, 
            Array tags, uint pv, uint likesNum, uint collectionNum, 
            uint passNum, short score, string coverUrl, string author, string subject, string description)
        {
            this.bookID = bookID;
            this.title = title;
            this.words = words;
            this.updateTime = updateTime;
            this.tags = tags;
            this.pv = pv;
            this.likesNum = likesNum;
            this.collectionNum = collectionNum;
            this.passNum = passNum;
            this.score = score;
            this.coverUrl = coverUrl;
            this.author = author;
            this.subject = subject;
            this.description = description;
        }
        public void BookDisplay()
        {
            //display the book info by context
        }
    }
}
