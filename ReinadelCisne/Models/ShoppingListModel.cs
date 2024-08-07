﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ShoppingListModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }    //Cantidad comprada 
        public double ValorUnitario { get; set; }     //Unidades compradas
        public double TotalCost { get; set; }

        [ForeignKey(typeof(ShoppingModel))]
        public int ShoppingModelId { get; set; }
        [ManyToOne]
        public ShoppingModel ShoppingModel { get; set; }

        [ForeignKey(typeof(KardexRMModel))]
        public int KardexRMModelId { get; set; }
        [ManyToOne]
        public KardexRMModel KardexRMModel { get; set; }
    }
}
