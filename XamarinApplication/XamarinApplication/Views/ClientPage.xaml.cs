using Newtonsoft.Json;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Rg.Plugins.Popup.Services;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientPage : ContentPage
    {
        // public IDownloadFile File; //2 method
        // bool isDownloading = true; //2 method
        public ClientPage()
        {
            InitializeComponent();
            BindingContext = new ClientViewModel();

           /* //2 method
            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
            System.Diagnostics.Debug.WriteLine(
                "[DownloadManager] " + e.Action +
                " -> New items: " + (e.NewItems?.Count ?? 0) +
                " at " + e.NewStartingIndex +
                " || Old items: " + (e.OldItems?.Count ?? 0) +
                " at " + e.OldStartingIndex
                );*/
        }
        /* //2 method
        public bool IsDownloading(IDownloadFile File)
        {
            if (File == null) return false;

            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;

                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public async void DownloadFile(String FileName)
        {
            await Task.Yield();
            await Task.Run(() =>
            {
                var downloadManager = CrossDownloadManager.Current;
                var file = downloadManager.CreateDownloadFile(FileName);
                downloadManager.Start(file, true);

                while (isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }
            });

            if (!isDownloading)
            {
                await DisplayAlert("File Status", "File Downloaded", "OK");
            }
        }
        public void AbortDownloading()
        {
            CrossDownloadManager.Current.Abort(File);
        }
        */
        private async void Download_pdf(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;

            var accesstoken = Settings.AccessToken;

            var request = JsonConvert.SerializeObject(user);
            Debug.WriteLine("********request*************");
            Debug.WriteLine(request);
            var content = new StringContent(
                request,
                Encoding.UTF8,
                "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var url = "http://phoneofficine.it/niini-gim/user/eventsReport?format=pdf";
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);
            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                return;
            }
                var pdf = JsonConvert.DeserializeObject<PdfClient>(result);
            
            byte[] bytes = Convert.FromBase64String(pdf.report);
            MemoryStream stream = new MemoryStream(bytes);

            await DependencyService.Get<ISave>().SaveAndView(pdf.name, pdf.defaultExtention, stream);

            //**************cette methode permet de créer pdf********************* 
            /* 
             // Create a new PDF document
              PdfDocument document = new PdfDocument();
              // Set the page size.
              //document.PageSettings.Size = PdfPageSize.A4;
              //Add a page to the document
              PdfPage page = document.Pages.Add();
              //Create PDF graphics for the page
              PdfGraphics graphics = page.Graphics;
              //Set the standard font
              PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);

              string linkSource = "data:application/pdf;base64," + pdf.report;
              byte[] sPDFDecoded = Convert.FromBase64String(pdf.report);
              string s = Encoding.UTF8.GetString( sPDFDecoded);
              //Draw the text
              graphics.DrawString("Bonjour Bassem cava !", font, PdfBrushes.Black, new PointF(0, 0));

              //Save the document to the stream
              MemoryStream stream = new MemoryStream(sPDFDecoded);
              stream.ReadByte();
             // stream.Write(sPDFDecoded, 0, sPDFDecoded.Length);
              document.Save(stream);
              document.Close(true);
              //Save the stream as a file in the device and invoke it for viewing
              await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView(pdf.name, pdf.defaultExtention, stream);*/

            //**************cette methode permet de telecharger pdf a partir d'un Url //2 method*****************
            /*  
              string linkSource = "data:application/pdf;base64," + pdf.report;
              byte[] sPDFDecoded = Convert.FromBase64String(pdf.report);
              byte[] byteArray = Encoding.ASCII.GetBytes(pdf.report);
              string s = Encoding.UTF8.GetString(sPDFDecoded);

              MemoryStream stream = new MemoryStream(byteArray);
              StreamReader reader = new StreamReader(stream);
              string text = reader.ReadToEnd();
              var Url = "https://info.sio2.be/tdtooo/sostdt.pdf";
              DownloadFile(text); */
        }

        private async void Add_Client(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddClientPage());
        }
        private async void Client_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;
            await Navigation.PushAsync(new ClientDetailPage(user));
        }
        private async void Update_Client(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;
            await PopupNavigation.Instance.PushAsync(new UpdateClientPage(user));
        }
        private async void Parametres_Client(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;
            await PopupNavigation.Instance.PushAsync(new ParametresClientPage(user));
        }
        private async void Lessons_Client(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;
            await PopupNavigation.Instance.PushAsync(new LessonsClientPage(user));
        }
        private async void Bia_Client(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var user = mi.CommandParameter as User;
            await PopupNavigation.Instance.PushAsync(new BiaClientPage(user));
        }
    }
}