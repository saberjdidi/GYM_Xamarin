using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Interfaces;
using XamarinApplication.Resources;

namespace XamarinApplication.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string Ok
        {
            get { return Resource.Ok; }
        }
        public static string UsernameValidation
        {
            get { return Resource.UsernameValidation; }
        }
        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }
        public static string Username
        {
            get { return Resource.Username; }
        }
        public static string Password
        {
            get { return Resource.Password; }
        }
        public static string Warning
        {
            get { return Resource.Warning; }
        }
        public static string CheckConnection
        {
            get { return Resource.CheckConnection; }
        }
        public static string FailedLogin
        {
            get { return Resource.FailedLogin; }
        }
        public static string Exit
        {
            get { return Resource.Exit; }
        }
        public static string Yes
        {
            get { return Resource.Yes; }
        }
        public static string No
        {
            get { return Resource.No; }
        }
        public static string Planning
        {
            get { return Resource.Planning; }
        }
        public static string Licence
        {
            get { return Resource.Licence; }
        }
        public static string Serie
        {
            get { return Resource.Serie; }
        }
        public static string GYM
        {
            get { return Resource.GYM; }
        }
        public static string Machine
        {
            get { return Resource.Machine; }
        }
        public static string Exercice
        {
            get { return Resource.Exercice; }
        }
        public static string Program
        {
            get { return Resource.Program; }
        }
        public static string BestFitness
        {
            get { return Resource.BestFitness; }
        }
        public static string Collection
        {
            get { return Resource.Collection; }
        }
        public static string ShowAll
        {
            get { return Resource.ShowAll; }
        }
        public static string Logout
        {
            get { return Resource.Logout; }
        }
        public static string Date
        {
            get { return Resource.Date; }
        }
        public static string Informations
        {
            get { return Resource.Informations; }
        }
        public static string Exercices
        {
            get { return Resource.Exercices; }
        }
        public static string Sportif
        {
            get { return Resource.Sportif; }
        }
        public static string Coach
        {
            get { return Resource.Coach; }
        }
        public static string Code
        {
            get { return Resource.Code; }
        }
        public static string Name
        {
            get { return Resource.Name; }
        }
        public static string Description
        {
            get { return Resource.Description; }
        }
        public static string SearchGYM
        {
            get { return Resource.SearchGYM; }
        }
        public static string TotalSeries
        {
            get { return Resource.TotalSeries; }
        }
        public static string TotalReputation
        {
            get { return Resource.TotalReputation; }
        }
        public static string SearchLicence
        {
            get { return Resource.SearchLicence; }
        }
        public static string Number
        {
            get { return Resource.Number; }
        }
        public static string StartDate
        {
            get { return Resource.StartDate; }
        }
        public static string EndDate
        {
            get { return Resource.EndDate; }
        }
        public static string SearchMachine
        {
            get { return Resource.SearchMachine; }
        }
        public static string SerialNumber
        {
            get { return Resource.SerialNumber; }
        }
        public static string Initials
        {
            get { return Resource.Initials; }
        }
        public static string EffortIncrease
        {
            get { return Resource.EffortIncrease; }
        }
        public static string SelectLanguage
        {
            get { return Resource.SelectLanguage; }
        }
        public static string Language
        {
            get { return Resource.Language; }
        }
        public static string Licences
        {
            get { return Resource.Licences; }
        }
        public static string Plannings
        {
            get { return Resource.Plannings; }
        }
        public static string Machines
        {
            get { return Resource.Machines; }
        }
        public static string Programs
        {
            get { return Resource.Programs; }
        }
        public static string Series
        {
            get { return Resource.Series; }
        }
        public static string PurchasedPackages
        {
            get { return Resource.PurchasedPackages; }
        }
        public static string PurchasedPackage
        {
            get { return Resource.PurchasedPackage; }
        }
        public static string Hour
        {
            get { return Resource.Hour; }
        }
        public static string PurchasedDate
        {
            get { return Resource.PurchasedDate; }
        }
        public static string TotalConsumed
        {
            get { return Resource.TotalConsumed; }
        }
        public static string Note
        {
            get { return Resource.Note; }
        }
        public static string TotalPurchased
        {
            get { return Resource.TotalPurchased; }
        }
        public static string BreakDuration
        {
            get { return Resource.BreakDuration; }
        }
        public static string Level
        {
            get { return Resource.Level; }
        }
        public static string Firstname
        {
            get { return Resource.Firstname; }
        }
        public static string Lastname
        {
            get { return Resource.Lastname; }
        }
        public static string Address
        {
            get { return Resource.Address; }
        }
        public static string Tel
        {
            get { return Resource.Tel; }
        }
        public static string WebSite
        {
            get { return Resource.WebSite; }
        }
        public static string ReferencePerson
        {
            get { return Resource.ReferencePerson; }
        }
        public static string ActivationDate
        {
            get { return Resource.ActivationDate; }
        }
        public static string DeliveryDate
        {
            get { return Resource.DeliveryDate; }
        }
        public static string SecondsIncrease
        {
            get { return Resource.SecondsIncrease; }
        }
        public static string Limits
        {
            get { return Resource.Limits; }
        }
        public static string TimeStaticity
        {
            get { return Resource.TimeStaticity; }
        }
        public static string TypeLoad
        {
            get { return Resource.TypeLoad; }
        }
        public static string Speed
        {
            get { return Resource.Speed; }
        }
        public static string ConsumedPackage
        {
            get { return Resource.ConsumedPackage; }
        }
        public static string ConsumedPackages
        {
            get { return Resource.ConsumedPackages; }
        }
        public static string ConsumedHour
        {
            get { return Resource.ConsumedHour; }
        }
        public static string ConsumedDate
        {
            get { return Resource.ConsumedDate; }
        }
        public static string DeletedDate
        {
            get { return Resource.DeletedDate; }
        }
        public static string Calendar
        {
            get { return Resource.Calendar; }
        }
        public static string Configuration
        {
            get { return Resource.Configuration; }
        }
        public static string ExerciceTypology
        {
            get { return Resource.ExerciceTypology; }
        }
        public static string NoResultFound
        {
            get { return Resource.NoResultFound; }
        }
        public static string NameRequired
        {
            get { return Resource.NameRequired; }
        }
        public static string Beginning
        {
            get { return Resource.Beginning; }
        }
        public static string End
        {
            get { return Resource.End; }
        }
        public static string Save
        {
            get { return Resource.Save; }
        }
        public static string Close
        {
            get { return Resource.Close; }
        }
        public static string Update
        {
            get { return Resource.Update; }
        }
        public static string NewExercice
        {
            get { return Resource.NewExercice; }
        }
        public static string EditExercice
        {
            get { return Resource.EditExercice; }
        }
        public static string NewGYM
        {
            get { return Resource.NewGYM; }
        }
        public static string EditGYM
        {
            get { return Resource.EditGYM; }
        }
        public static string CodeRequired
        {
            get { return Resource.CodeRequired; }
        }
        public static string DescriptionRequired
        {
            get { return Resource.DescriptionRequired; }
        }
        public static string LicenceRequired
        {
            get { return Resource.LicenceRequired; }
        }
        public static string NewLicence
        {
            get { return Resource.NewLicence; }
        }
        public static string EditLicence
        {
            get { return Resource.EditLicence; }
        }
        public static string NumberRequired
        {
            get { return Resource.NumberRequired; }
        }
        public static string NewMachine
        {
            get { return Resource.NewMachine; }
        }
        public static string EditMachine
        {
            get { return Resource.EditMachine; }
        }
        public static string SerialNumberRequired
        {
            get { return Resource.SerialNumberRequired; }
        }
        public static string NewPlanning
        {
            get { return Resource.NewPlanning; }
        }
        public static string EditPlanning
        {
            get { return Resource.EditPlanning; }
        }
        public static string AthleticRequired
        {
            get { return Resource.AthleticRequired; }
        }
        public static string ProgramsRequired
        {
            get { return Resource.ProgramsRequired; }
        }
        public static string NewProgram
        {
            get { return Resource.NewProgram; }
        }
        public static string EditProgram
        {
            get { return Resource.EditProgram; }
        }
    }
}
