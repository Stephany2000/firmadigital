using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;




using Xamarin.Essentials;

using SignaturePad.Forms;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace firmadigital.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirmasPage : ContentPage
    {
        public byte[] bytefirma;
        public FirmasPage()
        {
            InitializeComponent();
        }


        private async Task<bool> validar()
        {
            if (String.IsNullOrWhiteSpace(Nombre.Text))
            {
                await this.DisplayAlert("Error", "Escribir nombre", "OK");
                return false;
            }

            if (String.IsNullOrWhiteSpace(Descripcion.Text))
            {
                await this.DisplayAlert("Error", "Escribir descripción", "OK");
                return false;
            }

            if (Pantalla.IsBlank)
            {
                await this.DisplayAlert("Error", "Escribir firma digital", "OK");
                return false;
            }

            return true;
        }

        [Obsolete]
       
        private async void btnguardar_Clicked(object sender, EventArgs e)
        {

            using (MemoryStream memory = new MemoryStream())
            {

                Stream stream = await Pantalla.GetImageStreamAsync(SignatureImageFormat.Png);
                stream.CopyTo(memory);
                bytefirma = memory.ToArray();
            }

            Stream image = await Pantalla.GetImageStreamAsync(SignatureImageFormat.Png);

            if (image == null)
                return;
            BinaryReader br = new BinaryReader(image);
            br.BaseStream.Position = 0;
            Byte[] All = br.ReadBytes((int)image.Length);
            byte[] imagen = (byte[])All;
            ImageSource imageSource = null;
            imageSource = ImageSource.FromStream(() => new MemoryStream(imagen));

            bool storegaePermissionResp = await IsAvailStoragePermission();
            if (storegaePermissionResp)
            {
                var path = DependencyService.Get<Storage>().SaveImage(imagen);

                await DisplayAlert("Guardado", "Firma Guardada", "Ok");
            }
            else
            {
                await DisplayAlert("Notificacion", "El usuario denegó el permiso de almacenamiento", "Ok");
            }

            //Boton de Guardar
            if (await validar())
            {


                try
                {
                    //var personas = (Models.Personas)BindingContext;

                    var Firma = new Models.Firma
                    {
                        nombre = this.Nombre.Text,
                        descripcion = this.Descripcion.Text,
                        fotofirma = bytefirma,

                    };



                    var resultado = await App.BaseDatos.GrabarFirma(Firma);

                    if (resultado == 1)
                    {
                        await DisplayAlert("Agregado", "Ingresado Exitosamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo agregar", "OK");
                    }

                }

                catch (Exception ex)
                {
                    await DisplayAlert("Exito", "Guardado Correctamente", "OK");
                    await Navigation.PushAsync(new FirmasPage());
                    Pantalla.Clear();
                }
            }
        }

        [Obsolete]
        private async Task<bool> IsAvailStoragePermission()
        {


            bool response = false;
            Plugin.Permissions.Abstractions.PermissionStatus permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (permissionStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var resp = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                if (resp.ContainsKey(Permission.Storage))
                {
                    permissionStatus = resp[Permission.Storage];
                }


                if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    response = true;
                }
                else if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Denied)
                {
                    response = false;
                }
            }
            else
            {
                response = true;
            }
            return response;
        }

        private async void btnlista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FirmaLista());
        }
    }
}