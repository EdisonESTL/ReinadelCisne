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
            _database.CreateTableAsync<GroupsProductModel>();
            _database.CreateTableAsync<SaleModel>();
            _database.CreateTableAsync<OrderModel>();
            _database.CreateTableAsync<RawMaterialModel>();
            _database.CreateTableAsync<ListRMModel>();
            _database.CreateTableAsync<ItemsListRMModel>();
            _database.CreateTableAsync<WorkForceModel>();
            _database.CreateTableAsync<ListWFModel>();
            _database.CreateTableAsync<PersonalModel>();
            _database.CreateTableAsync<PaymentsModel>();
            _database.CreateTableAsync<OtherCostModel>();
            _database.CreateTableAsync<ListOCModel>();
            _database.CreateTableAsync<ShoppingModel>();
            _database.CreateTableAsync<ShoppingListModel>();
            _database.CreateTableAsync<ActivityModel>();
            _database.CreateTableAsync<ClientModel>();
            _database.CreateTableAsync<UserModel>();
            _database.CreateTableAsync<KardexModel>();
            _database.CreateTableAsync<KardexRMModel>();
            _database.CreateTableAsync<SaldosRMModel>();
            _database.CreateTableAsync<ProductShoppingList>();
            _database.CreateTableAsync<ProductShoppingModel>();
            _database.CreateTableAsync<GroupsRMModel>();
            _database.CreateTableAsync<UMedidasRMModel>();
            _database.CreateTableAsync<CostosConstitucionModel>();
            _database.CreateTableAsync<FixedAssetsModel>();
            _database.CreateTableAsync<GroupsFixedAssetsModel>();
            _database.CreateTableAsync<ListFixedAssetsXproductModel>();
            _database.CreateTableAsync<ListFAxProductModel>();
            _database.CreateTableAsync<OrderProduccionModel>();
            _database.CreateTableAsync<SaldosKardexProductModel>();
            _database.CreateTableAsync<UserPhotosModel>();
            _database.CreateTableAsync<ImagesAppModel>();
        }
        //Procesos guardar fotos
        public Task<int> SaveImageApp(ImagesAppModel images)
        {
            if (images.Id != 0)
            {
                return _database.UpdateAsync(images);
            }
            else
            {
                return _database.InsertAsync(images);
            }
        }
        public Task<int> SaveImage(UserPhotosModel Image)
        {
            if(Image.Id != 0)
            {
                return _database.UpdateAsync(Image);
            }
            else
            {
                return _database.InsertAsync(Image);
            }
        }
        /*public Task<UserPhotosModel> GetImageUser(UserModel user)
        {
            return _database.Table<UserPhotosModel>().FirstOrDefaultAsync(u => u.Name = user.NameUser);
        }*/
        //Proceso Usuario
        public Task<UserModel> GetUser()
        {
            return _database.Table<UserModel>().FirstOrDefaultAsync();
        }
        public Task<int> SaveUser(UserModel user)
        {
            if(user.Id != 0)
            {
                return _database.UpdateAsync(user);

            }
            else
            {
                return _database.InsertAsync(user);

            }
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
                

        //Procesos de Productos
        #region Productos
        /*GuardarProducto*/
        public Task<int> SaveProduct(ProductModel product)
        {
            if (product.Id != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);                
            }
        }
        /*Actualizar Relaciones de Producto*/
        public Task UpdateRelationsProduct(ProductModel product)
        {
            return _database.UpdateWithChildrenAsync(product);
        }
        /*Listar Productos*/
        public Task<List<ProductModel>> ListProduct()
        {
            var g = _database.GetAllWithChildrenAsync<ProductModel>();
            return g;
        }
        /*Borrar Producto*/
        public Task DeleteProduct(ProductModel obj)
        {
            return _database.DeleteAsync(obj);
        }
        /*Obtener un producto*/
        public Task<ProductModel> Get1Product(int idproduct)
        {
            return _database.GetWithChildrenAsync<ProductModel>(idproduct);
        }
        /*Total de productos*/
        public Task<int> GetTotalProducts()
        {
            return _database.Table<ProductModel>().CountAsync();
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

        //Proceso Kardex productos

        /*Guardar movimineto en el kardex*/
        public Task<int> SaveMovKardex(KardexModel kardex)
        {
            if (kardex.Id != 0)
            {
                return _database.UpdateAsync(kardex);
            }
            else
            {
                return _database.InsertAsync(kardex);
            }
        }
        /*Actualizar relaciones kardex*/
        public Task UpdateRelationKardexProduct(KardexModel kardex)
        {
            return _database.UpdateWithChildrenAsync(kardex);
        }
        //Obtener Kardex
        public Task<KardexModel> Get1KardexProduct(int kardexModel)
        {
            return _database.GetWithChildrenAsync<KardexModel>(kardexModel);
        }
        /*Obtener listado de kardex de un producto*/
        public Task<KardexModel> GetKardices(ProductModel product)
        {
            var fg = _database.Table<KardexModel>();
            //var fg = _database.Table<KardexModel>().OrderByDescending(x=>x.Id);
            var resp = (from a in fg
                        where a.ProductModel.Id == product.Id
                        select a).FirstOrDefaultAsync();
            return resp;
        }
        public Task<List<KardexModel>> GetAllKardxProduct()
        {
            return _database.GetAllWithChildrenAsync<KardexModel>();
        }
        /*Obtener el primer movimiento kardex de un producto*/
        public Task<KardexModel> GetFirstKardex(ProductModel Producto)
        {
            var lis = _database.Table<KardexModel>().OrderByDescending(x => x.Date).Where(x => x.IdProduct == Producto.Id).FirstOrDefaultAsync();
            /*var resp = (from a in lis
                       where a.IdProduct == Producto.Id
                       select a).FirstOrDefaultAsync();*/
            return lis;
        }
        public Task<int> SaveSaldoKardxPr(SaldosKardexProductModel kardex)
        {
            if (kardex.Id != 0)
            {
                return _database.UpdateAsync(kardex);
            }
            else
            {
                return _database.InsertAsync(kardex);
            }
        }

        public Task<List<SaldosKardexProductModel>> GetAllSaldosKrdxProd()
        {
            return _database.GetAllWithChildrenAsync<SaldosKardexProductModel>();
        }

        public Task UpdateRelationsSaldosKrdxProd(SaldosKardexProductModel kardex)
        {
            return _database.UpdateWithChildrenAsync(kardex);
        }
        //Procesos Grupos Productos

        /*Guardar Grupo de productos*/
        public Task<int> SaveGRoupProduct(GroupsProductModel groups)
        {
            if(groups.Id != 0)
            {
                return _database.UpdateAsync(groups);
            }
            else
            {
                return _database.InsertAsync(groups);
            }
        }

        /*Obtener lista de grupos de productos*/
        public Task<List<GroupsProductModel>> GetGroupsProduct()
        {
            return _database.Table<GroupsProductModel>().ToListAsync();
        }

        /*Eliminar grupo de productos*/
        public Task<int> DeleteGroupProduct(GroupsProductModel groups)
        {
            return _database.DeleteAsync(groups);
        }
        #endregion

        //Procesos de Ventas
        #region Ventas
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
        public Task<List<OrderModel>> GetAllOrders()
        {
            return _database.GetAllWithChildrenAsync<OrderModel>();
        }
        public Task UpdateRelationOrderModel(OrderModel order)
        {
            return _database.UpdateWithChildrenAsync(order);
        }
        #endregion
        
        //Procesos de Materia prima
        #region Materia Prima
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
        public Task<int> SaveItemListRM(ItemsListRMModel itemsListRM)
        {
            if (itemsListRM.Id != 0)
            {
                return _database.UpdateAsync(itemsListRM);
            }
            else
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
            }
            else
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
        public Task<List<ItemsListRMModel>> GetItemsListRMxListRm(ListRMModel listRM)
        {
            var AllItems =  GetAllItems().Result;
            var ListItems = (from a in AllItems
                            where a.ListRMModel.Id == listRM.Id
                            select a).ToList();
            return Task.Run(() => ListItems);
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
        public Task<RawMaterialModel> GetMaterialxKardex(KardexRMModel kardexRM)
        {
            var allKardex = GetMR().Result;
            var kardEleg = allKardex.Where(x => x.Id == kardexRM.IdRawMaterial).FirstOrDefault();
            return Task.Run(() => kardEleg);
        }
        //procesos de Kardex de materia prima
        public Task<int> SaveKardesxRM(KardexRMModel kardexRM)
        {
            if (kardexRM.Id != 0)
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
        public Task<List<KardexRMModel>> GetKardexsRM()
        {
            return _database.GetAllWithChildrenAsync<KardexRMModel>();
        }
        public Task<KardexRMModel> GetKardexRM(KardexRMModel kardexRM)
        {
            return _database.GetWithChildrenAsync<KardexRMModel>(kardexRM.Id);
        }
        public Task<KardexRMModel> GetKardexXRM(RawMaterialModel rawMaterial)
        {
            var krs = _database.Table<KardexRMModel>();

            var kr = (from k in krs
                      where k.IdRawMaterial == rawMaterial.Id
                      select k).FirstOrDefaultAsync();
            return kr;
        }

        //Procesos Grupos de Materia Prima
        public Task<int> SaveGroupRM(GroupsRMModel group)
        {
            if (group.Id != 0)
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
        public Task UpdateRelationGroupRM(GroupsRMModel groupsRM)
        {
            return _database.UpdateWithChildrenAsync(groupsRM);
        }
        //Procesos Unidades de medida de Materia prima
        public Task<int> SaveUMedidaRM(UMedidasRMModel uMedida)
        {
            if (uMedida.Id != 0)
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
        public Task UpdateRelationUMedidasRM(UMedidasRMModel uMedidasRM)
        {
            return _database.UpdateWithChildrenAsync(uMedidasRM);
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
        
        //Procesos Saldos RM
        public Task<int> SaveSaldoRM(SaldosRMModel saldosRM)
        {
            if(saldosRM.Id != 0)
            {
                return _database.UpdateAsync(saldosRM);
            }
            else
            {
                return _database.InsertAsync(saldosRM);
            }
        }

        public Task UpdateRelationsSaldosRM(SaldosRMModel saldosRM)
        {
            return _database.UpdateWithChildrenAsync(saldosRM);
        }

        public Task<List<SaldosRMModel>> GetAllSaldos()
        {
            return _database.GetAllWithChildrenAsync<SaldosRMModel>();
        }

        public Task<List<SaldosRMModel>> GetSaldosxKardex(KardexRMModel kardexRM)
        {
            var saldoss = _database.Table<SaldosRMModel>();

            var cons = (from a in saldoss
                        where a.KardexRMModelId == kardexRM.Id
                        select a).ToListAsync();
            return cons;
        }
        #endregion

        /*Metodos de Mano de Obra (Work Force)*/
        #region Mano de Obra
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
        public Task UpdateRelationWF(WorkForceModel workForce)
        {
            return _database.UpdateWithChildrenAsync(workForce);
        }
        public Task<List<WorkForceModel>> GetAllWorkForce()
        {
            return _database.GetAllWithChildrenAsync<WorkForceModel>();
        }
        public Task<int> SaveListWF(ListWFModel listWF)
        {
            if (listWF.Id != 0)
            {

                return _database.UpdateAsync(listWF);
            }
            else
            {
                return _database.InsertAsync(listWF);
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
        public Task<int> SavePersonal(PersonalModel personal)
        {
            if (personal.Id != 0)
            {

                return _database.UpdateAsync(personal);
            }
            else
            {
                return _database.InsertAsync(personal);                
            }
        }
        public Task<int> DeletePersonal(PersonalModel personal)
        {
            return _database.DeleteAsync(personal);
        }
        public Task UpdateRelationsPersonal(PersonalModel personal)
        {
            return _database.UpdateWithChildrenAsync(personal);
        }
        public Task UpdateRelationsPayments(PaymentsModel payments)
        {
            return _database.UpdateWithChildrenAsync(payments);
        }
        public Task<int> SavePayments(PaymentsModel payments)
        {
            if (payments.Id != 0)
            {

                return _database.UpdateAsync(payments);
            }
            else
            {
                return _database.InsertAsync(payments);
            }
        }
        public Task<int> DeletePayment(PaymentsModel payments)
        {
            return _database.DeleteAsync(payments);
        }
        public Task<WorkForceModel> GetWorkForce(int workForce)
        {
            return _database.GetWithChildrenAsync<WorkForceModel>(workForce);
        }

        public Task<PersonalModel> GetPersonal(int personal)
        {
            return _database.GetWithChildrenAsync<PersonalModel>(personal);
        }

        /*public Task<WorkForceModel> GetPuestoxObrero(PersonalModel personal)
        {
            var perfiles = GetAllWorkForce().Result;
            //var eleg = perfiles.Where(x => x.)
        }*/
        #endregion

        //Funciones Otros Costos o Costos Indirectos
        #region Otros Costos
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
        public Task UpdateRelationOC(OtherCostModel otherCost)
        {
            return _database.UpdateWithChildrenAsync(otherCost);
        }
        public Task<List<OtherCostModel>> GetAllOtherCost()
        {
            return _database.GetAllWithChildrenAsync<OtherCostModel>();
        }
        public Task<int> SaveListOC(ListOCModel listOC)
        {
            if (listOC.Id != 0)
            {

                return _database.UpdateAsync(listOC);
            }
            else
            {
                return _database.InsertAsync(listOC);
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
        #endregion

        //Funciones costos constitución
        #region Costos constitucion
        public Task<int> SaveCostosCostitucion(CostosConstitucionModel costosConstitucion)
        {
            if (costosConstitucion.Id != 0)
            {
                return _database.UpdateAsync(costosConstitucion);
            }
            else
            {
                return _database.InsertAsync(costosConstitucion);
            }
        }

        public Task<int> DeleteCostosConstitucion(CostosConstitucionModel costosConstitucion)
        {
            return _database.DeleteAsync(costosConstitucion);
        }

        public Task<CostosConstitucionModel> Get1CostosConstitucion(CostosConstitucionModel costosConstitucion)
        {
            return _database.GetWithChildrenAsync<CostosConstitucionModel>(costosConstitucion.Id);
        }

        public Task<List<CostosConstitucionModel>> GetAllCostosConstitucion()
        {
            return _database.GetAllWithChildrenAsync<CostosConstitucionModel>();
        }
        public Task<List<CostosConstitucionModel>> GetAllCostosConstitucion1()
        {
            return _database.Table<CostosConstitucionModel>().ToListAsync();
        }
        #endregion

        //Funciones Fixed Assets
        #region Fixed Assets
        public Task<int> SaveFixedAssets(FixedAssetsModel fixedAssets)
        {
            if (fixedAssets.Id != 0)
            {
                return _database.UpdateAsync(fixedAssets);
            }
            else
            {
                return _database.InsertAsync(fixedAssets);
            }
        }
        public Task<int> DeleteFixedAssets(FixedAssetsModel fixedAssets)
        {
            return _database.DeleteAsync(fixedAssets);
        }
        public Task<FixedAssetsModel> Get1FixedAsset(FixedAssetsModel fixedAssets)
        {
            return _database.GetWithChildrenAsync<FixedAssetsModel>(fixedAssets.Id);
        }
        public Task<List<FixedAssetsModel>> GetAllFixedAssets()
        {
            return _database.GetAllWithChildrenAsync<FixedAssetsModel>();
        }
        //Funciones grupos Fixed Assets
        public Task<int> SaveGroupsFixedAsset(GroupsFixedAssetsModel fixedAssets)
        {
            if (fixedAssets.Id != 0)
            {
                return _database.UpdateAsync(fixedAssets);
            }
            else
            {
                return _database.InsertAsync(fixedAssets);
            }
        }
        public Task<int> DeleteGroupsFixedAsset(GroupsFixedAssetsModel fixedAssets)
        {
            return _database.DeleteAsync(fixedAssets);
        }
        public Task<List<GroupsFixedAssetsModel>> GetAllGroupsFixedAssets()
        {
            return _database.Table<GroupsFixedAssetsModel>().ToListAsync();
        }
        public Task UpdateRelationFA(GroupsFixedAssetsModel fixedAssets)
        {
            return _database.UpdateWithChildrenAsync(fixedAssets);
        }
        public Task<List<FixedAssetsModel>> GetAssetsxGroup(string groupsFixed)
        {
            var FAS = _database.Table<FixedAssetsModel>();

            var kr = (from f in FAS
                      where f.Grupo == groupsFixed
                      select f).ToListAsync();
            return kr;
        }
        //Funciones Lista de FixedAssets
        public Task<int> SaveListSIxedAssetsProd(ListFixedAssetsXproductModel listFixed)
        {
            if(listFixed.Id != 0)
            {
                return _database.UpdateAsync(listFixed);
            }
            else
            {
                return _database.InsertAsync(listFixed);
            }
        }
        public Task UpdateRelationListFAxProd(ListFixedAssetsXproductModel listFixed)
        {
            return _database.UpdateWithChildrenAsync(listFixed);
        }
        public Task<int> SaveListFAxProd(ListFAxProductModel listFixed)
        {
            if (listFixed.Id != 0)
            {
                return _database.UpdateAsync(listFixed);
            }
            else
            {
                return _database.InsertAsync(listFixed);
            }
        }
        public Task<ListFAxProductModel> Get1ListFA(int listFixed)
        {
            return _database.GetWithChildrenAsync<ListFAxProductModel>(listFixed);
        }
        #endregion

        //Funciones Ordenes de produccion 
        public Task<int> SaveOrderProduccion(OrderProduccionModel order)
        {
            if (order.Id != 0)
            {
                return _database.UpdateAsync(order);
            }
            else
            {
                return _database.InsertAsync(order);
            }
        }
        public Task<List<OrderProduccionModel>> GetAllOrdersProduccion()
        {
            return _database.GetAllWithChildrenAsync<OrderProduccionModel>();
        }

        public Task<OrderProduccionModel> GetOrderProduccion(int orderID)
        {
            return _database.GetWithChildrenAsync<OrderProduccionModel>(orderID);
        }

        public Task UpdateRelationsOrderProduccion(OrderProduccionModel orderProduccion)
        {
            return _database.UpdateWithChildrenAsync(orderProduccion);
        }
    }
}
