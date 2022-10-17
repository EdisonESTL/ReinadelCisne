using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReinadelCisne.Services
{
    public class SearchProductos : SearchHandler
    {
        public IList<ProductModel> Products { get; set; }
        public Type SelectedItemNavigationTarget { get; set; }
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = App.Database.ListProduct().Result
                    .Where(producto => producto.NameProduct.ToLower().Contains(newValue.ToLower()))
                    .ToList<ProductModel>();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            // Let the animation complete
            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this application.
            await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((ProductModel)item).NameProduct}");
        }

        string GetNavigationTarget()
        {
            return (Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        }
    }
}
