using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using MussicStoreService.Models;
using System.Web.Http.Cors;

namespace MussicStoreService.Controllers
{
    //name of the controller
    //[EnableCors(origins:"http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    
    [RoutePrefix("Generes")]
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class GeneresController : ApiController
    {

      
        SqlConnection conn = new SqlConnection();
        private string generename;

        public GeneresController()
        {
            try
            {
                conn = new SqlConnection();
                conn.ConnectionString = "server=.;database=MusicStoreDb;integrated security=true";
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        [Route("all")]
        //GET :http://localhost/MussicStoreService/Generes/all
        public HttpResponseMessage GetAllGeneres()
        {
            try
            {

                List<Genere> generes = new List<Genere>();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
               
                comm.CommandText = "select * from Genere";
                SqlDataReader rdr = comm.ExecuteReader();
                Genere g = null;
                while (rdr.Read())
                {
                    g = new Genere
                    {
                        Genereid = int.Parse(rdr["genereid"].ToString()),
                        Generename = rdr["generename"].ToString()
                    };
                    generes.Add(g);
                }
                rdr.Close();
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, generes);
            }
            catch (Exception ex)
            {
                //throw ex;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        //GET :http://localhost/MussicStoreService/Generes/genere/22
        [Route("genere/{genereid}")]
        public HttpResponseMessage GetGenere(int genereid)
        {
            try

            {
                    Genere g = null;
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = "select *from Genere where genereid="+genereid;
                    SqlDataReader rdr = comm.ExecuteReader();
                       if(!rdr.HasRows)
                    {
                        rdr.Close();
                        conn.Close();
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "genere with id"+" " + genereid +"  "+"not found");
                    }
                    while (rdr.Read())
                    {
                        g = new Genere
                        {
                            Genereid = int.Parse(rdr["genereid"].ToString()),
                            Generename = rdr["generename"].ToString()
                        };

                    }
                    rdr.Close();
                    conn.Close();
                    return Request.CreateResponse(HttpStatusCode.OK, g);

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);

            }

        }


        [Route("addGenere")]
        public HttpResponseMessage PostGenere(Genere g)
        {
            try
            {
                //Genere g = null;
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.CommandText = "InsertGenere";

                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@genereid";
                p1.Value= g.Genereid;

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@generename";
                p2.Value = g.Generename;

                comm.Parameters.Add(p1);
                comm.Parameters.Add(p2);
                comm.ExecuteNonQuery();

                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, g);

            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    
       [Route("deleteGenere/{genereid}")]
       public HttpResponseMessage DeleteGenere(int genereid)
            {
                    try
                    {
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.CommandText = "DeleteGenere";

                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@genereid";
                p1.Value = genereid;
                comm.Parameters.Add(p1);
                comm.ExecuteNonQuery();
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
             
                    }
                    catch(Exception e)
                    {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,e.Message);
                    }
            }

        [Route("updategenere/{genereid}/{generename}")]
        public HttpResponseMessage PutGenere(int Genereid,string Generename)
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.CommandText = "UpdateGenere";

                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@genereid";
                p1.Value = Genereid;

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@newgenerename";
                p2.Value = Generename;

                comm.Parameters.Add(p1);
                comm.Parameters.Add(p2);
                comm.ExecuteNonQuery();
                conn.Close();
                return Request.CreateErrorResponse(HttpStatusCode.OK,"records updates");
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,e.Message);
            }
        }



    }
}