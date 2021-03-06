﻿using Npgsql;
using SUP_G6.DataTypes;
using SUP_G6.Interface;
using SUP_G6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SUP_G6.Other
{
    public static class DataBaseLogic
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["dbServer"].ConnectionString;

        #region CREATE
        #region CREATE PLAYER

        public static int AddPlayer(IPlayer player)
        {
            string stmt = "INSERT INTO player(name) values (@name) returning player_id;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("name", player.Name);
                    command.Parameters.AddWithValue("player_id", player.Id);
                    int id = (int)command.ExecuteScalar();
                    return id;
                }
            }
        }

        #endregion

        #region CREATE GAME RESULT

        /* Adds gameresult by when game is finished*/

        public static int AddGameResult(GameResult gameResult) 
        {
            string stmt = $"INSERT INTO game_result (player_id, tries, win, level, time, totalscore ) values (@Id, @Tries, @Win, @Level, @Time, @TotalScore) returning game_id;";


            using (var conn = new NpgsqlConnection(connectionString))


            {
                using (var command = new NpgsqlCommand(stmt, conn))
                {

                    conn.Open();
                    conn.TypeMapper.MapEnum<Level>("level");
                    command.Parameters.AddWithValue("Id", gameResult.PlayerId);
                    command.Parameters.AddWithValue("Tries", gameResult.Tries);
                    command.Parameters.AddWithValue("Win", gameResult.Win);
                    command.Parameters.AddWithValue("level", gameResult.Level);
                    command.Parameters.AddWithValue("time", gameResult.ElapsedTimeInSeconds);
                    command.Parameters.AddWithValue("totalscore", gameResult.TotalScore);
                    int id = (int)command.ExecuteScalar();
                    return id;
                }
            }
        }
        #endregion
        #endregion

        #region READ
        #region READ PLAYER
        public static ObservableCollection<IPlayer> GetPlayers()
        {
            string stmt = "select player_id, name from player";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                Player player = null;
                ObservableCollection<IPlayer> players = new ObservableCollection<IPlayer>();
                conn.Open();
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player
                            {
                                Id = (int)reader["player_id"],
                                Name = (string)reader["name"]
                            };

                            players.Add(player);
                        }
                    }
                }
                return players;
            }
        }

        public static ObservableCollection<IPlayer> GetPlayersByName(string name)
        {
            string stmt = "select player_id, name from player where name like @name";
            name = name + '%';

            using (var conn = new NpgsqlConnection(connectionString))
            {
                Player player = null;
                ObservableCollection<IPlayer> players = new ObservableCollection<IPlayer>();
                conn.Open();
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    command.Parameters.AddWithValue("name", name);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player
                            {
                                Id = (int)reader["player_id"],
                                Name = (string)reader["name"]
                            };

                            players.Add(player);
                        }
                    }
                }
                return players;
            }
        }

        public static ObservableCollection<IPlayer> GetDiligentPlayersOnLevel(Level level)
        {
            string stmt = "SELECT game_result.player_id, player.name, COUNT(game_result.player_id) FROM game_result INNER JOIN player ON game_result.player_id = player.player_id WHERE game_result.level = @level GROUP BY game_result.player_id, player.name, game_result.level ORDER BY COUNT DESC LIMIT 3;";
            using (var conn = new NpgsqlConnection(connectionString))
            {
                Player player = null;
                ObservableCollection<IPlayer> diligentPlayers = new ObservableCollection<IPlayer>();

                conn.Open();
                conn.TypeMapper.MapEnum<Level>("level");
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    command.Parameters.AddWithValue("level",level);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player()
                            {
                                Id = (int)reader["player_id"],
                                DisplayName = (string)reader["name"],
                                DisplayCount = (Int64)reader["COUNT"]
                            };

                            diligentPlayers.Add(player);
                        }
                    }
                }
                return new ObservableCollection<IPlayer>(diligentPlayers);
            }
        }
        #endregion


        public static ObservableCollection<IGameResult> GetGameResults(Level level)
        {
            string stmt = "select game_id, player.player_id, player.name, tries, time, win, level, totalscore from game_result inner join player ON game_result.player_id=player.player_id where win = true and level = @level ORDER BY totalscore DESC LIMIT 3" ;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                GameResult gameResult = null;
                ObservableCollection<IGameResult> gameResults = new ObservableCollection<IGameResult>();

                conn.Open();
                conn.TypeMapper.MapEnum<Level>("level");
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    command.Parameters.AddWithValue("level", level);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gameResult = new GameResult()
                            {
                                GameId = (int)reader["game_id"],
                                PlayerId = (int)reader["player_id"],
                                PlayerName = (string)reader["name"],
                                ElapsedTimeInSeconds = (double)reader["time"],
                                Tries = (int)reader["tries"],
                                Win = (bool)reader["win"],
                                Level = (Level)reader["level"],
                                TotalScore = (double)reader["totalscore"]
                            };

                            gameResults.Add(gameResult);
                        }
                    }
                }
                return gameResults;
            }
        }


        #endregion
    }
}
