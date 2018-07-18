using OMDbAPIClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDbAPIClient.ViewModel 
{
    public class DetailsViewModel : INotifyPropertyChanged
    {

        #region Fields
        private Movie selectedMovie;
        private Mediator mediator;
        private bool isClosed;
        #endregion

        #region Properties
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                selectedMovie = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMovie"));
            }
        }
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (isClosed == value) return;
                isClosed = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsClosed"));
            }
        }
        #endregion

        #region Constructors
        public DetailsViewModel(Mediator _mediator, Movie selectedMovie)
        {
            mediator = _mediator;
            SetMovie(selectedMovie);
            mediator.Register("MovieChanged", SetMovie);
            mediator.Register("ShowFavoriteDetails", ShowLoadedMovie);
            mediator.Register("ShowWatchedDetails", ShowLoadedMovie);
            this.PropertyChanged += DetailsViewModel_PropertyChanged;
            IsClosed = false;
            mediator.Notify("Closing", IsClosed);
            mediator.Notify("Closing2", IsClosed);
            mediator.Notify("Closing3", IsClosed);
        }
        #endregion

        #region Methods
        //Show loaded movie in memory
        private void ShowLoadedMovie(object obj)
        {
            if (obj != null)
            {
                SelectedMovie = obj as Movie;
            }
        }

        private async void SetMovie(object movie)
        {
            SelectedMovie = await Movie.GetMovieByID((movie as Movie).ImdbID);
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void DetailsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsClosed"))
            {
                mediator.Notify("Closing", IsClosed);
                mediator.Notify("Closing2", IsClosed);
                mediator.Notify("Closing3", IsClosed);
            }
        }
        #endregion
    }
}
