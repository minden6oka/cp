using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Dynamic;
using CinemaProject.CinemaCervises;

namespace CinemaProject
{
    class Server
    {
        
        private static CServiceClient client;

        public Server()
        {
            client = new CServiceClient();
        }
        public static Comment[] Comment(Comment newComment)
        {
            //send if succes true and add to listbox
            return client.Commenting(newComment);
        }
        //if we can get a Rating object nice else lets stick to the idea that we only get a double or something
        public static double AddRate(Rating rate)
        {
            return client.AddRate(rate);
        }
        public static User ValidateUser(string username, string password)
        {
            return client.ValidateUser(username, password);
        }
        public static Movie[] GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            foreach (Movie movie in client.GetMovies())
            {
                Movie temp = movie;
                int idx = client.SetImage(temp.ImageUri);
                int from = 0;
                int to = 20000;
                List<byte> bytes = new List<byte>();
                while (true)
                {
                    byte[] tb = client.GetImage(from, to, idx);
                    for (int i = 0; i < tb.Length; i++)
                    {
                        bytes.Add(tb[i]);
                    }
                    from += 20000;
                    to += 20000;
                    if (tb.Length < 20000)
                    {
                        break;
                    }
                }
                temp.Bmp = bytes.ToArray();
                movies.Add(temp);
            }
            string sajt;
            return movies.ToArray();
        }

        public static bool CreateMovie(Movie newMovie)
        {
            return client.CreatMovie(newMovie);
        }

        public static bool CreateUser(User newUser,string password)
        {
            return client.CreateUser(newUser, password);
        }
    }
}
