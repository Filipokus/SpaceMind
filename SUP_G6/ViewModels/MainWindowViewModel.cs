﻿using SUP_G6.ViewModels.BaseViewModel;
using SUP_G6.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SUP_G6.ViewModels
{
    class MainWindowViewModel : BaseViewModel.BaseViewModel
    {
        SoundPlayer starwarsMainTheme;
        SoundPlayer CantinaBand;
        SoundPlayer imperialMarch;
        List<SoundPlayer> soundList = new List<SoundPlayer>();
        int trackCounter = 0;
        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        public MainWindowPage windowPage = new MainWindowPage();

        public MainWindowViewModel()
        {
            windowPage.main2.Content = new StartPage();
            PopulateSongList();
            soundList[trackCounter].PlayLooping();
            SoundOffCommand = new RelayCommand(SoundOff);
            SoundOnCommand = new RelayCommand(SoundOn);
            PreviousSongCommand = new RelayCommand(PrevSong);
            NextSongCommand = new RelayCommand(NextSong);
        }

        #region Public Properties
        public Visibility TurnSoundOff { get; set; } = Visibility.Visible;
        public Visibility TurnSoundOn { get; set; } = Visibility.Collapsed;
        #endregion

        #region ICommands
        public ICommand SoundOffCommand { get; set; }
        public ICommand SoundOnCommand { get; set; }
        public ICommand PreviousSongCommand { get; set; }
        public ICommand NextSongCommand { get; set; }        
        #endregion

        private void PopulateSongList()
        {
            soundList.Add(starwarsMainTheme = new SoundPlayer(Properties.Resources.starwars));
            soundList.Add(CantinaBand = new SoundPlayer(Properties.Resources.cantinaband));
            soundList.Add(imperialMarch = new SoundPlayer(Properties.Resources.imperial));
        }
        private void SoundOff()
        {
            soundList[trackCounter].Stop();
            TurnSoundOff = Visibility.Collapsed;
            TurnSoundOn = Visibility.Visible;
        }
        private void SoundOn()
        {
            soundList[trackCounter].Play();
            TurnSoundOn = Visibility.Collapsed;
            TurnSoundOff = Visibility.Visible;
        }


        private void NextSong()
        {
            var lastIndex = soundList.Count() - 1;
            if (TurnSoundOff == Visibility.Visible)
            {
                if (trackCounter == (int)lastIndex)
                {
                    trackCounter = 0;
                    soundList[trackCounter].PlayLooping();
                }

                else
                {
                    trackCounter++;
                    soundList[trackCounter].PlayLooping();
                }

            }

            else
            {
                trackCounter++;
            }


        }

        private void PrevSong()
        {
            if (TurnSoundOff == Visibility.Visible)
            {
                if (trackCounter == 0)
                {
                    trackCounter = soundList.Count();
                    trackCounter--;
                    soundList[trackCounter].PlayLooping();
                }

                else
                {
                    trackCounter--;
                    soundList[trackCounter].PlayLooping();
                }
            }
            else
            {
                trackCounter--;
            }


        }


    }
}

