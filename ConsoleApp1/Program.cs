using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();
            var html = @"http://book.sfacg.com/List/?ud=7";
            var web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            //var node = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[4]/ul[1]/li[1]/a");
            //var nodes = htmlDoc.DocumentNode.SelectNodes("//li[@class='Conjunction']/a");
            var pagesNodes = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='list_pages']/ul[@class='nav pagebar']/li[4]/a");
            int pages = Convert.ToInt32(pagesNodes.InnerHtml);
            //Console.WriteLine(pages);
            for (int i = pages; i > 0; i--)
            {
                Console.WriteLine("Complete page: {0}/{1}", pages - i, pages);
                var ihtml = @"http://book.sfacg.com/List/default.aspx?ud=7&PageIndex=" + i;
                var iweb = new HtmlWeb();
                var ihtmlDoc = web.Load(ihtml);
                var nodes = ihtmlDoc.DocumentNode.SelectNodes("//li[@class='Conjunction']/a");
                foreach (var node in nodes)
                {
                    string bookHerf = node.Attributes["href"].Value;
                    int bID = Convert.ToInt32(bookHerf.Split('/')[2]);
                    //Console.WriteLine(bID);
                    //var index = books.FindIndex(c => c.bookID == bID);
                    bool containsItem = books.Any(o => o.bookID == bID);
                    if (containsItem)
                    {
                        var index = books.FindIndex(c => c.bookID == bID);
                        books[index] = new Book(bID);
                    }
                    else
                    {
                        books.Add(new Book(bID));

                    }
                    //books[0].GetValues(bookHerf);
                }
            }
            //books.Sort(delegate (Book x, Book y) { return x.rank.CompareTo(y.rank); });
            List<Book> sortBooks = books.OrderByDescending(o => o.rank).ToList();
            Console.WriteLine(sortBooks.Count);
            writeExcel("sortList.csv",sortBooks);
            void writeExcel(string dstFile,List<Book> sbooks)
            {
                StreamWriter sw = new StreamWriter(dstFile, false, Encoding.GetEncoding("utf-8"));
                using (sw)
                {
                    //PropertyInfo[] props = GetPropertyInfoArray();
                    //for (int i = 0; i < props.Length; i++)
                    //{
                    //    sw.WriteLine(props[i].Name); //write the column name
                    //}
                    sw.WriteLine($"BookID,书名,参考分(测试中),赞,收藏,字数,更新状态,作者,标签,链接");
                    for (int i = 0; i < sbooks.Count; i++)
                    {
                        Book eachBook = sbooks[i];
                        string tagStr = string.Join(";", eachBook.tags.ToArray()).Replace(System.Environment.NewLine, "");
                        sw.WriteLine($"{eachBook.bookID}," +
                            $"{eachBook.title}," +
                            $"{string.Format("{0:0.00%}", eachBook.rank)}," +
                            $"{eachBook.likesNum}," +
                            $"{eachBook.collectionNum}," +
                            $"{eachBook.words}," +
                            $"{eachBook.status}," +
                            $"{eachBook.author}," +
                            $"{tagStr}," + 
                            $"{eachBook.link}");
                    }
                }
            }
            //StreamWriter sw = new StreamWriter("sortList.csv", false, Encoding.GetEncoding("utf-8"));
            //using (sw)
            //{
            //    //PropertyInfo[] props = GetPropertyInfoArray();
            //    //for (int i = 0; i < props.Length; i++)
            //    //{
            //    //    sw.WriteLine(props[i].Name); //write the column name
            //    //}
            //    for (int i = 0; i < sortBooks.Count; i++)
            //    {
            //        Book eachBook = sortBooks[i];
            //        sw.WriteLine($"{eachBook.bookID}," +
            //            $"{eachBook.title}," +
            //            $"{string.Format("{0:0.00%}", eachBook.rank)}," +
            //            $"{eachBook.likesNum}," +
            //            $"{eachBook.collectionNum}," +
            //            $"{eachBook.words}," +
            //            $"{eachBook.status}," + 
            //            $"{eachBook.author}");
            //    }
            //}
            //foreach (var eachBook in sortBooks)
            //{
            //    Console.WriteLine(eachBook.title);
            //    Console.WriteLine(eachBook.likesNum);
            //    Console.WriteLine(eachBook.collectionNum);
            //    Console.WriteLine(string.Format("{0:0.00%}", eachBook.rank));
            //}
            //Console.WriteLine(node.InnerHtml);
        }
    }
    struct Book
    {
        public int bookID;
        public string title;
        public int words;
        public string status;
        //public DateTime updateTime;
        public ArrayList tags;
        //public uint pv;//click number
        public int likesNum;
        public int collectionNum;
        //public uint passNum;
        //public short score;
        //public string coverUrl;
        public string author;
        //public string subject;//theme
        //public string description;
        public double rank;
        public string link;

        public Book(int bid)
        {
            string bookHtml = @"http://book.sfacg.com/Novel/" + bid;
            //Console.WriteLine(bookHtml);
            HtmlWeb bookWeb = new HtmlWeb();
            HtmlDocument bookHtmlDoc = bookWeb.Load(bookHtml);
            title = bookHtmlDoc.DocumentNode.SelectSingleNode("//h1[@class='title']/span").InnerHtml.Replace(System.Environment.NewLine, "");
            bookID = bid;
            //this.words = Convert.ToUInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//p[@class='introduce']").InnerHtml,16);
            likesNum = Convert.ToInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//*[@id='BasicOperation']/a[2]").InnerHtml.Split()[1]);
            collectionNum = Convert.ToInt32(bookHtmlDoc.DocumentNode.SelectSingleNode("//*[@id='BasicOperation']/a[3]").InnerHtml.Split()[1]);
            string wordsAndstatus = bookHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='count-detail']/div[@class='text-row']/span[2]").InnerHtml.Split()[0];
            words = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(wordsAndstatus,@"[^0-9]+", ""));
            status = wordsAndstatus.Split('[', ']')[1].Replace(System.Environment.NewLine, "");
            author = bookHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='author-name']/span").InnerHtml.Replace(System.Environment.NewLine, "");
            HtmlNodeCollection tagList = bookHtmlDoc.DocumentNode.SelectNodes("//li[@class='tag']/a/span[@class='text']");
            tags = new ArrayList();
            if (tagList!=null)
            {
                foreach (var item in tagList)
                {
                    tags.Add(item.InnerHtml);
                }
            }
            link = bookHtml;
            switch (collectionNum)
            {
                case int n when (n<=500):
                    rank = (Convert.ToDouble(likesNum) * Convert.ToDouble(collectionNum)) / (500 * Convert.ToDouble(collectionNum));
                    break;
                //case int n when (collectionNum > 500 && collectionNum <= 3000):
                //    rank = (Convert.ToDouble(likesNum) * Convert.ToDouble(collectionNum)) / (3000 * Convert.ToDouble(collectionNum));
                //    break;
                //case int n when (collectionNum > 3000 && collectionNum <= 10000):
                //    rank = (Convert.ToDouble(likesNum) * Convert.ToDouble(collectionNum)) / (10000 * Convert.ToDouble(collectionNum));
                //    break;
                default:
                    rank = Convert.ToDouble(likesNum) / Convert.ToDouble(collectionNum);
                    break;
            }
            //Console.WriteLine(title);
            //Console.WriteLine(likesNum);
            //Console.WriteLine(collectionNum);
            //Console.WriteLine(string.Format("{0:0.00%}", rank));
            //Console.WriteLine();
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
