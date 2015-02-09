using System;
using Microsoft.VisualBasic;
using System.Xml;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace YoutubeDownloader
{
    class Downloader
    {
        public string[] formatArr = new string[] { ".mp4", ".flv", ".3gp", ".webm" };

        public string GetId(string videoUrl)
        {
            string videoId = videoUrl.Remove(0,32);
            return videoId;
        }

        public string GetVideoSource(string id)
        {
            string videoSource = "https://www.youtube.com/get_video_info?asv=3&el=detailpage&hl=en_US&video_id=" + id;
            return videoSource;
        }

        public string videoInfo(string node, string videoUrl)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"http://www.youtube.com/oembed?url=" + videoUrl + "&format=xml");
            XmlNode titleNode = xmlDoc.SelectSingleNode(node);

            string title = titleNode.InnerText;
          
            string[] illegalChar = new string[] { @"\", "/", ":", "*", "?", '"'.ToString(), "<", ">", "|"};

            foreach (string notAllowed in illegalChar)
            {
               title = title.Replace(notAllowed, "_");
            }

            return title;
        }

        public void videoDownload(string videoSource, string videoUrl, string videoTitle)
        {
            using (WebClient wc = new WebClient())
            {
                string[] itagByPriority = { "38", "37", "46", "45", "22", "44", "35", "43", "34", "18", "36", "5", "17" };

                string encodedVideo = null;

                using (var client = new WebClient())
                {
                    encodedVideo = client.DownloadString(videoSource);
                }

                NameValueCollection video = HttpUtility.ParseQueryString(encodedVideo);

                string encodedStreamsCommaDelimited = video["url_encoded_fmt_stream_map"];
                string[] encodedStreams = encodedStreamsCommaDelimited.Split(new char[] { ',' });
                var streams = encodedStreams.Select(s => HttpUtility.ParseQueryString(s));

                var streamsByPriority = streams.OrderBy(s => Array.IndexOf(itagByPriority, s["itag"]));
                NameValueCollection preferredStream = streamsByPriority.FirstOrDefault();

                string test = preferredStream["itag"];

                //foreach (string s in preferredStream)
                //{
                //    foreach (string value in preferredStream.GetValues(s))
                //    {
                //        Console.WriteLine("{0}: {1}", s, value);
                //    }
                //}

                if (!File.Exists(@"C:\Users\SebastianEsp\Desktop\" + videoTitle + formatArr[0]))
                {
                    wc.DownloadFile(preferredStream["url"], @"C:\Users\SebastianEsp\Desktop\" + videoTitle + formatArr[0]);
                }
                else
                {
                    string newName = Interaction.InputBox("A file with the same name already exist!\nPlease choose another name", "File already exist", "Enter new name here");
                    wc.DownloadFile(preferredStream["url"], @"C:\Users\SebastianEsp\Desktop\" + newName + formatArr[0]);
                }
            }
        }
    }
}
