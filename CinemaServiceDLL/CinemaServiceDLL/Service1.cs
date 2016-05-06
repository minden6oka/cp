using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace CinemaServiceDLL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CService" in both code and config file together.
    public class CService : ICService
    {
        private const string USERSFILE = "users.xml";
        private const string MOVIEFILE = "movies.xml";

        XElement SearchMovie(string title, XDocument movies)
        {
            foreach (XElement args in movies.Descendants("movie"))
            {
                if (args.Attribute("title").Value.ToLower() == title.ToLower())
                {
                    return args;
                }
            }
            return null;
        }

        public Comment[] Commenting(Comment com)
        {
            List<Comment> comments = new List<Comment>();
            XDocument users = null;
            if (File.Exists(MOVIEFILE) && File.Exists(USERSFILE))
            {
                XDocument movies = XDocument.Load(MOVIEFILE);
                XElement movie = SearchMovie(com.Title, movies);
                if (movie != null)
                {
                    if (com != null)
                    {
                        users = XDocument.Load(USERSFILE);
                        foreach (XElement descendant in users.Descendants("user"))
                        {
                            if (descendant.Attribute("email").Value.ToLower() == com.User.ToLower())
                            {
                                if (descendant.Attribute("banned").Value != "1")
                                {
                                    break;
                                }
                                movie.Element("comments")
                                    .Add(new XElement("comment", new XAttribute("nick", com.UserNick),
                                        new XAttribute("user", com.User), com.Content));
                                movies.Save(MOVIEFILE);
                            }
                        }
                    }
                    foreach (XElement descendant in movie.Descendants("comment"))
                    {
                        comments.Add(new Comment(descendant, movie.Attribute("title").Value));
                    }
                }
            }
            return comments.ToArray();
        }


        public User ValidateUser(string email, string password)
        {
            XDocument doc = null;
            if (File.Exists(USERSFILE))
            {
                doc = XDocument.Load(USERSFILE);
                foreach (XElement user in doc.Descendants("user"))
                {
                    if (user.Attribute("email").Value.ToLower() == email.ToLower())
                    {
                        if (user.Attribute("password").Value == password)
                        {
                            return new User(user);
                        }
                        return null;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }


        public double AddRate(Rating rate)
        {
            XDocument doc = null;
            if (File.Exists(MOVIEFILE))
            {
                doc = XDocument.Load(MOVIEFILE);
                foreach (XElement args in doc.Descendants("movie"))
                {
                    if (args.Attribute("title").Value.ToLower() == rate.Title.ToLower())
                    {
                        args.Element("ratings")
                            .Add(new XElement("rating", new XAttribute("user", rate.User1), (int)rate.RatetyType));
                        double sum = 0;
                        int count = 0;
                        foreach (XElement args2 in args.Element("ratings").Elements("rating"))
                        {
                            sum += double.Parse(args2.Value);
                            count++;
                        }
                        doc.Save(MOVIEFILE);
                        return sum / count;
                    }
                }
            }
            return 0;
        }


        public bool CreateUser(User newUser, string password)
        {
            XDocument doc;
            if (File.Exists(USERSFILE))
            {
                doc = XDocument.Load(USERSFILE);
            }
            else
            {
                doc = new XDocument(new XElement("users"));
            }
            if (doc.Element("users").HasElements)
                foreach (XElement xElement in doc.Element("users").Elements("user"))
                {
                    using (StreamWriter sw = new StreamWriter("data.txt")) sw.WriteLine(xElement);
                    if (xElement.Attribute("email").Value.ToLower() == newUser.Email.ToLower())
                    {
                        return false;
                    }
                }
            doc.Element("users").Add(new XElement("user", new XAttribute("email", newUser.Email), new XAttribute("password", password),
                new XAttribute("banned", 0), new XAttribute("birth", newUser.Birth), new XAttribute("gender", (int)newUser.GenderType1),
                new XAttribute("nickname", newUser.NickName), new XAttribute("usertype", (int)newUser.UserType1), new XElement("reservations")));
            doc.Save(USERSFILE);
            return true;
        }

        public bool CreatMovie(Movie newMovie)
        {
            XDocument doc = null;
            if (File.Exists(MOVIEFILE))
            {
                doc = XDocument.Load(MOVIEFILE);
            }
            else
            {
                doc = new XDocument(new XElement("movies", new XAttribute("pic", 0)));
            }
            foreach (XElement xElement in doc.Descendants("movie"))
            {
                if (xElement.Attribute("title").Value.ToLower() == newMovie.Title.ToLower())
                {
                    return false;
                }
            }
            string path = "movies\\" + newMovie.Title.ToLower();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(newMovie.Bmp,0,newMovie.Bmp.Length);
                ms.Position = 0;
                Bitmap temp = (Bitmap)Bitmap.FromStream(ms);
                temp.Save(path, ImageFormat.Jpeg);
            }
            XElement times = new XElement("screentimes");
            foreach (DateTime time in newMovie.ScreenTimes)
            {
                times.Add(new XElement("time",new XAttribute("room",(int)newMovie.Room),new XElement("reservations"), time));
            }
            doc.Element("movies").Add(new XElement("movie", new XAttribute("title", newMovie.Title), new XAttribute("description",
                newMovie.Description), new XAttribute("image", path), new XAttribute("length", newMovie.Length), new XElement("comments"),
                new XElement("ratings"), new XElement("times",times)));
            doc.Save(MOVIEFILE);
            return true;
        }

        IEnumerable<Movie> ICService.GetMovies()
        {
            XDocument doc = null;
            if (File.Exists(MOVIEFILE))
            {
                doc = XDocument.Load(MOVIEFILE);
                foreach (XElement descendant in doc.Descendants("movie"))
                {
                     yield return new Movie(descendant);
                }
            }
            doc.Save(MOVIEFILE);
        }

        private static List<int> usedNumbers;
        private static List<byte[]> images;

        public byte[] GetImage(int from, int to, int idx)
        {
            List<byte> temp = new List<byte>();

            bool finished = false;
            while (from < to)
            {
                temp.Add(images[idx][from]);
                from++;

                if (from == images[idx].Length)
                {
                    finished = true;
                    break;
                }
            }

            if (from > images[idx].Length || finished)
            {
                images[idx] = null;
                lock (usedNumbers)
                {
                    usedNumbers.Remove(idx);
                }
            }

            return temp.ToArray();
        }

        public int SetImage(string imageuri)
        {
            if (images == null)
            {
                images = new List<byte[]>();
                usedNumbers = new List<int>();
            }

            int idx = 0;

            lock (usedNumbers)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (i == images.Count || images[i] == null)
                    {
                        usedNumbers.Add(i);
                        idx = i;
                        break;
                    }
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(imageuri);
                bmp.Save(ms,ImageFormat.Jpeg);
                ms.Position = 0;
                images.Add(ms.ToArray());
            }
            return idx;
        }
    }
}
