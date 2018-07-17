using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OMDbAPIClient.Model
{
    public class SearchResult : INotifyPropertyChanged
    {
        #region Fields
        private int totalResults;
        private IEnumerable<Movie> search;
        private bool response;
        private string error;
        #endregion

        #region Properties
        public int TotalResults
        {
            get { return totalResults; }
            set {
                if (totalResults == value) return;
                totalResults = value;

                OnPropertyChanged(new PropertyChangedEventArgs("TotalResults"));

            }
        }
        public IEnumerable<Movie> Search
        {
            get { return search; }
            set {
                if (search == value) return;
                search = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Search"));
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
        public string Error
        {
            get { return error; }
            set
            {
                if (value == error) return;
                error = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Error"));
            }
        }
        #endregion

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods

        public static SearchResult SearchByTitle(string searchInput, int moviesPerPage = 1)
        {
            RestClient client = new RestClient("http://www.omdbapi.com/?apikey=38e4685d");

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("s",searchInput);
            request.AddParameter("page", moviesPerPage);
            try
            {
                IRestResponse<SearchResult> response = client.Execute<SearchResult>(request);
                SearchResult result = response.Data;

                return result;
            }
            catch (Exception xcp)
            {
                throw new Exception(xcp.Message, xcp.InnerException);
            }

            
        }

        public async static Task<SearchResult> SearchByTitleAsync(string searchInput, int moviesPerPage = 1)
        {
            RestClient client = new RestClient("http://www.omdbapi.com/?apikey=38e4685d");

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("s", searchInput);
            request.AddParameter("page", moviesPerPage);
            try
            {
                IRestResponse<SearchResult> response = await client.ExecuteGetTaskAsync<SearchResult>(request);
                SearchResult result = response.Data;

                return result;
            }
            catch (Exception xcp)
            {
                throw new Exception(xcp.Message,xcp.InnerException);
            }
        }




        #endregion
    }
}
