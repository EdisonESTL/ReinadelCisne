using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class InOtherCostVM : BaseVM, IQueryAttributable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value;
                OnPropertyChanged();
            }
        }
        
        private string _description;
        public string Description 
        { 
            get { return _description; }
            set { _description = value;
                OnPropertyChanged();
            }
        }
        
        private string _cost;
        public string Cost
        {
            get { return _cost; }
            set { _cost = value;
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

        private double _count = 0;
        public double Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set
            {
                _longList = value;
                OnPropertyChanged();
            }
        }
        
        public string IdMOD;

        public int IdList;
        public ObservableCollection<OtherCostModel> OtherCosts { get; set; } = new ObservableCollection<OtherCostModel>();
        
        public ICommand goback
        {
            get
            {
                return new Command(() =>
                {
                    Shell.Current.GoToAsync($"..?IdListOC=0");
                });
            }
        }
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListProductStock();

            IsRefreshing = false;
        });
        public ICommand AddCommand => new Command(() =>
        {
            if(!string.IsNullOrEmpty(Description) & !string.IsNullOrEmpty(Cost))
            {
                OtherCostModel otherCost = new OtherCostModel
                {
                    Id = Id,
                    DescriptionOC = Description,
                    CostOC = Convert.ToDouble(Cost)
                };

                if (IdMOD != null)
                {
                    OtherCosts.RemoveAt(int.Parse(IdMOD));
                    OtherCosts.Insert(int.Parse(IdMOD), otherCost);
                    Shell.Current.DisplayAlert("Éxito", otherCost.DescriptionOC + " Ha sido modificado", "Ok");
                    Count = 0;
                    foreach (var a in OtherCosts)
                    {
                        Count += a.CostOC;
                    }

                    IdMOD = null;
                }
                else
                {
                    OtherCosts.Add(otherCost);
                    Shell.Current.DisplayAlert("Éxito", otherCost.DescriptionOC + " Ha sido añadido", "Ok");
                    Count += otherCost.CostOC;
                }

                LongList = OtherCosts.Count;

                Description = string.Empty;
                Cost = string.Empty;
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Ingrese la descripcion del costo y el valor $", "ok");
            }
        });
        public ICommand FinshCommand => new Command(() =>
        {
            if(OtherCosts.Count != 0)
            {
                ListOCModel listOC = new ListOCModel
                {
                    Id = IdList
                };
                App.Database.SaveListOC(listOC);

                listOC.OtherCosts = new List<OtherCostModel>();
                foreach (var f in OtherCosts)
                {
                    App.Database.SaveOtherCost(f);
                    listOC.OtherCosts.Add(f);

                    App.Database.UpdateListOC(listOC);
                }

                var gy = App.Database.GetListsOC();
                gy.Wait();
                var fgh = gy.Result;

                var fd = listOC.Id;

                Shell.Current.GoToAsync($"..?IdListOC={fd}&IdlistRM=0&IdlistWF=0&idProduct=0");
            }
            else
            {
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct=0");
            }            
        });
        public ICommand CancelCommand => new Command(() =>
        {
            Id = 0;
            IdList = 0;
            Count = 0;
            LongList = 0;

            Description = string.Empty;
            Cost = string.Empty;
            OtherCosts.Clear();
        });
        public Command<OtherCostModel> DeleteCommand { get; set; }
        public Command<OtherCostModel> ModifyCommand { get; set; }
        public InOtherCostVM()
        {
            DeleteCommand = new Command<OtherCostModel>(DeleteOC);
            ModifyCommand = new Command<OtherCostModel>(ModifyOC);
        }

        private void ModifyOC(OtherCostModel obj)
        {
            IdMOD = Convert.ToString(OtherCosts.IndexOf(obj));

            Id = obj.Id;
            Description = obj.DescriptionOC;
            Cost = obj.CostOC.ToString();
        }

        private void DeleteOC(OtherCostModel obj)
        {
            OtherCosts.Remove(obj);

            if (Convert.ToString(IdList) != null)
            {
                App.Database.DeleteOtherCost(obj);
            }

            Count -= obj.CostOC;
            LongList = OtherCosts.Count;
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idListOC = HttpUtility.UrlDecode(query["IdListOC"]);

                LoadLS(idListOC);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
        private void LoadLS(string idListOC)
        {
            Count = 0;
            var t = App.Database.GetListOC(int.Parse(idListOC)).Result;

            foreach (var d in t.OtherCosts)
            {
                OtherCosts.Add(d);
                Count += d.CostOC;
            }

            IdList = int.Parse(idListOC);
            LongList = t.OtherCosts.Count;
        }
    }
}
