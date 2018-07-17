using OMDbAPIClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;

namespace OMDbAPIClient.ViewModel
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Mediator mediator;
        private ICollectionView favoriteMovies;
        private Movie selectedMovie;
        private ICommand removeFavoriteCommand;
        private ICommand openDetailsCommand;
        private bool isClosed;
        private IMessageBoxService messageService;
        #endregion

        #region Properties
        public ICollectionView FavoriteMovies
        {
            get { return favoriteMovies; }
            set
            {
                if (favoriteMovies == value) return;
                favoriteMovies = value;

                OnPropertyChanged(new PropertyChangedEventArgs("FavoriteMovies"));
            }
        }
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                if (selectedMovie == value) return;
                selectedMovie = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMovie"));
            }
        }
        public ICommand RemoveFavoriteCommand
        {
            get { return removeFavoriteCommand; }
            set
            {
                if (removeFavoriteCommand == value) return;
                removeFavoriteCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("RemoveFavoriteCommand"));
            }
        }
        public ICommand OpenDetailsCommand
        {
            get { return openDetailsCommand; }
            set
            {
                if (openDetailsCommand == value) return;
                openDetailsCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("OpenDetailsCommand"));
            }
        }
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (isClosed == value)
                {
                    return;
                }
                isClosed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsClosed"));
            }
        }

        #endregion

        #region Constructors
        public FavoritesViewModel(Mediator _mediator, IMessageBoxService _messageService)
        {
            messageService = _messageService;
            IsClosed = true;
            mediator = _mediator;
            mediator.Register("Favorited", Favorite);
            GetMovies();
            RemoveFavoriteCommand = new RelayCommand(RemoveFavoriteExecute, CanRemoveFavorite);
            OpenDetailsCommand = new RelayCommand(OpenDetailsExecute, CanOpenDetails);
            mediator.Register("Closing2", DetailsClosed);
            
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion

        #region Methods
        //Reads favorites from Json file and return an ID list
        private async Task<List<string>> ReadFavorites()
        {
            List<string> ids = new List<string>();
            try
            {
                if (File.Exists("Favorites.txt"))
                {
                    string jsonString = File.ReadAllText("Favorites.txt");
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        ids = await LocalStorageDAL.ReadFavorite();
                        return ids;
                    }
                    return ids;
                }
                return ids;
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        //Adds new favorite
        private async void Favorite(object input)
        {
            if (input != null)
            {
                string favoritedMovie = (input as Movie).ImdbID;
                List<string> idList = await ReadFavorites();
                if (!idList.Contains(favoritedMovie))
                {
                    idList.Add(favoritedMovie);
                }
                else {
                    messageService.ShowNotification("Selected movie is already in favorites");
                }
                LocalStorageDAL.WriteFavorite(idList);
                GetMovies();
            }
        }

        //Fiills up the listbox with items
        private async void GetMovies()
        {
            List<string> movieIds = await ReadFavorites();
            List<Movie> moviesList = new List<Movie>();
            foreach (string id in movieIds)
            {
                Movie movie = await Movie.GetMovieByID(id);
                moviesList.Add(movie);
            }

            FavoriteMovies = new ListCollectionView(moviesList);
        }

        //Removes movie from favorite list
        public async void RemoveFavoriteExecute(object selectedMovie)
        {
            if (selectedMovie != null)
            {
                Movie movie = selectedMovie as Movie;
                List<string> idList = await ReadFavorites();
                idList.Remove((selectedMovie as Movie).ImdbID);
                LocalStorageDAL.WriteFavorite(idList);
                GetMovies();
            }
        }

        //Disables the context menu item if none is selected
        public bool CanRemoveFavorite(object selectedMovie)
        {
            if (selectedMovie != null)
            {
                return true;
            }
            return false;
        }

        public void OpenDetailsExecute(object selectedMovie)
        {
            mediator.Notify("ShowFavoriteDetails", selectedMovie);
        }

        public bool CanOpenDetails(object selectedMovie)
        {
            if (selectedMovie != null)
            {
                return true;
            }
            return false;
        }

        private void DetailsClosed(object isClosed)
        {
            if (isClosed != null)
                IsClosed = (bool)isClosed;
        }
        #endregion
    }
}
