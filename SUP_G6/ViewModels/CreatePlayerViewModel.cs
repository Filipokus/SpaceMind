﻿using SUP_G6.Interface;
using SUP_G6.Models;
using SUP_G6.Other;
using SUP_G6.ViewModels.BaseViewModel;
using SUP_G6.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Npgsql;

namespace SUP_G6.ViewModels
{
    public class CreatePlayerViewModel 
    {
        #region Properties
        public string Name { get; set; }
        public Player Player { get; set; }
        public string CreatePlayerButton { get; set; } = "create player";
        public string BackButton { get; set; } = "back";
        #endregion

        #region ICommand
        public ICommand CreatePlayerCommand { get; set; }
        public ICommand BackButtonCommand { get; set; }
        #endregion

        public CreatePlayerViewModel()
        {

            CreatePlayerCommand = new RelayCommand(CreatePlayer);
            BackButtonCommand = new RelayCommand(BackToStart);
        }

        private void BackToStart()
        {
            var page1 = new StartPage();
            ((MainWindow)Application.Current.MainWindow).Main.Content = page1;
        }

        public void CreatePlayer()
        {
            if (Name.Length > 9)
            {
                MessageBox.Show($"Maximum ");
                    
            }
            if (Name != null)
            {
                Player player = new Player
                {

                    Name = this.Name.ToLower()  //The font convert text to ToUpper but doesnt render capitalized letters well in inputsstrings.

                };

                Player = player;
                //Player.Id = DataBaseLogic.AddPlayer(player);
                try
                {
                    Player.Id = DataBaseLogic.AddPlayer(player);
                    var page = new SelectLevelPage(player);
                    ((MainWindow)Application.Current.MainWindow).Main.Content = page;
                   
                }
                catch (PostgresException error)
                {
                    if (error.SqlState == "23505")
                    {
                        MessageBox.Show($"The name {player.Name} already exists. Pick another nickname.");
                        this.Name = "";
                    }
                }

                
            }
        }

    }
}
