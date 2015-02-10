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
using System.Collections;
using System.Collections.Specialized;

namespace YoutubeDownloader
{
    class Downloader
    {
        //Stores possible video formats for later use
        public string[] formatArr = new string[] { ".mp4", ".flv", ".3gp", ".webm" };

        //Gets the video id part of the youtube link
        public string GetId(string videoUrl)
        {
            string videoId = videoUrl.Remove(0,32);
            return videoId;
        }

        //Small method to string the video id to a youtube link
        public string GetVideoSource(string id)
        {
            string videoSource = "https://www.youtube.com/get_video_info?asv=3&el=detailpage&hl=en_US&video_id=" + id;
            return videoSource;
        }

        //Extracts the video title from the video xml file. Also removes any illegal characters from the file name.
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

        //Method handling everything related to the actual video download.
        public void videoDownload(string videoSource, string videoUrl, string videoTitle)
        {
            using (WebClient wc = new WebClient())
            {
                //Defines the priority of stream quality based on itags.
                string[] itagByPriority = { "38", "37", "46", "45", "22", "44", "35", "43", "34", "18", "36", "5", "17" };

                string encodedVideo = null;

                //Downloads the raw html source file.
                using (var client = new WebClient())
                {
                    encodedVideo = client.DownloadString(videoSource);
                }

                //Used to get the streams, and their respective key/value pairs.
                NameValueCollection video = HttpUtility.ParseQueryString(encodedVideo);

                string encodedStreamsCommaDelimited = video["url_encoded_fmt_stream_map"];
                string[] encodedStreams = encodedStreamsCommaDelimited.Split(new char[] { ',' });
                var streams = encodedStreams.Select(s => HttpUtility.ParseQueryString(s));

                //quick way of getting the best quality stream, can also be done with allStreams.
                var streamsByPriority = streams.OrderBy(s => Array.IndexOf(itagByPriority, s["itag"]));
                NameValueCollection preferredStream = streamsByPriority.FirstOrDefault();

                //Creates a NameValyeCollection that holds the key/value pairs for all streams. Used to get information about each individual stream. (itag, url, quality, fallback host, type)
                NameValueCollection allStreams = new NameValueCollection();
                int count = 0;
                foreach (NameValueCollection s in streams)
                {
                    allStreams.Add(streams.ElementAt(count));
                    count++;
                }
                
                string[] test = allStreams.GetValues(4);

                //Checks if file !exists, and downloads video to user defined location. If file exists user is promtet to use another name.
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
