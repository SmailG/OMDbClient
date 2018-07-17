using Microsoft.Win32;
using OMDbAPIClient.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace OMDbAPIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            MoviesViewModel moviesModel = new MoviesViewModel(Mediator.Instance, new MessageBoxService());
            FavoritesViewModel favoritesModel = new FavoritesViewModel(Mediator.Instance, new MessageBoxService());
            WatchedViewModel watchedModel = new WatchedViewModel(Mediator.Instance, new MessageBoxService());

            MoviesTab.DataContext = moviesModel;
            FavoritesTab.DataContext = favoritesModel;
            WatchedTab.DataContext = watchedModel;
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.SelectedIndex > -1)
            {
                MoviesViewModel dataContext = (MoviesViewModel)MoviesTab.DataContext;
                DetailsViewModel detailsModel = new DetailsViewModel(Mediator.Instance, dataContext.SelectedMovie);
                MovieDetailsWindow detailsWindow = new MovieDetailsWindow();
                detailsWindow.DataContext = detailsModel;
                detailsWindow.Show();
            }
            else
            {
                MessageBox.Show("You need to choose a movie first", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void MenuItemWithRadioButtons_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                RadioButton rb = mi.Icon as RadioButton;
                if (rb != null)
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void TextBlock_MouseUp_FromFavorites(object sender, MouseButtonEventArgs e)
        {
            

            if(((FavoritesViewModel)FavoritesTab.DataContext).IsClosed == true)
            {
                FavoritesViewModel dataContext = (FavoritesViewModel)FavoritesTab.DataContext;
                DetailsViewModel detailsModel = new DetailsViewModel(Mediator.Instance, dataContext.SelectedMovie);
                MovieDetailsWindow detailsWindow = new MovieDetailsWindow();
                detailsWindow.DataContext = detailsModel;
                detailsWindow.Show();
            }
        }

        private void TextBlock_MouseUp_FromWatched(object sender, MouseButtonEventArgs e)
        {


            if (((WatchedViewModel)WatchedTab.DataContext).IsClosed == true)
            {
                WatchedViewModel dataContext = (WatchedViewModel)WatchedTab.DataContext;
                DetailsViewModel detailsModel = new DetailsViewModel(Mediator.Instance, dataContext.SelectedMovie);
                MovieDetailsWindow detailsWindow = new MovieDetailsWindow();
                detailsWindow.DataContext = detailsModel;
                detailsWindow.Show();
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    ImportTextBox.Text = fbd.SelectedPath;
                }
            }


        }
    }
}
