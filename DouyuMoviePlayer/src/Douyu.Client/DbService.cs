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
        static IDbConnection _conn;

        static DbService()
        {
            _conn = new SqlConnection(Properties.Settings.Default.ConnectionString);
            _conn.Open();
        }

        public static void GetTopMovie(int roomId, out string movieName, out string movieFile)
        {
            var movieInfo = _conn.Query(
                "select top(1) MovieName, MovieFile " +
                "from MovieScore " +
                "where RoomId = @RoomId " +
                "order by MovieScore desc",
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
                "update MovieScore " +
                "set MovieScore = 0 " +
                "where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName }
            );
        }

        public static void ImportMovie(int roomId, string movieFile)
        {
            var movieName = Path.GetFileNameWithoutExtension(movieFile);

            var count = _conn.ExecuteScalar<int>(
                "select count(*) from MovieScore " +
                "where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName }
            );
            if (count == 0) {
                _conn.Execute(
                    "insert into " +
                    "MovieScore(RoomId, MovieName, MovieFile, MovieScore) " +
                    "values(@RoomId, @MovieName, @MovieFile, 0)",
                    new { RoomId = roomId, MovieName = movieName, MovieFile = movieFile }
                );
                return;
            }

            _conn.Execute(
                "update MovieScore " +
                "set MovieFile = @MovieFile " +
                "where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName, MovieFile = movieFile }
            );
        }

        public static void ImportAdvert(string advertMovie)
        {
            var movieName = Path.GetFileNameWithoutExtension(advertMovie);
            var roomId = 0;
            if (!int.TryParse(movieName, out roomId)) {
                MessageBox.Show("广告文件必须为数字(房间号)!", "导入广告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var count = _conn.ExecuteScalar<int>(
                "select count(*) from AdvertMovie where RoomId = @RoomId",
                new { RoomId = roomId }
            );
            if (count == 0) {
                _conn.Execute(
                    "insert into " +
                    "AdvertMovie(RoomId, AdvertMovie) " +
                    "values(@RoomId,  @AdvertMovie)",
                    new { RoomId = roomId, AdvertMovie = advertMovie }
                );
                return;
            }

            _conn.Execute(
                "update AdvertMovie " +
                "set AdvertMovie = @AdvertMovie " +
                "where RoomId = @RoomId",
                new { RoomId = roomId, AdvertMovie = advertMovie }
            );
        }

        public static bool HasAdvertMovie(int roomId)
        {
            var count = _conn.ExecuteScalar<int>(
                "select count(*) from MovieScore where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = roomId }
            );
            return count == 1;
        }

        public static bool HasMovie(int roomId)
        {
            var count = _conn.ExecuteScalar<int>(
                "select count(*) from MovieScore where RoomId = @RoomId",
                new { RoomId = roomId }
            );
            return count > 0;
        }

        public static void SetCurrentMovie(int roomId, string movieName)
        {
            const string ITEM_CURRENT_MOVIE = "CurrentMovie";

            var count = _conn.ExecuteScalar<int>("select count(*) from RoomInfo where RoomId = @RoomId and Name = @Name",
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
                "Update MovieScore " +
                "set LastPlaytime = @LastPlayTime " +
                "where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName, LastPlayTime = DateTime.Now });
        }

        public static void SaveAliasName(int roomId, string movieName, string movieAlias)
        {
            var count = _conn.ExecuteScalar<int>(
                "select count(*) from MovieAlias where RoomId = @RoomId and MovieAlias = @MovieAlias",
                new { RoomId = roomId, MovieAlias = movieAlias });

            if (count == 1) {
                return;
            }

            _conn.Execute(
                "insert into MovieAlias values(@Roomid, @MovieName, @MovieAlias)",
                new { RoomId = roomId, MovieName = movieName, MovieAlias = movieAlias }
            );
        }
    }
}
