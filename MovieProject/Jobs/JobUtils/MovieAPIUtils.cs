using MovieProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MovieProject.Jobs.JobUtils
{
    public class MovieAPIUtils
    {
        public static string request_token = "";
        public static string MovieDBBaseUrl = "";
        
        public MovieAPIUtils()
        {
            MovieDBBaseUrl = (string)ConfigurationManager.AppSettings["MovieDBBaseUrl"];
        }
    
        public void generateToken()
        {
            string APIKey = (String)ConfigurationManager.AppSettings["APIKey"];
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(MovieDBBaseUrl);
            var response = httpClient.GetAsync("/3/authentication/token/new?api_key=" + APIKey).Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(contents);
            request_token = (string)obj["request_token"];
        }

        public string getIMDBMovieID(string MovieTitle)
        {
            string OMDBAPIKey = (String)ConfigurationManager.AppSettings["OMDBAPIKey"];
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.omdbapi.com");
            var title = MovieTitle.Replace(" ", "%20");
            var response = httpClient.GetAsync("/?t=" + title + "&apikey=" + OMDBAPIKey).Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(contents);
            String movieID = (string)obj["imdbID"];
            return movieID;
        }

        public string getMovieDBIDFromIMDBID(string imdbID)
        {
            string APIKey = (String)ConfigurationManager.AppSettings["APIKey"];
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(MovieDBBaseUrl);
            var response = httpClient.GetAsync("/3/find/" + imdbID + "?api_key=" + APIKey + "&external_source=imdb_id").Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(contents);
            if(obj["movie_results"].Count() > 0)
            {
                string movieDBID = (string)obj["movie_results"][0]["id"];
                return movieDBID;
            }
            return null;
        }

        public List<Movie> getMovieSuggestions(string movieDBId)
        {
            string APIKey = (String)ConfigurationManager.AppSettings["APIKey"];
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(MovieDBBaseUrl);
            var response = httpClient.GetAsync("/3/movie/" + movieDBId + "/recommendations?api_key=" + APIKey + "&external_source=imdb_id").Result;
            var contents = response.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(contents);
            var results = obj["results"];
            if(results == null)
            {
                return null;
            }
            int ix = 0;
            List<Movie> movies = new List<Movie>();
            foreach (Object movie in results)
            {
                Movie mv = new Movie();
                mv.MovieTitle = (string)results[ix]["title"];
                ++ix;
                movies.Add(mv);
            }
            return movies;
        }
    }
}