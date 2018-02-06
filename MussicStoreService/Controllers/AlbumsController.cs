using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using MussicStoreService.Models;

namespace MussicStoreService.Controllers
{
    [RoutePrefix ("Albums")]
    public class AlbumsController : ApiController
    {
        //List<Albums> Album = new List<Albums>();
        SqlConnection conn = null;
        public AlbumsController()
        {
            try
            {
                conn = new SqlConnection();
                conn.ConnectionString = "server=.;database=MusicStoreDb;integrated security=true";
                conn.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        [Route("all")]
        public HttpResponseMessage GetAllAlbums()
        {
            List<Album> Albums = null;
            try
            {
                Albums = new List<Album>();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select * from album";
               // conn.Open();
                SqlDataReader rdr = comm.ExecuteReader();
                while(rdr.Read())
                {
                    Album a = new Album
                    {
                        albumid = int.Parse(rdr["albumid"].ToString()),
                        albumname = rdr["albumname"].ToString(),
                        generedetails = null,
                        artistdetails = null
                    };
                    Albums.Add(a);
                }
                rdr.Close();
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, Albums);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            
        }

        [Route("albums/{artistid}")]
        public HttpResponseMessage GetAlbum(int artistid)
        {
            try
            {
               

                Album a = null;
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select *from Album where artistid="+artistid;
                SqlDataReader rdr = comm.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();
                    conn.Close();
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "album with id" + artistid + "not found");
                }
                while (rdr.Read())
                {
                    a = new Album
                    {
                        albumid = int.Parse(rdr["albumid"].ToString()),
                        albumname = rdr["albumname"].ToString()
                    };

                }
                rdr.Close();
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, a);

            }
            catch(Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);

            }
        }




        [Route("Album/{generename}")]
        public HttpResponseMessage GetAllAlbums(string generename)
        {
            try

            {
                Album a = null;
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select * from Albums where genereid=(select genereid from Generes where generename ='" + generename + "')";
                SqlDataReader rdr = comm.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();
                    conn.Close();
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Album with" + generename + "not found");
                }
                while (rdr.Read())
                {
                    a = new Album
                    {
                        albumid = int.Parse(rdr["albumid"].ToString()),
                        albumname = rdr["albumname"].ToString(),
                        generedetails = null,
                        artistdetails = null

                    };

                }
                rdr.Close();
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, a);

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);

            }

        }
    }
}
