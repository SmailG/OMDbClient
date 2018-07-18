using OMDbAPIClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace OMDbAPIClient.ViewModel
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ICommand searchCommand;
        private ICommand pageBackCommand;
        private ICommand pageForwardCommand;
        private ICommand sortCommand;
        private ICommand favoriteCommand;
        private SearchResult searchResult;
        private ICollectionView moviesView;
        private Movie selectedMovie;
        private string pageStatus;
        private string searchParameter;
        private int currentPage;
        private string sortMethod;
        private Mediator mediator;
        private bool isClosed;
        private IMessageBoxService messageService;
        #endregion

        #region Properties
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (currentPage == value) return;
                currentPage = value;

                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));

            }
        }
        public string SearchParameter
        {
            get { return searchParameter; }
            set
            {
                if (searchParameter == value) return;
                searchParameter = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SearchParameter"));

            }
        }
        public string SortMethod
        {
            get { return sortMethod; }
            set
            {
                if (sortMethod == value) return;
                sortMethod = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SortMethod"));

            }
        }
        public ICommand PageBackCommand
        {
            get { return pageBackCommand; }
            set
            {
                if (pageBackCommand == value) return;
                pageBackCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("PageBackCommand"));

            }
        }
        public ICommand PageForwardCommand
        {
            get { return pageForwardCommand; }
            set
            {
                if (pageForwardCommand == value) return;
                pageForwardCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("PageForwardCommand"));

            }
        }
        public ICommand SearchCommand
        {
            get { return searchCommand; }
            set
            {
                if (searchCommand == value) return;
                searchCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SearchCommand"));

            }
        }
        public ICommand SortCommand
        {
            get { return sortCommand; }
            set
            {
                if (sortCommand == value) return;
                sortCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SortCommand"));

            }
        }
        public ICommand FavoriteCommand
        {
            get { return favoriteCommand; }
            set
            {
                if (favoriteCommand == value) return;
                favoriteCommand = value;

                OnPropertyChanged(new PropertyChangedEventArgs("FavoriteCommand"));
            }
        }
        public string PageStatus
        {
            get { return pageStatus; }
            set
            {
                if (pageStatus == value) return;
                pageStatus = value;

                OnPropertyChanged(new PropertyChangedEventArgs("PageStatus"));

            }
        }
        public SearchResult SearchResult
        {
            get { return searchResult; }
            set
            {
                if (searchResult == value) return;
                searchResult = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SearchResult"));

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
        public ICollectionView MoviesView
        {
            get { return moviesView; }
            set
            {
                if (moviesView == value)
                {
                    return;
                }
                moviesView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MoviesView"));
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
        public MoviesViewModel(Mediator _mediator, IMessageBoxService _messageService)
        {
            messageService = _messageService;
            IsClosed = true;
            mediator = _mediator;
            SortMethod = "sortbyyear";
            PageBackCommand = new RelayCommand(PageBackExecute, CanPageBack);
            PageForwardCommand = new RelayCommand(PageForwardExecute, CanPageForward);
            SearchCommand = new RelayCommand(SearchExecute, CanSearch);
            SortCommand = new RelayCommand(SortExecute, CanSort);
            FavoriteCommand = new RelayCommand(FavoriteExecute, CanFavorite);
            SearchParameter = LocalStorageDAL.GetLastSearch();
            CurrentPage = 1;
            SearchExecute(SearchParameter);
            PropertyChanged += MoviesViewModel_PropertyChanged;
            mediator.Register("Closing", DetailsClosed);
        }

        #endregion

        #region Events
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MoviesViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedMovie"))
            {
                mediator.Notify("MovieChanged", SelectedMovie);
            }
        }

        #endregion

        #region Methods
        //Searching
        public void SearchExecute(object searchParameter)
        {
            CurrentPage = 1;
            GetResults(searchParameter as string);
        }
        private async void GetResults(string searchParameter)
        {
            if (!string.IsNullOrWhiteSpace(SearchParameter as string))
            {
                try
                {
                    SearchResult = await SearchResult.SearchByTitleAsync(searchParameter.ToString(), CurrentPage);
                    MoviesView = new ListCollectionView(SearchResult.Search.ToList());
                    Sorting(SortMethod);
                    await LocalStorageDAL.SaveLastSearch(searchParameter.ToString());
                    SetPageStaus(PageStatus, SearchResult.TotalResults);
                }
                catch (Exception xcp)
                {
                    if (SearchResult != null)
                    {
                        bool response = SearchResult.Response;
                        string error = SearchResult.Error;
                        messageService.ShowNotification(error);
                    }
                    else {
                        messageService.ShowNotification(xcp.Message);
                    }
                }
            }
        }
        public bool CanSearch(object searchParameter)
        {
            if (!string.IsNullOrWhiteSpace(searchParameter.ToString()))
            {
                return true;
            }
            else {
                return false;
            }
        }

        //Paging
        //Back
        public void PageBackExecute(object o)
        {
            --CurrentPage;
            GetResults(SearchParameter);
        }
        public bool CanPageBack(object o)
        {
            if (SearchResult != null && CurrentPage > 1)
            {
                return true;
            }
            else {
                return false;
            }
        }

        //Forward
        public void PageForwardExecute(object o)
        {
            CurrentPage++;
            GetResults(SearchParameter);
        }
        public bool CanPageForward(object o)
        {
            if (SearchResult != null && CurrentPage < SearchResult.TotalResults / 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetPageStaus(string page, int totalresults)
        {
            PageStatus = CurrentPage + "/" + SearchResult.TotalResults / 10;
        }

        //Sorting
        private void Sorting(string sortParameter)
        {
            if (sortParameter.Equals("sortbyyear"))
            {
                MoviesView.GroupDescriptions.Remove(new PropertyGroupDescription("Type"));
                MoviesView.GroupDescriptions.Add(new PropertyGroupDescription("Year"));
                MoviesView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            }

            else if (sortParameter.Equals("sortbytype"))
            {
                MoviesView.GroupDescriptions.Remove(new PropertyGroupDescription("Year"));
                MoviesView.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                MoviesView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            }
        }

        public void SortExecute(object sortParameter)
        {
            SortMethod = sortParameter.Equals("sortbyyear") ? "sortbyyear" : "sortbytype";
            Sorting(SortMethod);
            GetResults(SearchParameter);
        }
        public bool CanSort(object sortParameter)
        {
            return true;
        }

        //Favorites
        public void FavoriteExecute(object selectedMovie)
        {
            mediator.Notify("Favorited", SelectedMovie);
        }
        public bool CanFavorite(object selectedMovie)
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
