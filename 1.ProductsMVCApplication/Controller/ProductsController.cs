using ProductsMVCApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductsMVCApplication.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductsDB;Integrated Security=True";
            cn.Open();
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Connection = cn;
            cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
            cmdSelect.CommandText = "ShowAllProducts";
            List<Product> products = new List<Product>();
            try
            {
                SqlDataReader sdr = cmdSelect.ExecuteReader();
                while (sdr.Read())
                {
                    products.Add(new Product { ProductId = sdr.GetInt32(0), ProductName = sdr.GetString(1), Rate = sdr.GetDecimal(2), Descriptions = sdr.GetString(3), CategoryName = sdr.GetString(4) });
                    //products.Add(new Product { ProductId = (int)sdr["ProductId"], ProductName=(string)sdr["ProductName"], Rate=(decimal)sdr["Rate"], CategoryName=(string)sdr["CategryName"], Descriptions=(string)sdr["Descriptions"] });
                }
                sdr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            cn.Close();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductsDB;Integrated Security=True";
            cn.Open();
            SqlCommand cmdInsert = new SqlCommand();
            cmdInsert.Connection = cn;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.CommandText = "select * from tbl_Products where ProductId=@ProductId";
            cmdInsert.Parameters.AddWithValue("@ProductId", id);
            SqlDataReader dr = cmdInsert.ExecuteReader();
            Product prod = null;
            if (dr.Read())
                prod = new Product{ProductId = id, ProductName = dr.GetString(1),Rate = dr.GetDecimal(2), Descriptions = dr.GetString(3), CategoryName=dr.GetString(4)};
            else
            {
                //not found
                ViewBag.ErrorMessage = "Not found";
            }
            cn.Close();
            return View(prod);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(Product prod)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductsDB;Integrated Security=True";
            cn.Open();
            SqlCommand cmdUpdate = new SqlCommand();
            cmdUpdate.Connection = cn;
            cmdUpdate.CommandType = System.Data.CommandType.StoredProcedure;
            //cmdUpdate.CommandText = "update tbl_Products set ProductName = @ProductName , Rate = @Rate , Descriptions = @Descriptions , CategoryName = @CategoryName where ProductId = @ProductId";
            cmdUpdate.CommandText = "UpdateProducts";
            cmdUpdate.Parameters.AddWithValue("@ProductId", prod.ProductId);
            cmdUpdate.Parameters.AddWithValue("@ProductName", prod.ProductName);
            cmdUpdate.Parameters.AddWithValue("@Rate", prod.Rate);
            cmdUpdate.Parameters.AddWithValue("@Descriptions", prod.Descriptions);
            cmdUpdate.Parameters.AddWithValue("@CategoryName", prod.CategoryName);
            try
            {
                // TODO: Add insert logic here
                cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
            cn.Close();
            return RedirectToAction("Index");
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
