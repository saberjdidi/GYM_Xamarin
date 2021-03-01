
namespace XamarinApplication.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class ProgramItemViewModel : Program
    {
        #region Commands
        // first method
         public ICommand SelectProgramCommand
        {
            get
            {
                return new RelayCommand(SelectProgram);
            }
        }

        private async void SelectProgram()
        {
            //MainViewModel.GetInstance().Program = new ProgramDetailViewModel(this);
            //await Application.Current.MainPage.Navigation.PushAsync(new ProgramDetailPage());
        }
        #endregion
    }
}
