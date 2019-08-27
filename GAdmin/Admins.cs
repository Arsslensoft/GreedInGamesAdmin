using GAdminLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GAdmin
{
  internal  class Admins
    {
      internal static GAdminLib.GigClient admin;
      internal static int Connected;
      internal static Form1 MainForm;
      internal static string HOST = "http://localhost/cl";
      internal static string GTA_HOST = "ogrp.greedingames.com";
      internal static string GTA_PASSWORD = "D5XU6AQ4HBOC";
      internal static string GTA_PORT = "7777";
      public static void Initialize()
      {
          try
          {
           
              //if (!File.Exists(Application.StartupPath + @"\FIRSTRUN.dat"))
              //{
              //    File.Create(Application.StartupPath + @"\FIRSTRUN.dat");
              //    GPlayer.Install();
              //}

              admin = new GigClient(HOST);
            
              if (!File.Exists(Application.StartupPath + @"\Data\AT.dat"))
              {
                  LoginFrm frm = new LoginFrm();
                  frm.ShowDialog();
                  if (frm.close)
                  {
                      File.WriteAllText(Application.StartupPath + @"\Data\AT.dat", admin.AccessToken);
                      File.WriteAllText(Application.StartupPath + @"\Data\UN.dat", admin.Username);
                  }
                  else
                      Application.Exit();
              }
              else
              {

                  admin.AccessToken = File.ReadAllText(Application.StartupPath + @"\Data\AT.dat");
                  admin.Username = File.ReadAllText(Application.StartupPath + @"\Data\UN.dat");

                  // check AT
                  if (!admin.IsConnected())
                  {
                      LoginFrm frm = new LoginFrm();
                      frm.ShowDialog();
                      if (frm.close)
                      {
                          File.WriteAllText(Application.StartupPath + @"\Data\AT.dat", admin.AccessToken);
                          File.WriteAllText(Application.StartupPath + @"\Data\UN.dat", admin.Username);
                      }
                      else
                          Application.Exit();
                  }
                  else
                      admin.MyAccount = admin.GetUserInfo(admin.Username);
              }
              if (!File.Exists(Application.StartupPath + @"\Data\" + admin.Username + ".dat"))
                  File.WriteAllLines(Application.StartupPath + @"\Data\" + admin.Username + ".dat", new string[2] { "m1", "n1" });

              admin.MyAccount = admin.GetUserInfo(admin.Username);
              // Initialize Controls
            
          }
          catch (Exception ex)
          {
           //   GigSpace.LogError(ex);
          }
      }
      internal static void SetSTAT(string stat)
      {
          try
          {
            //  MainForm.UpdateLabel(MainForm.slb, stat);

          }
          catch (Exception ex)
          {
             // GigSpace.LogError(ex);
          }
      }

    }
}
