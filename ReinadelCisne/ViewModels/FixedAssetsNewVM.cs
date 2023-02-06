using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class FixedAssetsNewVM : BaseVM
    {
        private FixedAssetsModel _fixedAsset;
        private string _groupSelectedFA;
        private int _cant;

        public FixedAssetsModel FixedAsset
        {
            get => _fixedAsset;
            set
            {
                _fixedAsset = value;
                OnPropertyChanged();
            }
        }
        public int Cant
        {
            get => _cant;
            set
            {
                _cant = value;
                OnPropertyChanged();
            }
        }
        public string GroupSelectedFA
        {
            get => _groupSelectedFA;
            set
            {
                _groupSelectedFA = value;
                OnPropertyChanged();
            }
        }
        public ICommand PushCommand => new Command((param) =>
        {
            Direccionar(param as string);
        });

        private void Direccionar(string parameter)
        {
            switch (parameter)
            {
                case "save":
                    SaveFA();
                    Regresar();
                    break;
                case "cancel":
                    Regresar();
                    break;
                case "sumar":
                    break;
                case "restar":
                    break;
                default: break;
            }
        }

        private void Regresar()
        {
            Shell.Current.GoToAsync("..");
        }

        private async void SaveFA()
        {
            if(FixedAsset.Name != null && FixedAsset.ValorUnit != 0)
            {
                FixedAsset.Grupo = GroupSelectedFA;
                FixedAsset.Amount = Cant;
                var resp = await App.Database.SaveFixedAssets(FixedAsset);
                if (resp != 0)
                {
                    await Shell.Current.DisplayAlert("Activo Guardado", FixedAsset.Name + " se guardó exitosamente", "ok");
                }
            }
        }

        public FixedAssetsNewVM()
        {
            FixedAsset = new FixedAssetsModel();
        }
            
    }
}
