using OMDbAPIClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OMDbAPIClient.ViewModel
{
    public class WatchedViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Mediator mediator;
        private ICommand importCommand;
        private string currentPath;
        private ICollectionView movieCollection;
        private Movie selectedMovie;
        private ICommand downloadPosterCommand;
        private ICommand openDetailsCommand;
        private bool isClosed;
        private string progressVisibility;
        private bool progressValue;
        private IMessageBoxService messageService;
        #endregion

        #region Properties
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
        public ICommand ImportCommand
        {
            get { return importCommand; }
            set
            {
                if (importCommand == value) return;
                importCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ImportCommand"));
            }
        }
        public ICommand DownloadPosterCommand
        {
            get { return downloadPosterCommand; }
            set
            {
                if (downloadPosterCommand == value) return;
                downloadPosterCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("DownloadPosterCommand"));
            }
        }
        public string CurrentPath
        {
            get { return currentPath; }
            set
            {
                if (currentPath == value) return;
                currentPath = value;

                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPath"));
            }
        }
        public ICollectionView MovieCollection
        {
            get { return movieCollection; }
            set
            {
                if (movieCollection == value) return;
                movieCollection = value;

                OnPropertyChanged(new PropertyChangedEventArgs("MovieCollection"));
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
        public string ProgressVisibility
        {
            get { return progressVisibility; }
            set
            {
                if (progressVisibility == value) return;
                progressVisibility = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ProgressVisibility"));
            }
        }
        public bool ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (progressValue == value) return;
                progressValue = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ProgressValue"));
            }
        }
        public List<string> BadFolders { get; set; }
        #endregion

        #region Constructor
        public WatchedViewModel(Mediator _mediator, IMessageBoxService _messageSerivce)
        {
            messageService = _messageSerivce;
            ProgressVisibility = "Hidden";
            mediator = _mediator;
            IsClosed = true;
            OpenDetailsCommand = new RelayCommand(OpenDetailsExecute, CanOpenDetails);
            ImportCommand = new RelayCommand(ImportWithProgress, CanImport);
            DownloadPosterCommand = new RelayCommand(DownloadPosterExecute, CanDownload);
            CurrentPath = LocalStorageDAL.GetLastPath();
            mediator.Register("Closing3", DetailsClosed);
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
        //Implemented to support progress bar
        public async void ImportWithProgress(object path)
        {
            ProgressVisibility = "Visible";
            ProgressValue = true;

            await ImportExecute(path);

            ProgressVisibility = "Hidden";
            ProgressValue = false;
        }


        public async Task ImportExecute(object path)
        {
            //Sets the progress bar
            BadFolders = new List<string>();
            IEnumerable<string> folders = LocalStorageDAL.ReadMoviesFromFolder(path.ToString());
            await LocalStorageDAL.SaveLastPath(path.ToString());
            List<Movie> movieList = new List<Movie>();
            StringBuilder sb = new StringBuilder();
            foreach (string folder in folders)
            {
                sb = new StringBuilder();
                //Checks movie pattern
                Match match = Regex.Match(folder, @".+\s?\([0-9]+\)");
                if (match.Success)
                {
                    IEnumerable<char> charArray = folder.TakeWhile(c => c != '(');
                    foreach (char c in charArray)
                    {
                        sb.Append(c);
                    }
                    
                    Movie movie = await Movie.GetMovieByTitle(sb.ToString().Trim());

                    //Added to demonstrate loading as the loading time is mostly too short
                    await Task.Delay(750);
                    if (movie.ImdbID != null)
                    {
                        movieList.Add(movie);
                    }
                }
                else {
                    BadFolders.Add(folder);
                }
            }
            if (BadFolders.Count > 0)
            {
                sb = new StringBuilder();
                foreach (string folder in BadFolders)
                {
                    sb.AppendLine(folder);
                }
                messageService.ShowNotification("Following movies could not be parsed\n" + sb.ToString());
            }
            
            MovieCollection = new ListCollectionView(movieList);
            
           
            
        }
        public bool CanImport(object path)
        {
            if (!string.IsNullOrWhiteSpace(path.ToString()))
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

        public void DownloadPosterExecute(object selectedMovie)
        {
            Movie movie = selectedMovie as Movie;
            if (!string.IsNullOrWhiteSpace((selectedMovie as Movie).Poster))
            {
                Uri posterUrl = new Uri(movie.Poster);
                // removes : / \ * ? " < > | because you can define these cahrs in the folder name
                string friendlyMovieTitle = movie.Title;
                string[] forRemove = {":", "/", "\\", "*", "?", "\"", "<", ">", "|"};
                foreach (string c in forRemove)
                {
                    if (friendlyMovieTitle.Contains(c))
                    {
                       friendlyMovieTitle = friendlyMovieTitle.Replace(c, string.Empty);
                    }
                }
                
                string file = CurrentPath + "\\" + friendlyMovieTitle + " (" + movie.Year + ")" + "\\" + Path.GetFileName( movie.Title + Path.GetExtension(movie.Poster));
                WebClient webClient = new WebClient();
                webClient.DownloadFileAsync(posterUrl, file);
            }
        }
        public bool CanDownload(object selectedMovie)
        {
            if (selectedMovie != null && !string.IsNullOrWhiteSpace((selectedMovie as Movie).Poster))
            {
                return true;
            }
            return false;
        }

        public void OpenDetailsExecute(object selectedMovie)
        {
            mediator.Notify("ShowWatchedDetails", selectedMovie);
        }
        public bool CanOpenDetails(object selectedMovie)
        {
            if (selectedMovie != null)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
