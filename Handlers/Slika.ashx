<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public class Handler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        // Create SQL Command 

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT s.MeniSlika FROM DP_MeniSlika As s" +
                          " WHERE s.MeniSlikaID = @Id";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = con;

        SqlParameter ImageID = new SqlParameter("@Id", System.Data.SqlDbType.Int);
        ImageID.Value = context.Request.QueryString["Id"];
        cmd.Parameters.Add(ImageID);
        try
        {
            con.Open();
            SqlDataReader dReader = cmd.ExecuteReader();
            dReader.Read();
            context.Response.BinaryWrite((byte[])dReader["MeniSlika"]);
            dReader.Close();
            con.Close();
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            con.Close();
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}