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
            _database.CreateTableAsync<ItemsListRMModel>();
            _database.CreateTableAsync<WorkForceModel>();
            _database.CreateTableAsync<ListWFModel>();
            _database.CreateTableAsync<OtherCostModel>();
            _database.CreateTableAsync<ListOCModel>();
            _database.CreateTableAsync<ShoppingModel>();
            _database.CreateTableAsync<ShoppingListModel>();
            _database.CreateTableAsync<ActivityModel>();
            _database.CreateTableAsync<ClientModel>();
            _database.CreateTableAsync<UserModel>();
            _database.CreateTableAsync<KardexModel>();
            _database.CreateTableAsync<KardexRMModel>();
            _database.CreateTableAsync<ProductShoppingList>();
            _database.CreateTableAsync<ProductShoppingModel>();
            _database.CreateTableAsync<GroupsRMModel>();
            _database.CreateTableAsync<UMedidasRMModel>();
        }

        //Proceso Kardex productos
        public Task<int> SaveMovKardex(KardexModel kardex)
        {
            if(kardex.Id != 0)
            {
                return Task<int>.Run(() =>
                {
                    int ctr;
                    _database.UpdateAsync(kardex);
                    ctr = 2;
                    return ctr;
                });
            }
            else
            {
                return _database.InsertAsync(kardex);
            }
        }

        public Task UpdateRelationKardexProduct(KardexModel kardex)
        {
            return _database.UpdateWithChildrenAsync(kardex);
        }
        public Task<List<KardexModel>> GetKardices(ProductModel product)
        {
            var fg = _database.Table<KardexModel>();
            //var fg = _database.Table<KardexModel>().OrderByDescending(x=>x.Id);
            var resp = (from a in fg
                       where a.IdProduct == product.Id
                        select a).ToListAsync();
            return resp;
        }
        
        public Task<KardexModel> GetFirstKardex(ProductModel Producto)
        {
            var lis = _database.Table<KardexModel>().OrderByDescending(x => x.Date).Where(x => x.IdProduct == Producto.Id).FirstOrDefaultAsync();
            /*var resp = (from a in lis
                       where a.IdProduct == Producto.Id
                       select a).FirstOrDefaultAsync();*/
            return lis;
        }
        
        //Proceso Usuario
        public Task<UserModel> GetUser()
        {
            return _database.Table<UserModel>().FirstOrDefaultAsync();
        }
        public Task<int> SaveUser(UserModel user)
        {
            return _database.InsertAsync(user);
        }
        public bool ValidarUsuario(string ci, string contrasenia)
        {
            //var gf = _dataBase.QueryAsync<UserModel>("SELECT NameUser FROM UserModel WHERE CiUser = ? AND PasswordUser = ?", ci, contrasenia);

            var data = _database.Table<UserModel>().FirstOrDefaultAsync(t => t.MailUser == ci && t.PasswordUser == contrasenia);
            data.Wait();
            var result = data.Result;
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //Procesos Clientes
        public Task<int> SaveClients(ClientModel client)
        {
            if (client.Id != 0)
            {
                return _database.UpdateAsync(client);
            }
            else
            {
                return _database.InsertAsync(client);
            }
        }
        
        //Procesos Actividades
        public Task<int> SaveActivity(ActivityModel activity)
        {
            if (activity.Id != 0)
            {
                return _database.UpdateAsync(activity);
            }
            else
            {
                return _database.InsertAsync(activity);
            }
        }
        public Task<List<ActivityModel>> ListActivity()
        {
            return _database.Table<ActivityModel>().ToListAsync();
        }
        public Task<int> DeleteActivity(ActivityModel activity)
        {
            return _database.DeleteAsync(activity);
        }
        public Task<ActivityModel> getActivity(int id)
        {
            return _database.GetWithChildrenAsync<ActivityModel>(id);
        }

        //Procesos de Compras - Materia Prima
        public Task<int> SaveShopping(ShoppingModel shopping)
        {
            if (shopping.Id != 0)
            {
                return _database.UpdateAsync(shopping);
            }
            else
            {
                return _database.InsertAsync(shopping);
            }
        }
        public Task UpdateRelationsSh(ShoppingModel shopping)
        {
            return _database.UpdateWithChildrenAsync(shopping);
        }
        public Task<ShoppingModel> GetShopping(int id)
        {
            return _database.GetWithChildrenAsync<ShoppingModel>(id);
        }
        public Task<int> SaveListShop(ShoppingListModel shoppingList)
        {
            if (shoppingList.Id != 0)
            {
                return _database.UpdateAsync(shoppingList);
            }
            else
            {
                return _database.InsertAsync(shoppingList);
            }
        }
        public Task UpdateRelationsListShop(ShoppingListModel shoppingList)
        {
            return _database.UpdateWithChildrenAsync(shoppingList);
        }
        public Task<ShoppingListModel> GetListShopping(int id)
        {
            return _database.GetWithChildrenAsync<ShoppingListModel>(id);
        }
        public Task<List<ShoppingModel>> ListShopping()
        {
            return _database.GetAllWithChildrenAsync<ShoppingModel>();
        }
        public Task<List<ShoppingListModel>> ListShoppingList()
        {
            return _database.GetAllWithChildrenAsync<ShoppingListModel>();
        }

        //Procesos Compras productos
        public Task<int> SaveShoppingProduct(ProductShoppingModel shopping)
        {
            if (shopping.Id != 0)
            {
                return _database.UpdateAsync(shopping);
            }
            else
            {
                return _database.InsertAsync(shopping);
            }
        }

        //Procesos de Productos
        public Task<int> SaveProduct(ProductModel product)
        {
            if(product.Id != 0)
            {

                return Task.Run(() =>
                {
                    int ctr;
                    _database.UpdateAsync(product);
                    ctr = 1;
                    return ctr;
                });
            }
            else
            {
                return Task.Run(() =>
                {
                    int ctr;
                    _database.InsertAsync(product);
                    ctr = 2;
                    return ctr;
                });
                //var g =_database.InsertAsync(product);
                /*KardexModel kardex = new KardexModel
                {
                    ValorUnitario = product.PrecioVentaProduct,
                    Cantidad = product.CantProduct,
                    ValorPromPond = product.PrecioVentaProduct
                };

                SaveMovKardex(kardex);

                kardex.ProductModel = product;
                
                UpdateRelationKardexProduct(kardex);*/
                //return g;
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
        public Task<int> GetTotalProducts()
        {
            return _database.Table<ProductModel>().CountAsync();
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
        {//se aumento if.. f21/07 1:01
            if (order.Id != 0)
            {
                return _database.UpdateAsync(order);
            }
            else
            {
                return _database.InsertAsync(order);
            }
            
        }
        public Task UpdateRealtionSales(SaleModel sale)
        {
            return _database.UpdateWithChildrenAsync(sale);
        }
        public Task<SaleModel> GetSale(int idsale)
        {
            return _database.GetWithChildrenAsync<SaleModel>(idsale);
        }
        public Task<SaleModel> GetOrder(int idOrder)
        {
            return _database.GetWithChildrenAsync<SaleModel>(idOrder);
        }
        //Procesos de Materia prima
        public Task<int> SaveRawMaterial(RawMaterialModel rawMaterial)
        {
            /*var fb = _database.Table<RawMaterialModel>().ToListAsync();

            var query = (from a in fb.Result
                        where a.NameRM == rawMaterial.NameRM & a.UnitMeasurementRM == rawMaterial.UnitMeasurementRM
                        select a).FirstOrDefault();*/

            if (rawMaterial.Id != 0)
            {
                var nn = _database.GetWithChildrenAsync<RawMaterialModel>(rawMaterial.Id).Result;
                if (rawMaterial.CostoRM != nn.CostoRM)
                {
                    calculatePriceponderd(rawMaterial);
                }
                return Task.FromResult(0);
            }
            else
            {
                //rawMaterial.AmountRM = 0;
                return _database.InsertAsync(rawMaterial);
            }
        }
        public Task<int> SaveItemListRM (ItemsListRMModel itemsListRM)
        {
            if(itemsListRM.Id != 0)
            {
                return _database.UpdateAsync(itemsListRM);
            } else
            {
                return _database.InsertAsync(itemsListRM);
            }
        }
        public Task UpdateRelationItemRM(ItemsListRMModel itemsListRM)
        {
            return _database.UpdateWithChildrenAsync(itemsListRM);
        }
        private async void calculatePriceponderd(RawMaterialModel rawMaterial)
        {
            var inPos = _database.GetWithChildrenAsync<RawMaterialModel>(rawMaterial.Id).Result;
            var cpp = ((inPos.CantidadRM * inPos.CostoRM) + (rawMaterial.CantidadRM * rawMaterial.CostoRM)) / (inPos.CantidadRM + rawMaterial.CantidadRM);
            inPos.CostoRM = (float)cpp;
            await _database.UpdateAsync(inPos);
        }
        public Task<int> UpdateRawMaterial(RawMaterialModel rawMaterial)
        {
            if (rawMaterial.Id != 0)
            {
                return _database.UpdateAsync(rawMaterial);
            } else
            {
                return Task.FromResult(0);
            }
        }
        public Task UpdateRealtionRawMat(RawMaterialModel rawMaterial)
        {
            return _database.UpdateWithChildrenAsync(rawMaterial);
        }
        public Task<int> SaveListRM(ListRMModel listRawMaterial)
        {
            if (listRawMaterial.Id != 0)
            {
                
                return _database.UpdateAsync(listRawMaterial);
            }
            else
            {
                return _database.InsertAsync(listRawMaterial);
            }
            
        }
        public Task<List<ItemsListRMModel>> GetAllItems()
        {
            return _database.GetAllWithChildrenAsync<ItemsListRMModel>();
        }
        public Task<ListRMModel> GetListRM(int i)
        {
            return _database.GetWithChildrenAsync<ListRMModel>(i);  
        }
        public Task<RawMaterialModel> GetOneRM(int i)
        {
            return _database.GetWithChildrenAsync<RawMaterialModel>(i);
        }
        public Task<List<ListRMModel>> GetAllRMList()
        {
            return _database.GetAllWithChildrenAsync<ListRMModel>();
        }
        public Task<List<RawMaterialModel>> GetMR()
        {
            return _database.GetAllWithChildrenAsync<RawMaterialModel>();
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
        public void UpdateInvRM(List<RawMaterialModel> rawMaterials)
        {
            foreach (var raw in rawMaterials)
            {
                var query = (from a in _database.Table<RawMaterialModel>()
                             where a.NameRM == raw.NameRM & a.UnitMeasurementRM == raw.UnitMeasurementRM
                             select a).FirstOrDefaultAsync().Result;
               
                query.CantidadRM += raw.CantidadRM;
                _database.UpdateAsync(query);
            }
        }

        //procesos de Kardex de materia prima
        public Task<int> SaveKardesxRM(KardexRMModel kardexRM)
        {
            if(kardexRM.Id != 0)
            {
                return _database.UpdateAsync(kardexRM);
            }
            else 
            {
                return _database.InsertAsync(kardexRM);
            }
        }

        public Task UpdateRelationsKardexRM(KardexRMModel kardexModel)
        {
            return _database.UpdateWithChildrenAsync(kardexModel);
        }

        //Procesos Grupos de Materia Prima
        public Task<int> SaveGroupRM(GroupsRMModel group)
        {
            if(group.Id != 0)
            {
                return _database.UpdateAsync(group);
            }
            else
            {
                return _database.InsertAsync(group);
            }
        }

        public Task<List<GroupsRMModel>> GetGroupRM()
        {
            return _database.Table<GroupsRMModel>().ToListAsync();
        }

        public Task<GroupsRMModel> GetESpecificGroup(GroupsRMModel rMModel)
        {
            //return _database.Table<GroupsRMModel>().Where(x => x.Id == rMModel.Id).FirstOrDefaultAsync();
            return _database.GetWithChildrenAsync<GroupsRMModel>(rMModel.Id);
        }

        //Procesos Unidades de medida de Materia prima
        public Task<int> SaveUMedidaRM(UMedidasRMModel uMedida)
        {
            if(uMedida.Id != 0) 
            {
                return _database.UpdateAsync(uMedida);
            }
            else
            {
                return _database.InsertAsync(uMedida);
            }
        }

        public Task<List<UMedidasRMModel>> GetUMedidadRM()
        {
            return _database.Table<UMedidasRMModel>().ToListAsync();
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
