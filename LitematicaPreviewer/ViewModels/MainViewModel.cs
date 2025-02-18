using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LitematicaPreviewer.LitematicaLib.Common;
using Microsoft.Win32;

namespace LitematicaPreviewer.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {
        [ObservableProperty] private string? _openedFilePath;

        [ObservableProperty] private Schematic? _schematic;

        [RelayCommand]
        public Task OpenFile()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Litematica Files (*.litematic)|*.litematic";
                if (dialog.ShowDialog() == true)
                {
                    OpenedFilePath = dialog.FileName;
                }

                Schematic = Schematic.Load(OpenedFilePath);
            });
        }
    }
}
