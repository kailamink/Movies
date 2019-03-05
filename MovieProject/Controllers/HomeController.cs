using MovieProject.Jobs.JobUtils;
using MovieProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncompleteForm()
        {
            return View();
        }
        public ActionResult MovieNotFound()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindRelatedMovies(Movie movie)
        {
            if(movie.MovieTitle == null)
            {
                return RedirectToAction("IncompleteForm");
            }
            var imdbMovieID = getIMDBMovieID(movie);
            if (imdbMovieID == null)
            {
                return RedirectToAction("MovieNotFound");
            }
            else
            {
                string movieDBID = getMovieDBIDFromIMDBID(imdbMovieID);
                if(movieDBID == null)
                {
                    return RedirectToAction("MovieNotFound");
                }
                return RedirectToAction("MovieSuggestions", new { movieDBID });
            }

        }

        public ActionResult MovieSuggestions(string movieDBID)
        {
            List<Movie> movies = getMovieRecommendations(movieDBID);
            if(movies == null)
            {
                return RedirectToAction("MovieNotFound");
            }
            return View(movies);
        }


        public List<Movie> getMovieRecommendations(string movieDBID)
        {
            MovieAPIUtils movieAPIUtils = new MovieAPIUtils();
            var movies = movieAPIUtils.getMovieSuggestions(movieDBID);
            return movies;
        }
        public string getMovieDBIDFromIMDBID(string imdbID)
        {
            MovieAPIUtils movieAPIUtils = new MovieAPIUtils();
            var movieDBID = movieAPIUtils.getMovieDBIDFromIMDBID(imdbID);
            return movieDBID;
        }
        public String getIMDBMovieID(Movie movie)
        {
            MovieAPIUtils movieAPIUtils = new MovieAPIUtils();
            var movieID = movieAPIUtils.getIMDBMovieID(movie.MovieTitle);
            return movieID;
        }
    }
}