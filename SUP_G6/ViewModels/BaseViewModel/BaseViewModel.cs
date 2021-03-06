﻿using SUP_G6.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SUP_G6.ViewModels.BaseViewModel
{
    // All view models inherits from BaseViewModel which enables INotifyPropertyChanged.
    public class BaseViewModel : INotifyPropertyChanged

    {
        #region Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Property Changed Method

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
