using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDbAPIClient.Model
{
    public class Movie : INotifyPropertyChanged
    {
        #region Fields
        private string title;
        private string year;
        private string rated;
        private string released;
        private string runtime;
        private string genre;
        private string director;
        private string writer;
        private string actors;
        private string plot;
        private string country;
        private string awards;
        private string poster;
        private IEnumerable<Rating> ratings;
        private string metascore;
        private string imdbRating;
        private string imdbVotes;
        private string imdbID;
        private string type;
        private string dVD;
        private string boxOffice;
        private string production;
        private string website;
        private bool response;
        #endregion

        #region Properties
        public string Title
        {
            get { return title; }
            set {
                if (value == title) return;
                title = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        public string Year
        {
            get { return year; }
            set
            {
                if (value == year) return;
                    year = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Year"));
            }
        }
        public string Rated
        {
            get { return rated; }
            set
            {
                if (value == rated) return;
                    rated = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Rated"));
            }
        }
        public string Released
        {
            get { return released; }
            set
            {
                if (value == released) return;
                released = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Released"));
            }
        }
        public string Runtime
        {
            get { return runtime; }
            set
            {
                if (value == runtime) return;
                    runtime = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Runtime"));
            }
        }
        public string Genre
        {
            get { return genre; }
            set
            {
                if (value == genre) return;
                    genre = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Genre"));
            }
        }
        public string Director
        {
            get { return director; }
            set
            {
                if (value == director) return;
                    director = value;
                 OnPropertyChanged(new PropertyChangedEventArgs("Director"));
            }
        }
        public string Writer
        {
            get { return writer; }
            set
            {
                if (value == writer) return;
                    writer = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Writer"));
            }
        }
        public string Actors
        {
            get { return actors; }
            set
            {
                if (value == actors) return;
                    actors = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Actors"));
            }
        }
        public string Plot
        {
            get { return plot; }
            set
            {
                if (value == plot) return;
                    plot = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Plot"));
            }
        }
        public string Country
        {
            get { return country; }
            set
            {
                if (value == country) return;
                    country = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }
        public string Awards
        {
            get { return awards; }
            set
            {
                if (value == awards) return;
                    awards = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Awards"));
            }
        }
        public string Poster
        {
            get { return poster; }
            set
            {
                if (value == poster) return;
                    poster = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Poster"));
            }
        }
        public IEnumerable<Rating> Ratings
        {
            get { return ratings; }
            set
            {
                if (value == ratings) return;

                ratings = value;
                StringBuilder sb = new StringBuilder();
                foreach (Rating r in Ratings)
                {
                    sb.AppendLine(r.ToString());
                    RatingsShow = sb.ToString();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Ratings"));
            }
        }
        public string Metascore
        {
            get { return metascore; }
            set
            {
                if (value == metascore) return;
                    metascore = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Metascore"));
            }
        }
        public string ImdbRating
        {
            get { return imdbRating; }
            set
            {
                if (value == imdbRating) return;
                imdbRating = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ImdbRating"));
            }
        }
        public string ImdbVotes
        {
            get { return imdbVotes; }
            set
            {
                if (value == imdbVotes) return;
                imdbVotes = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ImdbVotes"));
            }
        }
        public string ImdbID
        {
            get { return imdbID; }
            set
            {
                if (value == imdbID) return;
                imdbID = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ImdbID"));
            }
        }
        public string Type
        {
            get { return type; }
            set
            {
                if (value == type) return;
                type = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
            }
        }
        public string DVD
        {
            get { return dVD; }
            set
            {
                if (value == dVD) return;
                dVD = value;

                OnPropertyChanged(new PropertyChangedEventArgs("DVD"));
            }
        }
        public string BoxOffice
        {
            get { return boxOffice; }
            set
            {
                if (value == boxOffice) return;
                boxOffice = value;

                OnPropertyChanged(new PropertyChangedEventArgs("BoxOffice"));
            }
        }
        public string Production
        {
            get { return production; }
            set
            {
                if (value == production) return;
                production = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Production"));
            }
        }
        public string Website
        {
            get { return website; }
            set
            {
                if (value == website) return;
                website = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Website"));
            }
        }
        public bool Response
        {
            get { return response; }
            set
            {
                if (value == response) return;
                response = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Response"));
            }
        }
        public string RatingsShow { get; set; }
        #endregion

        #region Events
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        public override string ToString()
        {
            return Title + " (" + year + ")";
        }

        public async static Task<Movie> GetMovieByID(string id)
        {
            RestClient client = new RestClient("http://www.omdbapi.com/?apikey=38e4685d");

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("i", id);
            try
            {
                IRestResponse<Movie> response = await client.ExecuteGetTaskAsync<Movie>(request);
                Movie result = response.Data;

                return result;
            }
            catch (Exception xcp)
            {
                throw new Exception(xcp.Message, xcp.InnerException);
            }
        }

        public async static Task<Movie> GetMovieByTitle(string title)
        {
            RestClient client = new RestClient("http://www.omdbapi.com/?apikey=38e4685d");

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("t", title);
            try
            {
                IRestResponse<Movie> response = await client.ExecuteGetTaskAsync<Movie>(request);
                Movie result = response.Data;

                return result;
            }
            catch (Exception xcp)
            {
                throw new Exception(xcp.Message, xcp.InnerException);
            }
        }
        #endregion
    }
}
