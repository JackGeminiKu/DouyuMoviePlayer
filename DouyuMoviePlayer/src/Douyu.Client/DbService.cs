using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Jack4net.Log;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Dapper;

namespace Douyu.Client
{
    public static class DbService
    {
        static IDbConnection _conn = new SqlConnection(Properties.Settings.Default.ConnectionString);

        public static void GetTopMovie(int roomId, out string movieName, out string movieFile)
        {
            var movieInfo = _conn.Query(
                "select top(1) Movie.MovieName, Movie.MovieFile " +
                "from RoomMovie inner join Movie on RoomMovie.MovieId = Movie.Id " +
                "where RoomMovie.RoomId = @RoomId " +
                "order by RoomMovie.MovieScore desc",
                new { RoomId = roomId }
            );

            movieName = movieInfo.First().MovieName;
            movieFile = movieInfo.First().MovieFile;
        }

        public static string[] GetAllMovies()
        {
            var movies = new List<string>();
            foreach (var item in _conn.Query("select MovieFile from MovieScore")) {
                movies.Add((string)item.MovieFile);
            }
            return movies.ToArray();
        }

        public static string GetAdvertMovie(int roomId)
        {
            return _conn.ExecuteScalar<string>(
                "select AdvertMovie " +
                "from AdvertMovie " +
                "where RoomId = @RoomId",
                new { RoomId = roomId }
            );
        }

        public static void ClearMovieScore(int roomId, string movieName)
        {
            _conn.Execute(
                "update RoomMovie " +
                "set MovieScore = 0 " +
                "where RoomId = @RoomId and MovieId = @MovieId",
                new { RoomId = roomId, MovieId = GetMovieId(roomId, movieName) }
            );
        }

        static int GetMovieId(int roomId, string movieName)
        {
            var movieId = _conn.ExecuteScalar<int>(
                "select Movie.Id from RoomMovie " +
                "inner join Movie on RoomMovie.MovieId = Movie.Id " +
                "where RoomMovie.RoomId = @RoomId and Movie.MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName });
            return movieId;
        }

        public static void SetCurrentMovie(int roomId, string movieName)
        {
            const string ITEM_CURRENT_MOVIE = "CurrentMovie";

            var count = _conn.ExecuteScalar<int>(
                "select count(*) from RoomInfo where RoomId = @RoomId and Name = @Name",
                new { RoomId = roomId, Name = ITEM_CURRENT_MOVIE }
            );

            if (count == 1) {
                _conn.Execute(
                    "update RoomInfo set value = @MovieName where RoomId = @RoomId and Name = @Name",
                    new { RoomId = roomId, MovieName = movieName, Name = ITEM_CURRENT_MOVIE }
                );
            } else {
                _conn.Execute(
                    "insert into RoomInfo(RoomId, Name, Value) Values(@RoomId, @Name, @Value)",
                    new { RoomId = roomId, Name = ITEM_CURRENT_MOVIE, Value = movieName }
                );
            }
        }

        public static void UpdateMoviePlaytime(int roomId, string movieName)
        {
            _conn.Execute(
                "Update RoomMovie " +
                "set LastPlaytime = GetDate() " +
                "where RoomId = @RoomId and MovieId = @MovieId",
                new { RoomId = roomId, MovieId = GetMovieId(roomId, movieName) });
        }
    }
}
