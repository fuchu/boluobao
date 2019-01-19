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
                string bID = bookHerf.Split('/')[2];
                Console.WriteLine(bID);
                books.Add(new Book(Convert.ToInt32(bID)));
                //books[0].GetValues(bookHerf);
            }
            foreach (var eachBook in books)
            {
                Console.WriteLine(eachBook.bookID);
                //eachBook.GetValues()
            }
            //Console.WriteLine(node.InnerHtml);
            Console.ReadKey();
        }
    }
    struct Book
    {
        public int bookID;
        public string title;
        //public uint words;
        //public DateTime updateTime;
        //public Array tags;
        //public uint pv;//click number
        public int likesNum;
        public int collectionNum;
        //public uint passNum;
        //public short score;
        //public string coverUrl;
        //public string author;
        //public string subject;//theme
        //public string description;
        public double rank;

        public Book(int bid)
        {
            string bookHtml = @"http://book.sfacg.com/Novel/" + bid;
            Console.WriteLine(bookHtml);
            HtmlWeb bookWeb = new HtmlWeb();
            HtmlDocument bookHtmlDoc = bookWeb.Load(bookHtml);
            title = bookHtmlDoc.DocumentNode.SelectSingleNode("//h1[@class='title']/span").InnerHtml;
            bookID = bid;
            //this.words = Convert.ToUInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//p[@class='introduce']").InnerHtml,16);
            likesNum = Convert.ToInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//*[@id='BasicOperation']/a[2]").InnerHtml.Split()[1]);
            collectionNum = Convert.ToInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//*[@id='BasicOperation']/a[3]").InnerHtml.Split()[1]);
            if (collectionNum < 500)
            {
                rank = (Convert.ToDouble(likesNum) * Convert.ToDouble(collectionNum)) / (500*Convert.ToDouble(collectionNum));
            }
            else
            {
                rank = Convert.ToDouble(likesNum) / Convert.ToDouble(collectionNum);
            }
            Console.WriteLine(title);
            Console.WriteLine(likesNum);
            Console.WriteLine(collectionNum);
            Console.WriteLine(string.Format("{0:0.00%}", rank));
            Console.WriteLine();
            //string this.title = bookNode.
        }
        
        public void GetValues(int bookID, string title, int words, DateTime updateTime, 
            Array tags, int pv, int likesNum, int collectionNum, 
            int passNum, short score, string coverUrl, string author, string subject, string description)
        {
            this.bookID = bookID;
            this.title = title;
            //this.introduce = words;
            //this.updateTime = updateTime;
            //this.tags = tags;
            //this.pv = pv;
            this.likesNum = likesNum;
            this.collectionNum = collectionNum;
            //this.passNum = passNum;
            //this.score = score;
            //this.coverUrl = coverUrl;
            //this.author = author;
            //this.subject = subject;
            //this.description = description;
        }
        public void BookDisplay()
        {
            //display the book info by context
        }
    }
}
