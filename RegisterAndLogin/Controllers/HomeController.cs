using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RegisterAndLogin.Models;

namespace RegisterAndLogin.Controllers
{
    public class HomeController : Controller
    {

        string connectionString = ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(MyModel MM)
        {
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string Query = "sp_InsertLoginTable";
            SqlCommand sqlcmd = new SqlCommand(Query, sqlcon);
            sqlcmd.Parameters.AddWithValue("@Username", MM.Username);
            sqlcmd.Parameters.AddWithValue("@EmailID", MM.EmailID);
            sqlcmd.Parameters.AddWithValue("@Password", MM.Password);
            sqlcmd.Parameters.AddWithValue("@Country", MM.Country);
            sqlcmd.Parameters.AddWithValue("@MobileNo", MM.MobileNo);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();
            sqlcmd.ExecuteNonQuery();
            sqlcon.Close();
            return RedirectToAction("Login", "Home"); 
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MyModel MM)
        {
            DataTable myregtable = new DataTable();
            SqlConnection mycon = new SqlConnection(connectionString);
            //string myregquery = "select MAX(UserID)+1 from Registerr";
            string myregquery = "EXEC sp_UserLogin '" + MM.Username + "','" + MM.Password + "'";
            mycon.Open();
            SqlDataAdapter sqldata = new SqlDataAdapter(myregquery, mycon);
            sqldata.Fill(myregtable);
            mycon.Close();
            Session["Username"] = MM.Username.ToString();
            if (myregtable.Rows.Count == 0)
            {
                ViewBag.alertlogin = "Invalid Credentials";
                return View();
            }
            else
            {
                return RedirectToAction("Welcome", "Home");
            }


        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }
    }
}