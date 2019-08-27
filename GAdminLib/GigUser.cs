using System;
using System.Collections.Generic;
using System.Text;

namespace GAdminLib
{
    public enum CommandStatus : byte
    {
        WAITING = 0,
        VALIDATED = 1
    }
    public class GSkin
    {
        public string Name;
        public int ID;
        public int Price;
    }
    public class GigCommand
    {
        public string SID;
        public string Username;
        public string GTAUsername;
        public string Email;
        public string ContactEmail;
        public string Name;
        public string Pack;
        public DateTime TimeStamp;
        public int Price = 0;
        public string Option;
        public CommandStatus Status = CommandStatus.WAITING; 
    }
    public class GigTransaction
    {
        public string Sender;
        public string Receiver;
        public int Amount;
        public DateTime TimeStamp;
    }
    public class GIGNewsEntry
    {
        public string Name;
        public string Content;
    }
    public enum GIGRoles : byte
    {
        Directeur = 4,
        Administrateur = 3 ,
        Moderateur = 2,
        Utilisateur = 1
    }

    public class Car
    {
        public string Name;
        public int ID;
    }
    public class GigMessage
    {
        public string Sender;
        public string Message;
        public DateTime RD;
    }
   public class GigUser
    {
       public string Username;
       public string Name;
       public string Email;
       public GIGRoles Role;
       public int GIGP;
       public DateTime RegistrationDate;
   
    }
   public class GFriendRequests
   {
       public string Friend;
       public string Message;
   }
   public class GCharacter
   {
       public string Name;
       public string Firstname;
       public ushort Skin;
       public ushort Job;
       public ulong PayDay;
       public ushort Faction;
       public ushort Rank;
       public byte Level;
   }

   public class GLog
   {
       public string LogText;
       public DateTime LogDate;
   }
}
