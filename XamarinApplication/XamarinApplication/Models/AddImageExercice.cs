using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddImageExercice
    {
        //public byte[] imageFile { get; set; }
        public MultipartFormDataContent imageFile { get; set; }
        public int idExercise { get; set; }
    }
}
