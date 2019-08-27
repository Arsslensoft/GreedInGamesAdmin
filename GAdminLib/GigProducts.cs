using System;
using System.Collections.Generic;
using System.Text;

namespace GAdminLib
{
    public enum GProductType : byte
    {
        Voitures= 1,
        Bateaux = 2,
        Avions = 3,
        Semi = 4,
        Maisons = 5,
        Bizz = 6,
        Usines = 7
    }
    
   public class GProduct
    {
       public ulong ProductID = 0;
       public string Name ="";
       public string Description="";
       public ulong Price=0;
       public ushort Quantity=0;
       public GProductType ProductType = GProductType.Avions;
       public DateTime ProductDate = DateTime.Now;

    }

   public class Vehicle
   {
       public string pid; public string Color1; public string Color2; public byte Sale; public byte Featured; public ushort Speed; public ushort Fuel; public byte Places; public byte Tuning; public ulong PrixIG; public byte Water; public string Category;
   }
   public class House
   {
       public string Pieces; public string IdInt; public string Sale; public byte Featured; public string Ville; public ushort Popularity; public string Garage; public string GarageMap; public string Wall; public string WallMap; public ulong PrixIG; public string Jardin; public string Piscine; public string Category;
   }
   public class Bizz
   {
       public string Stock; public string Sale; public byte Featured; public string Ville; public ushort Popularity; public string Depot; public string DepotMap; public ulong PrixIG; public string Category;
   }

}
