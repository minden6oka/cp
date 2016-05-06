using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICService" in both code and config file together.
    [ServiceContract]
    public interface ICService
    {
        [OperationContract]
        User ValidateUser(string email, string password);

        [OperationContract]
        bool CreateUser(User newUser, string password);

        [OperationContract]
        bool CreatMovie(Movie newMovie);

        [OperationContract]
        IEnumerable<Movie> GetMovies();
        [OperationContract]
        byte[] GetImage(int from, int to, int idx);
        [OperationContract]
        int SetImage(string imageUri);

        [OperationContract]
        double AddRate(Rating rate);
        [OperationContract]
        Comment[] Commenting(Comment com);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "CinemaServiceDLL.ContractType".
    [DataContract]       
    public class Comment
    {
        private string user;
        private string title;
        private string userNick;
        private string content;

        public Comment(XElement comment,string title)
        {
            user = comment.Attribute("user").Value;
            this.title = title;
            userNick = comment.Attribute("usernick").Value;
            content = comment.Value;
        }

        public Comment(string user, string title,string userNick, string content)
        {
            this.user = user;
            this.title = title;
            this.content = content;
            this.userNick = userNick;
        }

        [DataMember]
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [DataMember]
        public string UserNick
        {
            get { return userNick; }
            set { userNick = value; }
        }
        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
    [DataContract]
    public enum UserType
    {
        [EnumMember]
        Administrator,
        [EnumMember]
        Moderator,
        [EnumMember]
        AverageUser
    }
    [DataContract]
    public enum GenderType
    {
        [EnumMember]
        Male,
        [EnumMember]
        Female,
        [EnumMember]
        Other
    }
    [DataContract]
    public class User
    {
        private string nickName;
        private DateTime birth;
        private string email;
        private UserType userType;
        private GenderType genderType;
        private Reservation[] reservations;

        public User(XElement user)
        {
            this.nickName = user.Attribute("nickname").Value;
            this.birth = DateTime.Parse(user.Attribute("birth").Value);
            this.email = user.Attribute("email").Value;
            this.userType = (UserType)int.Parse(user.Attribute("usertype").Value);
            this.genderType = (GenderType)int.Parse(user.Attribute("gender").Value);
            List<Reservation> temp = new List<Reservation>();
            foreach (XElement ress in user.Descendants("reservation"))
            {
                temp.Add(new Reservation(ress));
            }
            reservations = temp.ToArray();
        }
        public User(string nickname, DateTime birth, string email, UserType userType, GenderType genderType)
        {
            this.nickName = nickname;
            this.birth = birth;
            this.email = email;
            this.userType = userType;
            this.genderType = genderType;
            this.reservations = new Reservation[0];
        }
        [DataMember]
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        [DataMember]
        public DateTime Birth
        {
            get { return birth; }
            set { birth = value; }
        }
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public UserType UserType1
        {
            get { return userType; }
            set { userType = value; }
        }
        [DataMember]
        public GenderType GenderType1
        {
            get { return genderType; }
            set { genderType = value; }
        }
        [DataMember]
        public Reservation[] Reservations
        {
            get { return reservations; }
            set { reservations = value; }
        }
    }
    [DataContract]
    public class Reservation
    {
        private string title;
        private DateTime time;
        private RoomNames roomName;
        private int seat;
        private int row;

        public Reservation(string title,DateTime time, RoomNames roomName, int seat,int row)
        {
            this.title = title;
            this.time = time;
            this.roomName = roomName;
            this.seat = seat;
            this.row = row;
        }

        public Reservation(XElement res)
        {
            this.title = res.Attribute("title").Value;
            this.time = DateTime.Parse(res.Attribute("time").Value);
            this.roomName = (RoomNames)int.Parse(res.Attribute("roomname").Value);
            this.seat = int.Parse(res.Attribute("seat").Value);
            this.row = int.Parse(res.Attribute("row").Value);
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        [DataMember]
        public RoomNames RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }
        [DataMember]
        public int Seat
        {
            get { return seat; }
            set { seat = value; }
        }
        [DataMember]
        public int Row
        {
            get { return row; }
            set { row = value; }
        }
    }

    [DataContract]
    public class Rating
    {
        private string user;
        private RatingType ratetyType;
        private string title;

        public Rating(string user, RatingType ratingType, string title)
        {
            this.user = user;
            this.ratetyType = ratingType;
            this.title = title;
        }
        [DataMember]
        public string User1
        {
            get { return user; }
            set { user = value; }
        }
        [DataMember]
        public RatingType RatetyType
        {
            get { return ratetyType; }
            set { ratetyType = value; }
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }

    [DataContract]
    public enum RatingType
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Bad = 1,
        [EnumMember]
        Acceptable = 2,
        [EnumMember]
        Good = 3,
        [EnumMember]
        Excellent = 4,
        [EnumMember]
        JustWow = 5
    }
    [DataContract]
    public class Movie
    {
        private string title;
        private string description;
        private int length;
        private byte[] bmp;
        private double ratingValue;
        private RatingType ratingType;
        private DateTime[] screenTimes;
        private RoomNames room;
        private string imageUri;



        public Movie(XElement movie)
        {
            title = movie.Attribute("title").Value;
            description = movie.Attribute("description").Value;
            length = int.Parse(movie.Attribute("length").Value);
            bmp = null;
            imageUri = movie.Attribute("image").Value;
            List<int> temp = new List<int>();
            foreach (XElement args in movie.Descendants("rate"))
            {
                temp.Add(int.Parse(args.Value));
            }
            double sum = 0;
            foreach (int args in temp)
            {
                sum += args;
            }
            ratingValue = temp.Count==0 ? 0.0 : sum / temp.Count;
            ratingType = (RatingType)((int)ratingValue);

            List<DateTime> dates = new List<DateTime>();
            foreach (XElement descendant in movie.Descendants("time"))
            {
                dates.Add(DateTime.Parse(descendant.Value));
                room = (RoomNames)int.Parse(descendant.Attribute("room").Value);
            }
            screenTimes = dates.ToArray();
        }

        public Movie(string title,string description, int length,string imageUri,RoomNames room,DateTime[] screenTimes)
        {
            this.title = title;
            this.description = description;
            this.length = length;
            this.screenTimes = screenTimes;
            this.room = room;
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap temp = (Bitmap)Bitmap.FromFile(imageUri);
                temp.Save(ms,ImageFormat.Jpeg);
                bmp = ms.ToArray();
            }
        }
        
        public string RatingOutput
        {
            get { return String.Format("5/{0:n1} {1}", this.ratingValue, this.ratingType); }
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        [DataMember]
        public double RatingValue
        {
            get { return ratingValue; }
            set { ratingValue = value; }
        }
        [DataMember]
        public RatingType RatingType1
        {
            get { return ratingType; }
            set { ratingType = value; }
        }
        [DataMember]
        public DateTime[] ScreenTimes
        {
            get { return screenTimes; }
            set { screenTimes = value; }
        }
        [DataMember]
        public byte[] Bmp
        {
            get { return bmp; }
            set { bmp = value; }
        }
        [DataMember]
        public RoomNames Room
        {
            get { return room; }
            set { room = value; }
        }
        [DataMember]
        public string ImageUri
        {
            get { return imageUri; }
            set { imageUri = value; }
        }
    }
    [DataContract]
    public enum RoomNames
    {
        [EnumMember]
        Cheddar
    }
    }

