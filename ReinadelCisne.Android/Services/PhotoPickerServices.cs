using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ReinadelCisne.Services;

using Xamarin.Forms;
using Android.Provider;
using ReinadelCisne.Droid.Services;

[assembly: Dependency(typeof(PhotoPickerServices))]
namespace ReinadelCisne.Droid.Services
{
    public class PhotoPickerServices : IPhotoPickerService
    {
        public Task<Stream> GetImageStreamAsync()
        {
            //Definir la Intención para obtener imágenes
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
                       

            //Inicie la actividad del selector de imágenes(se reanuda en MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Photo"),
                MainActivity.PickImageId);

            //Guarde el objeto TaskCompletionSource como una propiedad MainActivity
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            //Objeto de tarea de devolución
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }

        /*public async Task<Stream> GetImageStreamAsync1()
        {
            Intent intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            MainActivity.Instance.StartActivityForResult(Intent.CreateChooser(intent, "Select Photo"), MainActivity.PickImageId);

            TaskCompletionSource<Stream> taskCompletionSource = new TaskCompletionSource<Stream>();
            MainActivity.Instance.ActivityResult += (sender, args) =>
            {
                if (args.RequestCode == MainActivity.PickImageId && args.ResultCode == Result.Ok)
                {
                    Android.Net.Uri uri = args.Data.Data;
                    Stream stream = MainActivity.Instance.ContentResolver.OpenInputStream(uri);
                    taskCompletionSource.SetResult(stream);
                }
                else
                {
                    taskCompletionSource.SetResult(null);
                }
            };

            return await taskCompletionSource.Task;
        }*/
    }
}