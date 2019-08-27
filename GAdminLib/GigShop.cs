using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GAdminLib
{
    public class GigHouse
    {
        public int ID;
        public string Name;
        public double Price = 0;
    }
    public class GigWeapon
    {
        public int ID;
        public string Name;
        public double Price = 0;
    }
   public class GigVehicle
    {
       public int ID;
       public string Name;
       public double Price = 0;
       public int Model;
    }
   public class GigServer
   {
       public string Name;
       public string Description;
       public IPEndPoint EP;

   }

   public class CommunityInfo
   {
       public List<string> TopTenPlayers = new List<string>();
       public List<string> TopTenRich = new List<string>();
       public int NumberPlayers = 0;
       public List<GigServer> Servers = new List<GigServer>();
   }
}
