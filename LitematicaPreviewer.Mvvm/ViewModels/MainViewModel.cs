using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace LitematicaPreviewer.Mvvm.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
        /// <summary>
        /// 打开的投影文件的路径
        /// </summary>
        [ObservableProperty]
        private string? _current_file_path;

        [RelayCommand]
        private void OpenFile()
        {
            var openFileDialog =  new ();
        }
    }
}
