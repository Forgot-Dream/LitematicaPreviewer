using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LitematicaPreviewer.Common.Extension;
using LitematicaPreviewer.Common.Structs;
using LitematicaPreviewer.LitematicaLib.Common;
using Microsoft.Win32;

namespace LitematicaPreviewer.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {
        [ObservableProperty] private string? _openedFilePath;

        [ObservableProperty] private Schematic? _schematic;

        [ObservableProperty] private List<MaterialInfoItem> _materialInfoItems = [];



        [RelayCommand]
        public Task OpenFile()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Litematica Files (*.litematic)|*.litematic"
                };

                if (dialog.ShowDialog() == true)
                {
                    OpenedFilePath = dialog.FileName;
                }

                if(string.IsNullOrEmpty(OpenedFilePath))
                {
                    return;
                }   

                Schematic = Schematic.Load(OpenedFilePath);

                MaterialInfoItems = Schematic.GetMaterialInfo();
            });
        }
    }
}
