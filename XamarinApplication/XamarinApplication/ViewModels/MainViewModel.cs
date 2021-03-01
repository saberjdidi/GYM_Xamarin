
namespace XamarinApplication.ViewModels
{
    using System.Collections.Generic;
    using Models;

    public class MainViewModel
    {
        #region Properties
        public List<Program> ProgramsList
        {
            get;
            set;
        }
        public List<Licence> LicencesList
        {
            get;
            set;
        }
        public TokenResponse Token
        {
            get;
            set;
        }
        //string accesstoken = Settings.AccessToken;
        #endregion

        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }
        public LicenceViewModel Licences
        {
            get;
            set;
        }
        public ProgramViewModel Programs
        {
            get;
            set;
        }
        public ExerciceViewModel Exercices
        {
            get;
            set;
        }
        public MachineViewModel Machines
        {
            get;
            set;
        }

        public ProgramDetailViewModel Program
        {
            get;
            set;
        }
        #endregion
        #region Constructors
        public MainViewModel()
        {
            instance = this;
            //this.Login = new LoginViewModel();
            //Programs = new ProgramViewModel();
            //Licences = new LicenceViewModel();
            //Exercices = new ExerciceViewModel();
            //Program = new ProgramDetailViewModel(program);
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
