using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace getdata
{
    class Program
    {
     
        static void Main(string[] args)
        {

            Process("http://tinbds.com/tk/ho-chi-minh/binh-thanh/p-5");
        }

        static String ReadData(string add)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            string webData = wc.DownloadString(add);
            return webData;
        }
        static void Process(string add)
        {

            var doc = new HtmlDocument();
            doc.LoadHtml(ReadData(add));

            var centercontent = doc.DocumentNode.SelectNodes("//section[@id='center-content']/div").FirstOrDefault();
            var article = centercontent.SelectNodes("article");
            var items = new List<Person>();

            foreach (var a in article)
            {
                Person person = new Person();
                //Extract các giá trị từ các tag con của tag a
                var linkNode = a.SelectSingleNode(".//a[contains(@class,'title')]");
           
                person.Name = linkNode.InnerText;

                //lay cac the p
                var pNode = a.SelectNodes(".//figure/figcaption/p").ToList();


                //xóa node strong
                var strongNode = a.SelectNodes(".//figure/figcaption/p/strong").ToList();
                foreach (var c in strongNode)
                {
                    c.Remove();
                }

                // Lấy nội dung file p
                person.Dc = pNode[0].InnerHtml;
                person.Sdt = pNode[1].InnerHtml;
                person.Email = pNode[2].InnerHtml;

                items.Add(person);

            }

            String filepath = "D:\\test.txt";
            FileStream fs = new FileStream(filepath, FileMode.Create);//Tạo file mới tên là test.txt  ;
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
         



       
         
            foreach (var a in items)
            {
                Console.WriteLine("ten:" + a.Name + " dc: " + a.Dc + " sdt: " + a.Sdt + " email: " + a.Email);
                sWriter.WriteLine("ten:" + a.Name + " dc: " + a.Dc + " sdt: " + a.Sdt + " email: " + a.Email); ;
            }

            sWriter.Flush();
            fs.Close();
            Console.ReadKey();

        }
    }
}
