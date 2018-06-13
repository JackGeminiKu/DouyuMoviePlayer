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
                "select top(1) MovieName, MovieFile from MovieScore where RoomId = @RoomId order by MovieScore desc",
                new { RoomId = roomId }
            );

            movieName = movieInfo.First().MovieName;
            movieFile = movieInfo.First().MovieFile;
        }

        public static string GetAdvertMovie(int roomId)
        {
            var result = _conn.ExecuteScalar(
                "select MovieFile from MovieScore where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = roomId }
            );
            return (string)result;
        }

        public static void ClearTopMovie(int roomId, string movieFile)
        {
            _conn.Execute(
                "update MovieScore set MovieScore = 0 where RoomId = @RoomId and MovieFile = @MovieFile",
                new { RoomId = roomId, MovieFile = movieFile }
            );
        }

        public static void ImportMovie(int roomId, string movieFile)
        {
            string movieName = Path.GetFileNameWithoutExtension(movieFile);

            var count = _conn.ExecuteScalar(
                "select count(*) from MovieScore where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = movieName }
            );
            if ((int)count != 0) {
                return;
            }

            _conn.Execute(
                "insert into MovieScore(RoomId, MovieName, MovieScore, MovieFile) values(@RoomId, @MovieName, @MovieScore, @MovieFile)",
                new { RoomId = roomId, MovieName = movieName, MovieScore = 0, MovieFile = movieFile }
            );
        }

        public static bool HasAdvertMovie(int roomId)
        {
            var count = _conn.ExecuteScalar(
                "select count(*) from MovieScore where RoomId = @RoomId and MovieName = @MovieName",
                new { RoomId = roomId, MovieName = roomId }
            );
            return (int)count == 1;
        }

        public static bool HasMovie(int roomId)
        {
            var count = _conn.ExecuteScalar(
                "select count(*) from MovieScore where RoomId = @RoomId and MovieName != @AdvertMovie",
                new { RoomId = roomId, AdvertMovie = roomId }
            );
            return (int)count > 0;
        }

        public static void SetCurrentMovie(int roomId, string movieName)
        {
            _conn.Execute(
                "update Config set value = @MovieName where Name = 'current movie' and RoomId = @RoomId",
                new { MovieName = movieName, RoomId = roomId }
            );
        }

        public static void SaveAliasName(int roomId, string movieName, string movieAlias)
        {
            var count = _conn.ExecuteScalar(
                "select count(*) from MovieAlias where RoomId = @RoomId and MovieAlias = @MovieAlias",
                new { RoomId = roomId, MovieAlias = movieAlias });

            if ((int)count == 1) {
                return;
            }

            _conn.Execute(
                "insert into MovieAlias values(@Roomid, @MovieName, @MovieAlias)",
                new { RoomId = roomId, MovieName = movieName, MovieAlias = movieAlias }
            );
        }
    }
}
