﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    [Table("Product")]
    public class ProductModel
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public string LocationProduct { get; set; }
        public string ProductSupplier { get; set; }
        public int MinimalExistence { get; set; }
        public int MaximumExistence { get; set; }
        public string UnidadMedida { get; set; }
        public double PrecioVentaProduct { get; set; }
        public double CostElaboracionProduct { get; set; }



        //public double UtilityProduct { get; set; }


        //Relacion con OrderModel (Ventas)
        [OneToMany("ProductModelId")]
        public List<OrderModel> Orders { get; set; }

        //Relacion con ProductShoppingList (Compras)
        /*[OneToMany("ProductModelId")]
        public List<ProductShoppingList> productShoppingLists { get; set; }
        */

        //Relacion con Kardex
        [OneToMany("IdProduct")]
        public List<KardexModel> Kardices { get; set; }

        //Relacion con ListRMModel
        [ForeignKey(typeof(ListRMModel))]
        public int ListRMModelId { get; set; }
        [OneToOne]
        public ListRMModel ListRMModel { get; set; }

        //Relación con ListWFModel
        [ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }
        [OneToOne]
        public ListWFModel ListWFModel { get; set; }

        //Relacion con ListOCModel
        [ForeignKey(typeof(ListOCModel))]
        public int ListOCModelId { get; set; }
        [OneToOne]
        public ListOCModel ListOCModel { get; set; }

        //Relación con los grupos de productos
        [ForeignKey(typeof(GroupsProductModel))]
        public int GroupProductId { get; set; }
        [ManyToOne]
        public GroupsProductModel Groups { get; set; }

        //Relacion con la mano de obra
        /*[OneToMany]
        public List<PersonalModel> Personals { get; set; }*/
    }
}
