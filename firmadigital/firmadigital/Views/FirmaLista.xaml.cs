using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace firmadigital.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirmaLista : ContentPage
    {
        public FirmaLista()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var listadireccione = await App.BaseDatos.ObtenerListaFirmas();
            lsfirma.ItemsSource = listadireccione;
        }

        private async void toolbar01_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new FirmasPage());

        }

        private void lsfirmas_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

    }
}