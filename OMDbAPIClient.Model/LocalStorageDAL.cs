using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OMDbAPIClient.Model
{
    public class LocalStorageDAL
    {
        //Last search
        public static async Task SaveLastSearch(string searchParameter)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"Lastsearch.txt"))
                {
                    await sw.WriteAsync(searchParameter);
                }
            }
            catch (Exception)
            {

                return;
            }
            
        }

        public static string GetLastSearch()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Lastsearch.txt"))
                {
                    string lastSearch = reader.ReadToEnd();
                    return lastSearch;
                }
            }
            catch (Exception)
            {

                return "";
            }
        }

        //Favorites
        public static async void WriteFavorite(List<string> idList)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"Favorites.txt"))
                {
                    string jsonString = JsonConvert.SerializeObject(idList);
                    await sw.WriteAsync(jsonString);
                }
            }
            catch (Exception)
            {

                return;
            }
        }

        public async static Task<List<string>> ReadFavorite()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Favorites.txt"))
                {
                    string jsonString = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<List<string>>(jsonString);
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        //Watched movies
        public static IEnumerable<string> ReadMoviesFromFolder(string path)
        {
            IEnumerable<string> movies = Directory.EnumerateDirectories(path).Select(f => Path.GetFileName(f));
            return movies;
        }

        public static async Task SaveLastPath(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"Lastpath.txt"))
                {
                    await sw.WriteAsync(path);
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        public static string GetLastPath()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Lastpath.txt"))
                {
                    string lastPath = reader.ReadToEnd();
                    return lastPath;
                }
            }
            catch (Exception)
            {

                return "";
            }
        }




    }
}
