using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Burrow;
using NHibernate.Burrow.Util;

/// <summary>
/// Summary description for Util
/// </summary>
public class Util
{
   public static void ResetEnvironment()
   {
       SchemaUtil su = new SchemaUtil();
       su.CreateSchemas();
       BurrowFramework f = new BurrowFramework();
       f.CloseWorkSpace();
       f.BurrowEnvironment.ShutDown(); //Restart the environment to prepare a fresh start 
       f.BurrowEnvironment.Start();
       Checker.CheckSpanningConversations(0);
       f.InitWorkSpace();
   }
}
