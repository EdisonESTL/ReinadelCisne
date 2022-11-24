using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Xamarin.Forms;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
using System.Collections.ObjectModel;

namespace ReinadelCisne.ViewModels
{
    public class KardexProductVM : BaseVM, IQueryAttributable
    {
        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PassString> Kard { get; private set; } = new ObservableCollection<PassString>();

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged();
            }
        }

        public KardexProductVM()
        {
            //ObtenerKardex(product);
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string ObjPass = HttpUtility.UrlDecode(query["objId"]);
                ObtenerObjeto(ObjPass);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        public ProductModel product = new ProductModel();
        private void ObtenerObjeto(string objPass)
        {
            try
            {
                ProductModel objresp = App.Database.Get1Product(int.Parse(objPass)).Result;
                Nombre = objresp.NameProduct;
                Descripcion = objresp.DescriptionProduct;
                //product = objresp
                ObtenerKardex(objresp);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load animal.");
            }
            
        }

        private void ObtenerKardex(ProductModel objresp)
        {
            List<KardexModel> resp = App.Database.GetKardices(objresp).Result;
            LLenarList(resp, objresp.DateTime);
            
        }

        private void LLenarList(List<KardexModel> resp, DateTime dateTime)
        {
            foreach (var b in resp)
            {
                PassString pass = new PassString
                {
                    Data0 = dateTime.ToString("d"),
                    Data1 = "$" + b.ValorPromPond.ToString(),
                    Data2 = b.Cantidad.ToString(),
                    Data3 = "$" + b.Valor.ToString()
                };

                Kard.Add(pass);
            }
        }
    }
}
