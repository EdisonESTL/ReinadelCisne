using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using ReinadelCisne.Models;
using System.Threading.Tasks;
using SQLiteNetExtensionsAsync.Extensions;
using System.Linq;
using System.Data;

namespace ReinadelCisne.Services
{
    public class DataBase
    {
        private SQLiteAsyncConnection _database;
        public DataBase(string dbPath)
        {
            var options = new SQLiteConnectionString(dbPath, true, key: "222706");
            _database = new SQLiteAsyncConnection(options);
            _database.CreateTableAsync<ProductModel>();
            _database.CreateTableAsync<SaleModel>();
            _database.CreateTableAsync<OrderModel>();
            _database.CreateTableAsync<RawMaterialModel>();
            _database.CreateTableAsync<ListRMModel>();
            _database.CreateTableAsync<WorkForceModel>();
            _database.CreateTableAsync<ListWFModel>();
            _database.CreateTableAsync<OtherCostModel>();
            _database.CreateTableAsync<ListOCModel>();
        }

        //Procesos de Productos
        public Task<int> SaveProduct(ProductModel product)
        {
            if(product.Id != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }

        public Task UpdateRelationsRM(ProductModel product)
        {
            return _database.UpdateWithChildrenAsync(product);
        }
        public Task<List<ProductModel>> ListProduct()
        {
            var g = _database.GetAllWithChildrenAsync<ProductModel>();
            return g;
        }

        public Task DeleteProduct(ProductModel obj)
        {
            return _database.DeleteAsync(obj);
        }

        public Task<ProductModel> Get1Product(int idproduct)
        {
            return _database.GetWithChildrenAsync<ProductModel>(idproduct);
        }
    
        //Procesos de Ventas
        public Task<int> SaveSale(SaleModel sale)
        {
            if (sale.Id != 0)
            {
                return _database.UpdateAsync(sale);
            }
            else
            {
                return _database.InsertAsync(sale);
            }
        }

        public Task<List<SaleModel>> ListSales()
        {
            return _database.GetAllWithChildrenAsync<SaleModel>();
        }

        public Task<List<OrderModel>> ListOrders()
        {
            return _database.GetAllWithChildrenAsync<OrderModel>();
        }

        public Task DeleteSale(SaleModel sale)
        {
            return _database.DeleteAsync(sale, recursive: true);
        }

        public Task SaveOrder(OrderModel order)
        {
            return _database.InsertAsync(order);
        }

        public Task UpdateRealtionSales(SaleModel sale)
        {
            return _database.UpdateWithChildrenAsync(sale);
        }

        /*public Task<SaleModel> GetVentasDate(DateTime dateI, DateTime dateF)
        {
            var Sales = ListSales();
            var Orders = ListOrders();
            var Products = ListProduct();
            var fg = from o in Orders
                     join s in Sales on o equals
                     join p in Products
                     where s.DateSale >= dateI && s.DateSale <= dateF
                     select s;

            return fg;
        }*/
        //Procesos de Materia prima
        public Task<int> SaveListRM(ListRMModel listRawMaterial)
        {
            if (listRawMaterial.Id != 0)
            {
                
                return _database.UpdateAsync(listRawMaterial);
            }
            else
            {
                var m = _database.InsertAsync(listRawMaterial);
                
                return m;
            }
            
        }
        public Task<ListRMModel> GetListRM(int i)
        {
            return _database.GetWithChildrenAsync<ListRMModel>(i);
            
        }

        public Task<List<ListRMModel>> GetV()
        {
            return _database.GetAllWithChildrenAsync<ListRMModel>();
        }

        public Task<int> SaveRawMaterial(RawMaterialModel rawMaterial)
        {
            if (rawMaterial.Id != 0)
            {
                return _database.UpdateAsync(rawMaterial);
            }
            else
            {
                return _database.InsertAsync(rawMaterial);
            }
        }
                
        public Task<List<RawMaterialModel>> GetMR()
        {
            return _database.Table<RawMaterialModel>().ToListAsync();
        }

        public Task UpdateListRM(ListRMModel listRMModel)
        {
            var t = _database.UpdateWithChildrenAsync(listRMModel);

            return t;
        }

        public Task<int> DeleteRawMaterial(RawMaterialModel rawMaterial)
        {
            return _database.DeleteAsync(rawMaterial);
        }

        //Metodos de Mano de Obra (Work Force)
        public Task<int> SaveWorkForce(WorkForceModel workForce)
        {
            if (workForce.Id != 0)
            {

                return _database.UpdateAsync(workForce);
            }
            else
            {
                var m = _database.InsertAsync(workForce);

                return m;
            }
        }

        public Task<int> SaveListWF(ListWFModel listWF)
        {
            if (listWF.Id != 0)
            {

                return _database.UpdateAsync(listWF);
            }
            else
            {
                var m = _database.InsertAsync(listWF);

                return m;
            }
        }

        public Task UpdateListWF(ListWFModel listWF)
        {
            var t = _database.UpdateWithChildrenAsync(listWF);

            return t;
        }

        public Task<List<ListWFModel>> GetListsWF()
        {
            return _database.GetAllWithChildrenAsync<ListWFModel>();
        }

        public Task<ListWFModel> GetListWF(int i)
        {
            return _database.GetWithChildrenAsync<ListWFModel>(i);
        }
        public Task<int> DeleteWorkcForce(WorkForceModel workForce)
        {
            return _database.DeleteAsync(workForce);
        }
        //Funciones Otros Costos
        public Task<int> SaveOtherCost(OtherCostModel otherCost)
        {
            if (otherCost.Id != 0)
            {

                return _database.UpdateAsync(otherCost);
            }
            else
            {
                var m = _database.InsertAsync(otherCost);

                return m;
            }
        }

        public Task<int> SaveListOC(ListOCModel listOC)
        {
            if (listOC.Id != 0)
            {

                return _database.UpdateAsync(listOC);
            }
            else
            {
                var m = _database.InsertAsync(listOC);

                return m;
            }
        }

        public Task UpdateListOC(ListOCModel listOC)
        {
            return _database.UpdateWithChildrenAsync(listOC);            
        }

        public Task<List<ListOCModel>> GetListsOC()
        {
            return _database.GetAllWithChildrenAsync<ListOCModel>();
        }

        public Task<ListOCModel> GetListOC(int i)
        {
            return _database.GetWithChildrenAsync<ListOCModel>(i);
        }

        public Task<int> DeleteOtherCost(OtherCostModel otherCost)
        {
            return _database.DeleteAsync(otherCost);
        }
    }
}
