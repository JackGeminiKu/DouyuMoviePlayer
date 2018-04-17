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
            string connectString = @"Data Source=10.0.0.2;Initial Catalog=Douyu2;User ID=sa;Password=Jack52664638";
            _conn = new SqlConnection(connectString);
            _conn.Open();
        }

        public static void GetTopMovie(int roomId, out string movieName, out string movieFile)
        {
            var movieInfo = _conn.Query(string.Format(
                "select top(1) movie_name, movie_file from movie_score where room_id = {0} order by movie_score desc",
                roomId));

            movieName = movieInfo.First().movie_name;
            movieFile = movieInfo.First().movie_file;
        }

        public static string GetAdvertMovie(int roomId)
        {
            return ExecuteScalar<string>(
                "select movie_file from movie_score where room_id = {0} and movie_name = '{1}'",
                roomId, roomId
            );
        }

        public static void ClearTopMovie(int roomId, string movieFile)
        {
            string command = string.Format(
                "update movie_score set movie_score = 0 where room_id = {0} and movie_file = '{1}'",
                roomId, movieFile
            );
            if (ExecuteNonQuery(command) != 1)
                LogService.GetLogger("Error").ErrorFormatted("[SQL] 清除电影积分失败: {0}", command);
        }

        public static void ImportNewMovie(int roomId, string movieFile)
        {
            string movieName = Path.GetFileNameWithoutExtension(movieFile);

            int row = ExecuteScalar<int>(
                "select count(*) from movie_score where room_id = {0} and movie_name = '{1}'",
                roomId, movieName
            );
            if (row != 0) {
                return;
            }

            row = ExecuteNonQuery(
                "insert into movie_score(room_id, movie_name, movie_score, movie_file) values({0}, '{1}', {2}, '{3}')",
                roomId, movieName, 0, movieFile
            );
            if (row != 1)
                MessageBox.Show("导入电影失败: " + movieFile, "导入电影");
        }

        public static bool HasAdvertMovie(int roomId)
        {

            int count = ExecuteScalar<int>(
                "select count(*) from movie_score where room_id = {0} and movie_name = '{1}'",
                roomId, roomId
            );
            return count == 1;

        }

        public static bool HasMovie(int roomId)
        {
            int count = ExecuteScalar<int>(
                "select count(*) from movie_score where room_id = {0} and movie_name != '{1}'",
                roomId, roomId
            );
            return count > 0;
        }

        public static void SetCurrentMovie(int roomId, string movieName)
        {
            var count = _conn.Execute(
                "update config set value = @MovieName where name = 'current movie' and room_id = @RoomId",
                new { MovieName = movieName, RoomId = roomId });
            if (count != 1)
                LogService.GetLogger("error").Error("更新当前电影失败!");
        }


        public static void SaveAliasName(int roomId, string movieName, string movieAlias)
        {
            var aliasInfo = _conn.Query(
                "select * from movie_alias where room_id = @RoomId and movie_alias = @MovieAlias",
                new { RoomId = roomId, MovieAlias = movieAlias });

            if (aliasInfo.Count() == 1) {
                MessageBox.Show("已经存在别名" + movieAlias, "添加别名");
                return;
            }

            _conn.Execute("insert into movie_alias values(@Roomid, @MovieName, @MovieAlias)",
                new { RoomId = roomId, MovieName = movieName, MovieAlias = movieAlias });
            MessageBox.Show("添加成功!", "添加别名");
        }
        #region "SQL functions"

        static SqlCommand CreateCommand(string sql)
        {
            return new SqlCommand(sql, _conn as SqlConnection);
        }

        static int ExecuteNonQuery(string sql)
        {
            LogService.Default.Debug("[SQL] " + sql);
            return CreateCommand(sql).ExecuteNonQuery();
        }

        static int ExecuteNonQuery(string sql, params object[] args)
        {
            return ExecuteNonQuery(string.Format(sql, args));
        }

        static T ExecuteScalar<T>(string sql)
        {
            LogService.Default.Debug("[SQL] " + sql);
            return (T)CreateCommand(sql).ExecuteScalar();
        }

        static T ExecuteScalar<T>(string sql, params object[] args)
        {
            return ExecuteScalar<T>(string.Format(sql, args));
        }

        static IDataReader ExecuteReader(string sql)
        {
            LogService.Default.Debug("[SQL] " + sql);
            return CreateCommand(sql).ExecuteReader();
        }

        static IDataReader ExecuteReader(string sql, params object[] args)
        {
            return ExecuteReader(string.Format(sql, args));
        }

        #endregion
    }
}
