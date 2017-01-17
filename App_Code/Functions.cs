using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Web;
using Novacode;

/// <summary>
/// Summary description for Functions
/// </summary>
public class Functions
{
    public static Boolean ExecuteCommand(String SQL)
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;
        try
        {
            connection.Open();
            cmd.CommandText = SQL;
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return false;
        }       
        finally
        { connection.Close(); }
    }

    public static string ExecuteScalar(String SQL)
    {
        try
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
            SqlCommand cmd = new SqlCommand(SQL, connection);
            connection.Open();

            string tPom = cmd.ExecuteScalar().ToString();
            connection.Close();

            return tPom;
        }
        catch (Exception err)
        {
            //HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            //if (HttpContext.Current != null)
            //    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return ""; 
        }
    }

    //public static String[] ReadIntoArray(String SQL, int NumberOfColumns)
    public static List<String> ReadIntoArray(String SQL, String ColumnName)
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        SqlCommand cmd = new SqlCommand(SQL, connection);
        try
        {
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<String> list = (from IDataRecord r in reader select r[ColumnName].ToString()).ToList();
            return list;
        }
       
        catch (Exception err) 
        { 
            List<String> Empty = null;
            //HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            //if (HttpContext.Current != null)
            //    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return Empty;        
        }
        finally
        {
            connection.Close();
        }
    }

    public static String[] ReturnIntoArray(String SQL, int NumberOfColumns)
    {
        String[] Array = new String[NumberOfColumns];

        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        SqlCommand cmd = new SqlCommand(SQL, connection);
        try
        {
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int Br = 0; Br < NumberOfColumns; Br++)
                    Array[Br] = reader[Br].ToString();
            }
            reader.Close();
            connection.Close();
            return Array;
        }
        catch (Exception err) { String[] Empty = new String[0];
            //HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            //if (HttpContext.Current != null)
            //    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return Empty; }
        finally
        {
            connection.Close();
        }
    }

    public static SqlCommand ReturnCommand(String SQL)
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = SQL;
        return cmd;
    }

    public static System.Data.DataTable ReturnDataTable(String SQL)
    {
        try
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
            SqlCommand cmd = new SqlCommand(SQL, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            System.Data.DataTable dt = new System.Data.DataTable();
            connection.Open();
            da.Fill(dt);
            return dt;
        }
        catch (Exception err) { System.Data.DataTable Empty = new System.Data.DataTable();
            //HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            //if (HttpContext.Current != null)
            //    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return Empty; }
    }

    public static byte[] GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath, int MaxWidth)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            Decimal Odnos = (Decimal)((Decimal)image.Width / (Decimal)image.Height);
            int newWidth = 0;
            int newHeight = 0;
            if (image.Width > MaxWidth)
            {
                newWidth = MaxWidth;
                newHeight = (int)(MaxWidth / Odnos);
            }
            else
            {
                newWidth = (int)(image.Width * scaleFactor);
                newHeight = (int)(image.Height * scaleFactor);
            }
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighSpeed;
            thumbGraph.SmoothingMode = SmoothingMode.HighSpeed;
            thumbGraph.InterpolationMode = InterpolationMode.Low;
            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                thumbnailImg.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Position = 0;
                byte[] data = new byte[stream.Length];
                data = stream.ToArray();
                return data;
            }
        }
    }

    public static void FillCombo(String pSQL, DropDownList Control, String DisplayMember, String ValueMember)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = Functions.ReturnDataTable(pSQL);
        Control.DataSource = dt;
        Control.DataTextField = DisplayMember;
        Control.DataValueField = ValueMember;
        Control.DataBind();
    }

    public static bool SendEmail(String Subject, String BodyIn)
    {
        String pSQLEmail = "SELECT ConfigValue FROM [DP_Config] WHERE ConfigOption='email'";
        String ToAddress = ExecuteScalar(pSQLEmail);
        const string FromAddress = "divnapesic.webservice@gmail.com";
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(ToAddress);
        //mail.To.Add("panovski_v@yahoo.com");
        mail.From = new MailAddress(FromAddress);
        mail.Subject = Subject;

        string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
        body += "</HEAD><BODY><DIV><FONT face=Arial size=2>" + BodyIn;
        body += "</FONT></DIV></BODY></HTML>";

        ContentType mimeType = new System.Net.Mime.ContentType("text/html");
        AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
        mail.AlternateViews.Add(alternate);

        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential(FromAddress, "dpws1234");
        client.Port = 587; // Gmail works on this port
        client.Host = "smtp.gmail.com"; client.EnableSsl = true; //Gmail works on Server Secured Layer
        try
        {
            client.Send(mail);
            return true;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return false;
        }
    }

    public static string Encrypt(string clearText)
    {
        try
        {
            string EncryptionKey = ConfigurationManager.AppSettings["Enc_Key"];
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return "";
        }
    }   

    public static string Decrypt(string cipherText)
    {
        try
        {
            string EncryptionKey = ConfigurationManager.AppSettings["Enc_Key"];
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return "";
        }
    }

    public static string VratiWherePart(TextBox Kontrola, String Kolona, String WherePart)
    {
        String WherePart1 = "";
        if (Kontrola.Text.Length > 0)
        {
            string[] Lista = Kontrola.Text.ToString().Split(',');
            foreach (string word in Lista)
            {
                String tWord = word.Trim();
                if (WherePart1.Length > 0) WherePart1 += " OR ";
                WherePart1 += " " + Kolona + " LIKE N'%" + tWord + "%' ";
            }

            if (WherePart.Length > 0)
            {
                if (WherePart1.Length > 0)
                    WherePart += " AND (" + WherePart1 + ") ";
            }
            else
            {
                if (WherePart1.Length > 0)
                    WherePart += " (" + WherePart1 + ") ";
            }
        }

        return WherePart;
    }

    public static string VratiWherePartDDL(DropDownList Kontrola, String Kolona, String WherePart)
    {
        String WherePart1 = "";
        if (Kontrola.Text.Length > 0)
        {
            string[] Lista = Kontrola.Text.ToString().Split(',');
            foreach (string word in Lista)
            {
                String tWord = word.Trim();
                if (tWord == "-1") return WherePart;
                if (WherePart1.Length > 0) WherePart1 += " OR ";
                WherePart1 += " " + Kolona + " LIKE N'%" + tWord + "%' ";
            }

            if (WherePart.Length > 0)
            {
                if (WherePart1.Length > 0)
                    WherePart += " AND (" + WherePart1 + ") ";
            }
            else
            {
                if (WherePart1.Length > 0)
                    WherePart += " (" + WherePart1 + ") ";
            }
        }

        return WherePart;
    }

    public static string VratiWherePartInteger(TextBox Kontrola, String Kolona, String Sign, String WherePart)
    {
        String WherePart1 = "";
        if (Kontrola.Text.Length > 0)
        {
            WherePart1 += " " + Kolona + " " + Sign + " N'" + Kontrola.Text + "' ";

            if (WherePart.Length > 0)
            {
                if (WherePart1.Length > 0)
                    WherePart += " AND (" + WherePart1 + ") ";
            }
            else
            {
                if (WherePart1.Length > 0)
                    WherePart += " (" + WherePart1 + ") ";
            }
        }

        return WherePart;
    }
    public static void PrintWord(String Dokument, String Sql, String Sql2 = "")
    {
        object missing = System.Reflection.Missing.Value;
        object oTemplate = Dokument +".dotx";

        DocX owordDocnew = DocX.Load(Dokument + ".docx");
        
        //owordDocnew.ApplyTemplate(Dokument+".dotx");
        //Microsoft.Office.Interop.Word.ApplicationClass oWordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
        //Microsoft.Office.Interop.Word.Document oWordDoc = oWordApp.Documents.Add(ref oTemplate, ref missing, ref missing, ref missing);


        SqlConnection MyConn = new SqlConnection();
        MyConn.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        System.Data.DataTable dt = new System.Data.DataTable();
        System.Data.DataTable dt2 = new System.Data.DataTable();
        DataSet ds = new DataSet();

        SqlDataAdapter da = new SqlDataAdapter(Sql, MyConn);

        MyConn.Open();
        da.Fill(dt);
        MyConn.Close();
        ds.Tables.Add(dt);

        if (Sql2 != "")
        {
            SqlDataAdapter da2 = new SqlDataAdapter(Sql2, MyConn);
            MyConn.Open();
            da2.Fill(dt2);
            MyConn.Close();
            ds.Tables.Add(dt2);
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (DataColumn dcol in ds.Tables[0].Columns)
            {
                try
                {
                    owordDocnew.Bookmarks[dcol.ColumnName].SetText(dr[dcol.ColumnName].ToString());
                    //oWordDoc.Bookmarks[dcol.ColumnName].Range.Text = dr[dcol.ColumnName].ToString();
                }
                catch(Exception err) {
                    HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                    if (HttpContext.Current != null)
                        HttpContext.Current.Response.Redirect(@"~\Error.aspx");
                }
            }
        }


        Int32 tTableForPrint = 0;
        if (ds.Tables.Count > 1)
        {
            try
            {
                for (int brTabeli = 1; brTabeli <= ds.Tables.Count - 1; brTabeli++)
                {
                    int BrRedovi = 1;
                    Boolean tPostoi = false;
                    Boolean tKreiranaTabela = false;
                    int rows = ds.Tables[brTabeli].Rows.Count;
                    int columns = ds.Tables[brTabeli].Columns.Count;

                    if (brTabeli <= owordDocnew.Tables.Count)
                    //if (brTabeli <= oWordDoc.Tables.Count)
                    {
                        //tTableForPrint += 1;
                        //for (int tBrPostoecki = 1; tBrPostoecki <= owordDocnew.Tables.Count; tBrPostoecki++)
                        //{
                        //    if (owordDocnew.Bookmarks["Table" + brTabeli].Range.Start >= oWordDoc.Tables[tBrPostoecki].Range.Start)
                        //    {
                        //        if ((oWordDoc.Bookmarks["Table" + brTabeli].Range.Start >= oWordDoc.Tables[tBrPostoecki].Range.Start &&
                        //            oWordDoc.Bookmarks["Table" + brTabeli].Range.Start <= oWordDoc.Tables[tBrPostoecki].Range.End) ||
                        //            (oWordDoc.Bookmarks["Table" + brTabeli].Range.End >= oWordDoc.Tables[tBrPostoecki].Range.Start &&
                        //            oWordDoc.Bookmarks["Table" + brTabeli].Range.End >= oWordDoc.Tables[tBrPostoecki].Range.End))
                        //        //ako ima iskreirano tabeli vo template-ot:
                        //        {
                        //            BrRedovi = 1; tPostoi = true; tTableForPrint = tBrPostoecki;
                        //            for (int tPostoeckiRows = 1; tPostoeckiRows <= oWordDoc.Tables[tBrPostoecki].Rows.Count; tPostoeckiRows++)
                        //            {
                        //                for (int tPostoeckiColumns = 1; tPostoeckiColumns <= oWordDoc.Tables[tBrPostoecki].Columns.Count; tPostoeckiColumns++)
                        //                {
                        //                    if (oWordDoc.Tables[tBrPostoecki].Cell(tPostoeckiRows, tPostoeckiColumns).Range.Text.Length > 2)
                        //                    {
                        //                        BrRedovi += 1;
                        //                    }
                        //                }
                        //            cont:;
                        //            }

                        //            for (int tBr = 0; tBr <= rows - 2; tBr++)
                        //            {
                        //                oWordDoc.Tables[tBrPostoecki].Rows.Add();
                        //            }
                        //            goto cont1;
                        //        }
                        //        else
                        //        {
                        //            tPostoi = false; tTableForPrint = tBrPostoecki;
                        //        }
                        //    }
                        //cont1:;
                        //}
                    }
                    else tPostoi = false;

                    if (tPostoi == false)
                    {
                        
                        ////Range r = oWordDoc.Bookmarks["Table" + brTabeli].Range;
                        ////oWordDoc.Tables.Add(r, rows + 1, columns);
                        ////BrRedovi = 2;
                        ////tTableForPrint += 1;

                        ////int j = 1;
                        ////foreach (DataColumn dcc in ds.Tables[brTabeli].Columns)
                        ////{
                        ////    oWordDoc.Tables[tTableForPrint].Cell(1, j).Range.InsertAfter(dcc.ColumnName);
                        ////    j++;
                        ////}
                    }

                    int i = 2; // BrRedovi;
                    foreach (DataRow drr in ds.Tables[brTabeli].Rows)
                    {
                        int j = 1;
                        foreach (DataColumn dc2 in ds.Tables[brTabeli].Columns)
                        {
                            //oWordDoc.Tables[tTableForPrint].Cell(i, j).Range.InsertAfter(drr[dc2.ColumnName].ToString());
                            j++;
                        }
                        i++;
                    }

                }
            }
            catch (Exception err) {
                HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            }
        }

        //owordDocnew.SaveAs(Dokument + ".pdf");
        owordDocnew.SaveAs(Dokument + "-1.docx");
        //oWordDoc.Activate();
        //oWordDoc.SaveAs(Dokument+".pdf", WdSaveFormat.wdFormatPDF);

        object oMissing = System.Reflection.Missing.Value;
        //object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
        //((_Document)oWordDoc).Close(ref saveChanges, ref oMissing, ref oMissing);
        //oWordDoc = null;
        owordDocnew = null;
    }

    public static void PrintWordMulti(String Dokument, String Output, String Sql, int Number, String Sql2 = "")
    {
        object missing = System.Reflection.Missing.Value;
        object oTemplate = Dokument;//+ Template+ ".dotx";

        Microsoft.Office.Interop.Word.ApplicationClass oWordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
        Microsoft.Office.Interop.Word.Document oWordDoc = oWordApp.Documents.Add(ref oTemplate, ref missing, ref missing, ref missing);


        SqlConnection MyConn = new SqlConnection();
        MyConn.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
        System.Data.DataTable dt = new System.Data.DataTable();
        System.Data.DataTable dt2 = new System.Data.DataTable();
        DataSet ds = new DataSet();

        SqlDataAdapter da = new SqlDataAdapter(Sql, MyConn);

        MyConn.Open();
        da.Fill(dt);
        MyConn.Close();
        ds.Tables.Add(dt);

        if (Sql2 != "")
        {
            SqlDataAdapter da2 = new SqlDataAdapter(Sql2, MyConn);
            MyConn.Open();
            da2.Fill(dt2);
            MyConn.Close();
            ds.Tables.Add(dt2);
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (DataColumn dcol in ds.Tables[0].Columns)
            {
                try
                {
                    oWordDoc.Bookmarks[dcol.ColumnName].Range.Text = dr[dcol.ColumnName].ToString();
                }
                catch (Exception err) {
                    HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                    if (HttpContext.Current != null)
                        HttpContext.Current.Response.Redirect(@"~\Error.aspx");
                }
            }
        }


        Int32 tTableForPrint = 0;
        if (ds.Tables.Count > 1)
        {
            try
            {
                for (int brTabeli = 1; brTabeli <= ds.Tables.Count - 1; brTabeli++)
                {
                    int BrRedovi = 1;
                    Boolean tPostoi = false;
                    Boolean tKreiranaTabela = false;
                    int rows = ds.Tables[brTabeli].Rows.Count;
                    int columns = ds.Tables[brTabeli].Columns.Count;

                    if (brTabeli <= oWordDoc.Tables.Count)
                    {
                        tTableForPrint += 1;
                        for (int tBrPostoecki = 1; tBrPostoecki <= oWordDoc.Tables.Count; tBrPostoecki++)
                        {
                            if (oWordDoc.Bookmarks["Table" + brTabeli].Range.Start >= oWordDoc.Tables[tBrPostoecki].Range.Start)
                            {
                                if ((oWordDoc.Bookmarks["Table" + brTabeli].Range.Start >= oWordDoc.Tables[tBrPostoecki].Range.Start &&
                                    oWordDoc.Bookmarks["Table" + brTabeli].Range.Start <= oWordDoc.Tables[tBrPostoecki].Range.End) ||
                                    (oWordDoc.Bookmarks["Table" + brTabeli].Range.End >= oWordDoc.Tables[tBrPostoecki].Range.Start &&
                                    oWordDoc.Bookmarks["Table" + brTabeli].Range.End >= oWordDoc.Tables[tBrPostoecki].Range.End))
                                //ako ima iskreirano tabeli vo template-ot:
                                {
                                    BrRedovi = 1; tPostoi = true; tTableForPrint = tBrPostoecki;
                                    for (int tPostoeckiRows = 1; tPostoeckiRows <= oWordDoc.Tables[tBrPostoecki].Rows.Count; tPostoeckiRows++)
                                    {
                                        for (int tPostoeckiColumns = 1; tPostoeckiColumns <= oWordDoc.Tables[tBrPostoecki].Columns.Count; tPostoeckiColumns++)
                                        {
                                            if (oWordDoc.Tables[tBrPostoecki].Cell(tPostoeckiRows, tPostoeckiColumns).Range.Text.Length > 2)
                                            {
                                                BrRedovi += 1;
                                            }
                                        }
                                    cont:;
                                    }

                                    for (int tBr = 0; tBr <= rows - 2; tBr++)
                                    {
                                        oWordDoc.Tables[tBrPostoecki].Rows.Add();
                                    }
                                    goto cont1;
                                }
                                else
                                {
                                    tPostoi = false; tTableForPrint = tBrPostoecki;
                                }
                            }
                        cont1:;
                        }
                    }
                    else tPostoi = false;

                    if (tPostoi == false)
                    {
                        Range r = oWordDoc.Bookmarks["Table" + brTabeli].Range;
                        oWordDoc.Tables.Add(r, rows + 1, columns);
                        BrRedovi = 2;
                        tTableForPrint += 1;

                        int j = 1;
                        foreach (DataColumn dcc in ds.Tables[brTabeli].Columns)
                        {
                            oWordDoc.Tables[tTableForPrint].Cell(1, j).Range.InsertAfter(dcc.ColumnName);
                            j++;
                        }
                    }

                    int i = 2; // BrRedovi;
                    foreach (DataRow drr in ds.Tables[brTabeli].Rows)
                    {
                        int j = 1;
                        foreach (DataColumn dc2 in ds.Tables[brTabeli].Columns)
                        {
                            oWordDoc.Tables[tTableForPrint].Cell(i, j).Range.InsertAfter(drr[dc2.ColumnName].ToString());
                            j++;
                        }
                        i++;
                    }
                }
            }
            catch (Exception err) {
                HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            }
        }

        oWordDoc.Activate();
        //oWordDoc.SaveAs2(Dokument + "Multi\\" + Template + Number.ToString() + ".docx");
        oWordDoc.SaveAs(Output + Number.ToString() + ".docx");

        object oMissing = System.Reflection.Missing.Value;
        object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
        ((_Document)oWordDoc).Close(ref saveChanges, ref oMissing, ref oMissing);
        oWordDoc = null;
    }

    public static string ReturnLatin(String Input)
    {
        String Izlez = Input;
        try
        {
            var map = new Dictionary<char, string>
        {
            { 'А', "A" },{ 'а', "a" },{ 'Б', "B" },{ 'б', "b" },{ 'В', "V" },{ 'в', "v" },{ 'Г', "G" },{ 'г', "g" },
            { 'Д', "D" },{ 'д', "d" },{ 'Ѓ', @"\" },{ 'ѓ', "|" },{ 'Е', "E" },{ 'е', "e" },{ 'Ж', "@" },{ 'ж', "`" },
            { 'З', "Z" },{ 'з', "z" },{ 'Ѕ', "Y" },{ 'ѕ', "y" },{ 'И', "I" },{ 'и', "i" },{ 'Ј', "J" },{ 'ј', "j" },
            { 'К', "K" },{ 'к', "k" },{ 'Л', "L" },{ 'л', "l" },{ 'Љ', "Q" },{ 'љ', "q" },{ 'М', "M" },{ 'м', "m" },
            { 'Н', "N" },{ 'н', "n" },{ 'Њ', "W" },{ 'њ', "w" },{ 'О', "O" },{ 'о', "o" },{ 'П', "P" },{ 'п', "p" },
            { 'Р', "R" },{ 'р', "r" },{ 'С', "S" },{ 'с', "s" },{ 'Т', "T" },{ 'т', "t" },{ 'Ќ', "]" },{ 'ќ', "}" },
            { 'У', "U" },{ 'у', "u" },{ 'Ф', "F" },{ 'ф', "f" },{ 'Х', "H" },{ 'х', "h" },{ 'Ц', "c" },{ 'ц', "c" },
            { 'Ч', "&" },{ 'ч', "~" },{ 'Џ', "X" },{ 'џ', "x" },{ 'Ш', "[" },{ 'ш', "{" },{ ' ', " " },
            { '1', "1" },{ '2', "2" },{ '3', "3" },{ '4', "4" },{ '5', "5" },{ '6', "6" },{ '7', "7" },{ '8', "8" },
            { '9', "9" },{ '_', "_" },{ '-', "-" }
        };
            return string.Concat(Input.Select(c => map[c]));
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            return Izlez;
        }
    }

    public static void MergeDocs(String ExportDoc, String CurrentDoc, bool Clear, bool Last)
    {
        object missing = System.Reflection.Missing.Value;
        object oExportDoc = ExportDoc; //+ ".dotx";
        object oCurrentDoc = CurrentDoc;
        object pageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;

        Microsoft.Office.Interop.Word.ApplicationClass oWordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
        Microsoft.Office.Interop.Word.Document oWordDocExport = oWordApp.Documents.Add(ref oExportDoc, ref missing, ref missing, ref missing);
     
        if (Clear)
        {
            oWordDocExport.Content.Delete();
        }

        Microsoft.Office.Interop.Word.Selection selection = oWordApp.Selection;
        selection.InsertFile(CurrentDoc, ref missing, ref missing, ref missing, ref missing);

        //if(!Last)
        //    selection.InsertBreak(ref pageBreak);

        oWordDocExport.Activate();
        oWordDocExport.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
        oWordDocExport.SaveAs(ExportDoc);

        if (Last)
            oWordDocExport.SaveAs(ExportDoc.Replace(".docx", ".pdf"), WdSaveFormat.wdFormatPDF);

        object oMissing = System.Reflection.Missing.Value;
        object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
        ((_Document)oWordDocExport).Close(ref saveChanges, ref oMissing, ref oMissing);
        oWordDocExport = null;
    }
}