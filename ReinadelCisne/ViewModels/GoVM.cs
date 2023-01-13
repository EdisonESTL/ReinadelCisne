﻿using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Globalization;

namespace ReinadelCisne.ViewModels
{    
    public class GoVM : BaseVM
    {
        #region Atributos
        private double _entryDesc = 0;
        public double EntryDesc
        {
            get => _entryDesc;
            set
            {
                _entryDesc = value;
                OnPropertyChanged();
            }
        }

        private string _wayPay = "efectivo";
        public string WayPay
        {
            get => _wayPay;
            set
            {
                _wayPay = value;
                OnPropertyChanged();
            }
        }

        private string _clientexC;
        public string ClientexC
        {
            get => _clientexC;
            set
            {
                _clientexC = value;
                OnPropertyChanged();
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private double _cuenta = 0.00;
        public double Cuenta
        {
            get { return _cuenta; }
            set
            {
                _cuenta = value;
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

        private double _totaldescueotaplicado = 0;
        public double Totaldescueotaplicado
        {
            get => _totaldescueotaplicado;
            set
            {
                _totaldescueotaplicado = value;
                OnPropertyChanged();
            }
        }

        private bool _esPedido = false;
        public bool EsPedido
        {
            get { return _esPedido; }
            set
            {
                _esPedido = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<ProductModel> ListPS { get; private set; } = new ObservableCollection<ProductModel>();
        public ObservableCollection<GroupsProductModel> GruposProductos { get; private set; } = new ObservableCollection<GroupsProductModel>();
        private List<OrderModel> Order { get; set; } = new List<OrderModel>();

        #region Icommands
        //Actualiza lista de productos a vender
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ListProductStock();

            IsRefreshing = false;
        });
        //Eleccion de opcion en menu
        public ICommand SelectedCommand => new Command((obj) =>
        {
            ProductModel prods = obj as ProductModel;

            //ListOrder.Add(prods);
            SumarOrden(prods);
        });
        //Aplicar Descuento
        public ICommand DiscountCommand => new Command(() =>
        {
            if (Cuenta != 0)
            {
                AppplyDiscount();
            }
        });
        //Por cobrar
        public ICommand WaytoPayCommand => new Command(() =>
        {
            PaytoWay();
        });
        //Limpiar
        public ICommand ClearCommand => new Command(() =>
        {
            ClearOrder();
        });
        //Guarda la venta
        public ICommand NewCommand => new Command(async () =>
        {
            if (Order.Count > 0)
            {
                string res = await Shell.Current.DisplayPromptAsync("Hola", "Nombre de Cliente");
                ClientexC = res;
                ClientModel client = new ClientModel
                {
                    Name = ClientexC
                };

                await App.Database.SaveClients(client);

                if (EsPedido == false)
                {                    
                    SaleModel d = new SaleModel();
                    d.DateSale = DateTime.Now;
                    d.TotalSale = Cuenta;
                    d.Discount = EntryDesc;
                    d.WayToPay = WayPay;
                    d.DateDelivery = DateTime.Now;
                    d.SaleStatus = "Entregado";

                    await App.Database.SaveSale(d);
                    d.Orders = new List<OrderModel>();
                    d.ClientModel = new ClientModel();
                    d.ClientModel = client;

                    foreach (OrderModel obj in Order)
                    {
                        await App.Database.SaveOrder(obj);
                        d.Orders.Add(obj);
                        await App.Database.UpdateRealtionSales(d);
                        UpdateCantidadesInv(obj);
                    }

                }
                else
                {
                    //var celu = await Shell.Current.DisplayPromptAsync("Contacto", "ingrese numero telefonico del cliente");
                    var cultureInfo = new CultureInfo("es-EC");
                    var fechaentrega = await Shell.Current.DisplayPromptAsync("Fecha de entrega", "Dia/Mes/Año");

                    if (!string.IsNullOrEmpty(fechaentrega))
                    {
                        var datet = DateTime.Parse(fechaentrega, cultureInfo);
                        SaleModel d = new SaleModel();
                        d.DateSale = DateTime.Now;
                        d.TotalSale = Cuenta;
                        d.Discount = EntryDesc;
                        d.WayToPay = WayPay;
                        d.DateDelivery = datet;
                        d.SaleStatus = "En proceso";
                        

                        await App.Database.SaveSale(d);
                        d.Orders = new List<OrderModel>();
                        d.ClientModel = new ClientModel();
                        d.ClientModel = client;

                        foreach (OrderModel obj in Order)
                        {
                            await App.Database.SaveOrder(obj);
                            d.Orders.Add(obj);
                            await App.Database.UpdateRealtionSales(d);
                            //UpdateCantidadesInv(obj);
                        }
                    }                    
                }

                ClearOrder();
                ListProductStock();
            }
            else
            {
                await Shell.Current.DisplayAlert("Hola", "Para guardar una venta primero selecciona un producto", "ok");
            }
        });

        

        //Direcciona a la lista de ventas
        public ICommand RegisterSale => new Command(() =>
        {
            Shell.Current.GoToAsync("GoRegistration");
        });
        public ICommand FilterCommand => new Command<GroupsProductModel>((obj) =>
        {
            GroupsProductModel objr = obj;
            FiltrarProducts(objr);
            //await Shell.Current.DisplayAlert("Aqui", obj.Description, "ok");
        });

        public ICommand CancelCommand => new Command(() =>
        {
            ClearOrder();
            Shell.Current.GoToAsync("//Rini");
        });

        
        #endregion
        //Constructor
        public GoVM()
        {
            ListProductStock();
            ProductGroups();
        }

        #region Metodos
        //Filtrar productos por grupos
        private async void FiltrarProducts(GroupsProductModel objr)
        {
            ListPS.Clear();

            List<ProductModel> lps = await App.Database.ListProduct();
            var resp = (from p in lps
                        where p.GroupProductId == objr.Id
                        select p).ToList();
            if (resp != null)
            {
                foreach (ProductModel tp in resp)
                {
                    ListPS.Add(tp);

                }
            }
        }

        //Cargar los grupos de los productos
        private void ProductGroups()
        {
            var grupos = App.Database.GetGroupsProduct();

            GruposProductos = new ObservableCollection<GroupsProductModel>(grupos.Result);
        }
        private async void PaytoWay()
        {
            if (Cuenta != 0)
            {
                string res = await Shell.Current.DisplayPromptAsync("Por Cobrar", "Nombre de Cliente");
                ClientexC = res;
                WayPay = "por cobrar";
            }

        }
        private void UpdateCantidadesInv(OrderModel obj)
        {


            //actualiza cantidad de productos en poseción
            var id = obj.ProductModelId;

            ProductModel updt = App.Database.Get1Product(id).Result;

            //var NewCantidad = updt.CantProduct - obj.AmountProduct;
            //CalcularPromedioPonderado(updt.CantProduct * updt.PrecioVentaProduct, obj.AmountProduct, obj.ValorUnitario, updt.CantProduct, out double resp);
            //var NewValor = NewCantidad * obj.ValorUnitario;

            KardexModel kardex = new KardexModel
            {
                Date = DateTime.Now,
                //Cantidad = NewCantidad,
                //Valor = NewValor,
                ValorPromPond = obj.ValorUnitario
            };
            App.Database.SaveMovKardex(kardex);

            kardex.ProductModel = updt;

            //updt.CantProduct = NewCantidad;

            App.Database.SaveProduct(updt);
            App.Database.UpdateRelationKardexProduct(kardex);

            //Actualiza cantidad de materia prima
            var aux = obj.ProductModelId;
            var product = App.Database.Get1Product(aux).Result;
            if (product.ListRMModelId != 0)
            {
                var recetaProducto = App.Database.GetListRM(product.ListRMModelId).Result;
                var materialesdeReceta = App.Database.GetAllItems().Result;

                var seleccion = (from m in materialesdeReceta
                                 where m.ListRMModelId == recetaProducto.Id
                                 select m).ToList();

                foreach (var s in seleccion)
                {
                    RawMaterialModel rm = App.Database.GetOneRM(s.RawMaterialModelId).Result;
                    rm.CantidadRM -= s.Amount;
                    App.Database.UpdateRawMaterial(rm);
                }
            }
        }
        private async void AppplyDiscount()
        {
            string res = await Shell.Current.DisplayPromptAsync("Descuento", "Cuanto va a descontar en $?", maxLength: 2, keyboard: Keyboard.Numeric);
            if (!string.IsNullOrEmpty(res))
            {
                EntryDesc += double.Parse(res);
                Cuenta -= double.Parse(res);
            }
        }
        //Lista de productos para vender
        private async void ListProductStock()
        {
            ListPS.Clear();

            List<ProductModel> lps = await App.Database.ListProduct();
            if (lps != null)
            {
                foreach (ProductModel tp in lps)
                {
                    ListPS.Add(tp);

                }
            }

        }
        //Calcula el total de la venta
        private async void SumarOrden(ProductModel selectedPS)
        {
            var res = await Shell.Current.DisplayPromptAsync(selectedPS.NameProduct, "Cuantos desea?", initialValue: "1", maxLength: 2, keyboard: Keyboard.Numeric);

            if (!string.IsNullOrEmpty(res))
            {
                OrderModel pedido = new OrderModel
                {
                    ProductModelId = selectedPS.Id,
                    ValorUnitario = selectedPS.PrecioVentaProduct,
                    AmountProduct = int.Parse(res),
                    Valor = selectedPS.PrecioVentaProduct * int.Parse(res)
                };

                Cuenta += selectedPS.PrecioVentaProduct * int.Parse(res);

                Order.Add(pedido);
                ListProductStock();
            }
        }
        //Limpia la orden
        private void ClearOrder()
        {
            Cuenta = 0.00;
            EntryDesc = 0;
            Order.Clear();
            ClientexC = string.Empty;
            WayPay = string.Empty;
            EsPedido = false;
        } 
        #endregion
    }
}
