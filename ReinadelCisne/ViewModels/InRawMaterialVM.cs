using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Collections.ObjectModel;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class InRawMaterialVM : BaseVM, IQueryAttributable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        private string _nameRM;
        public string NameRM
        {
            get { return _nameRM; }
            set
            {
                _nameRM = value;
                OnPropertyChanged();
            }
        }
        
        private string _unitMeasurementRM;
        public string UnitMeasurementRM
        {
            get { return _unitMeasurementRM; }
            set
            {
                _unitMeasurementRM = value;
                OnPropertyChanged();
            }
        }
        
        private string _costRM;
        public string CostRM
        {
            get { return _costRM; }
            set
            {
                _costRM = value;
                OnPropertyChanged();
            }
        }
        
        private double _amountRM;
        public double AmountRM
        {
            get { return _amountRM; }
            set
            {
                _amountRM = value;
                OnPropertyChanged();
            }
        } //poner double

        private string _typeRM;
        public string TypeRM
        {
            get { return _typeRM; }
            set { _typeRM = value;
                OnPropertyChanged();
            }
        }
        
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private float _count = 0;
        public float Count
        {
            get { return _count; }
            set { _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set { _longList = value;
                OnPropertyChanged();
            }
        }

        public string IdMOD;
        public int IdList;
        public ObservableCollection<RawMaterialModel> ListRawMl { get; set; } = new ObservableCollection<RawMaterialModel>();

        public ICommand goback
        {
            get
            {
                return new Command(() =>
                {
                    Shell.Current.GoToAsync($"..?IdlistRM=0");
                });
            }
        }
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRawMl.get

            IsRefreshing = false;
        });
        public ICommand AddRM => new Command(() =>
        {
            if(!string.IsNullOrEmpty(NameRM) & AmountRM != 0 & CostRM != null)
            {
                RawMaterialModel rawMaterial = new RawMaterialModel()
                {
                    Id = Id,
                    NameRM = NameRM,
                    UnitMeasurementRM = UnitMeasurementRM,
                    CostoRM = float.Parse(CostRM),
                    AmountRM = AmountRM,
                    TypeRM = TypeRM
                };

                if (IdMOD != null)
                {
                    ListRawMl.RemoveAt(int.Parse(IdMOD));
                    ListRawMl.Insert(int.Parse(IdMOD), rawMaterial);
                    Shell.Current.DisplayAlert("Éxito", rawMaterial.NameRM + " Ha sido modificado", "Ok");
                    Count = 0;
                    foreach (var a in ListRawMl)
                    {
                        Count += (float)(a.CostoRM * a.AmountRM);
                    }

                    IdMOD = null;
                }
                else
                {
                    ListRawMl.Add(rawMaterial);
                    Shell.Current.DisplayAlert("Éxito", rawMaterial.NameRM + " Ha sido añadido", "Ok");
                    Count += (float)(rawMaterial.CostoRM * rawMaterial.AmountRM);
                }

                LongList = ListRawMl.Count;

                NameRM = string.Empty;
                UnitMeasurementRM = string.Empty;
                CostRM = string.Empty;
                AmountRM = 0;
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Ingrese la cantidad, nombre de materia prima y el precio unitario", "ok");
            }
            
        });
        public ICommand CancelRM => new Command(() =>
        {
            Id = 0;
            IdList = 0;
            NameRM = string.Empty;
            UnitMeasurementRM = string.Empty;
            CostRM = string.Empty;
            AmountRM = 0;
            ListRawMl.Clear();
            Count = 0;
            LongList = 0;
        });
        public ICommand FinishRM => new Command(() =>
        {
            var ghj = ListRawMl.Count;
            if (ghj != 0)
            {
                ListRMModel b = new ListRMModel
                {
                    Id = IdList
                };
                App.Database.SaveListRM(b);
                b.RawMaterials = new List<RawMaterialModel>();

                foreach (var f in ListRawMl)
                {
                    App.Database.SaveRawMaterial(f);
                    b.RawMaterials.Add(f);

                    App.Database.UpdateListRM(b);
                }


                var gy = App.Database.GetV();
                gy.Wait();
                var fgh = gy.Result;

                var fd = b.Id;
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM={b.Id}&IdlistWF=0&idProduct=0");
            }
            else
            {
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct=0");
            }

        });
        public Command<RawMaterialModel> DeleteCommand { get; set; }     
        public Command<RawMaterialModel> ModifyCommand { get; set; }     
        
        public InRawMaterialVM()
        {
            DeleteCommand = new Command<RawMaterialModel>(DeleteRM);
            ModifyCommand = new Command<RawMaterialModel>(ModifyRM);
        }

        private void ModifyRM(RawMaterialModel obj)
        {
            IdMOD = Convert.ToString(ListRawMl.IndexOf(obj));

            Id = obj.Id;
            NameRM = obj.NameRM;
            UnitMeasurementRM = obj.UnitMeasurementRM;
            CostRM = obj.CostoRM.ToString("N2");
            AmountRM = obj.AmountRM;
        }
        private void DeleteRM(RawMaterialModel obj)
        {
            ListRawMl.Remove(obj);
            
            if (Convert.ToString(IdList) != null)
            {
                App.Database.DeleteRawMaterial(obj);
            }

            Count -= (float)(obj.CostoRM * obj.AmountRM);
            LongList = ListRawMl.Count;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idListRM = HttpUtility.UrlDecode(query["IdlistRM"]);

                LoadLS(idListRM);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void LoadLS(string idListRM)
        {
            Count = 0;
            var t = App.Database.GetListRM(int.Parse(idListRM)).Result;

            foreach(var d in t.RawMaterials)
            {
                ListRawMl.Add(d);
                Count += (float)(d.CostoRM * d.AmountRM);
            }

            IdList = int.Parse(idListRM);
            LongList = t.RawMaterials.Count;
        }
    }
}
