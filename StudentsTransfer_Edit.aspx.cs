#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class StudentsTransfer_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();   
            if (Request.QueryString["StudentID"] != "")
                Functions.FillCombo(@"SELECT g.GroupID, g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course 
                                    FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID 
                                    WHERE GroupID NOT IN (SELECT GroupID FROM GroupStudent WHERE StudentID=" + Request.QueryString["StudentID"] + ")", ddlCourse, "Course", "GroupID");

                tbGroupName.Text = Functions.ExecuteScalar(@"SELECT g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course 
                                    FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID 
                                    WHERE GroupID=" + Request.QueryString["GroupID"] );
        }
    }
    #endregion

    #region Functions
    protected void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
    }
    protected string Get_PaymentNumber()
    {
        String MaxID = Functions.ExecuteScalar(@"SELECT Max(CAST(LEFT(PaymentNumber, 4) AS INT)) as PaymentN FROM Payment WHERE CAST(RIGHT(PaymentNumber, 2) AS INT)='" + DateTime.Now.ToString("yy") + "'");
        int NewID = 0;
        NewID = Convert.ToInt32(MaxID) + 1;

        return NewID.ToString().PadLeft(4, '0') + "/" + DateTime.Now.ToString("yy");
    }
    public static string NumberToWords(int number, string Tip)
    {
        if (number == 0)
            return "нула";

        if (number < 0)
            return "минус " + NumberToWords(Math.Abs(number), "");

        string words = "";

        if ((number / 1000000) > 0)
        {
            String Add = "";
            if (number / 1000000 == 1)
                Add = " милион ";
            else
                Add = " милиони ";

            words += NumberToWords(number / 1000000, "milion") + Add;// " милион ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            String Add = "";
            if (number / 1000 == 1)
                Add = " илјада ";
            else
                Add = " илјади ";

            words += NumberToWords(number / 1000, "iljada") + Add;// " илјади ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            String Add = "";
            int Ostatok = number % 100;
            if (Ostatok == 0 && words.Length > 0)
                Add = " и ";
            words += Add + NumberToWords(number / 100, "stotka") + "";
            number %= 100;
        }
        if ((number / 10) > 0)
        {
            String Add = "";
            int Ostatok = number % 10;
            if (Ostatok == 0 && words.Length > 0)
                Add = " и ";
            words += Add;// + NumberToWords(number / 100, "stotka") + "";
            //number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += " ";
            //words += " и ";

            var unitsMap = new[] { "нула", "еден", "два", "три", "четири", "пет", "шест", "седум", "осум", "девет", "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет" };
            var tensMap = new[] { "нула", "десет", "дваесет", "триесет", "четириесет", "педесет", "шеесет", "седумдесет", "осумдесет", "деведесет" };
            var HundredMap = new[] { "нула", "сто", "двестa", "триста", "четиристотини", "петстотини", "шестотини", "седумстотини", "осумстотини", "деветстотини" };
            var unitsMapThousend = new[] { "нула", "", "две", "три", "четири", "пет", "шест", "седум", "осум", "девет", "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет" };

            if (number < 20)
            {
                if (Tip == "stotka")
                    words += HundredMap[number];
                else if (Tip == "iljada")
                    words += unitsMapThousend[number];
                else
                    words += unitsMap[number];
            }
            else
            {
                if (Tip == "stotka")
                {
                    words += HundredMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
                else if (Tip == "iljada")
                {
                    words += unitsMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
                else if (Tip == "milion")
                {
                    words += HundredMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMapThousend[number % 10];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " и " + unitsMap[number % 10];
                }
            }
        }

        return words;
    }

    #endregion

    #region Handled Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["GroupID"] != "" && Request.QueryString["StudentID"] != "")
        {
            //Insert into transfered table:
            String SQLInsert = @"INSERT INTO StudentTransfer(StudentID,fromGroupID,toGroupID,TransferDate,CreatedBy) VALUES(" + Request.QueryString["StudentID"] + ", " +
                            Request.QueryString["GroupID"] + "," + ddlCourse.SelectedValue + ",N'" + tbTransferDate.Text.Replace("'", "''") + "'," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
            Functions.ExecuteCommand(SQLInsert);

            //Set as transfered and set old costs:            
            String OldCosts = Functions.ExecuteScalar(@"SELECT DATEDIFF(WEEK,g.StartDate,st.TransferDate)*(SELECT COUNT(*) FROM Termin t WHERE t.GroupID=g.GroupID)*CEILING(g.Cost/g.NumberOfClasses) as TotalCostForBefore
                            FROM StudentTransfer st LEFT OUTER JOIN [Group] g ON g.GroupID=st.fromGroupID
                            LEFT OUTER JOIN GroupStudent gs ON (gs.StudentID=st.StudentID AND gs.GroupID=g.GroupID)
                            WHERE st.StudentID=" + Request.QueryString["StudentID"] + " AND st.fromGroupID=" + Request.QueryString["GroupID"]);
            if (OldCosts == "") OldCosts = "0";
            String SQLUpdate1 = @"UPDATE GroupStudent SET Transfered=1, TotalCost = " + OldCosts +
                            "WHERE StudentID=" + Request.QueryString["StudentID"] + " AND GroupID=" + Request.QueryString["GroupID"];
            Functions.ExecuteCommand(SQLUpdate1);

            //Get old discount:
            String Discount = Functions.ExecuteScalar("SELECT Discount FROM GroupStudent WHERE StudentID=" + Request.QueryString["StudentID"] + " AND GroupID=" + Request.QueryString["GroupID"]);
            if (Discount == "") Discount = "0";

            //Insert into new group with new costs:
            String SQLUpdate2 = @"INSERT INTO GroupStudent(GroupID,StudentID,Status,TotalCost,Discount,CreatedBy)
                            VALUES(" + ddlCourse.SelectedValue + "," + Request.QueryString["StudentID"] + @",0,
                            (SELECT MAX(tab.TotalCostForAfter) FROM (SELECT (g.NumberOfClasses - DATEDIFF(WEEK,g.StartDate,st.TransferDate)*(SELECT COUNT(*) FROM Termin t WHERE t.GroupID=g.GroupID))*
                            CEILING(g.Cost/g.NumberOfClasses)*(1-1.0*" + Discount + @"/100) as TotalCostForAfter
                            FROM StudentTransfer st LEFT OUTER JOIN [Group] g ON g.GroupID=st.toGroupID
                            LEFT OUTER JOIN GroupStudent gs ON (gs.StudentID=st.StudentID AND gs.GroupID=g.GroupID)
							WHERE st.toGroupID=" + ddlCourse.SelectedValue + " AND st.StudentID=" + Request.QueryString["StudentID"] + ")as tab)," + Discount + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
            Functions.ExecuteCommand(SQLUpdate2);

            //Check payments:
            //Find sum of current payments:
            String Paid = Functions.ExecuteScalar(@"SELECT SUM(Ammount) FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                             WHERE gs.StudentID=" + Request.QueryString["StudentID"] + " AND gs.GroupID=" + Request.QueryString["GroupID"]);
            if (Paid == "") Paid = "0";

            if (Convert.ToInt32(Paid) > Convert.ToInt32(OldCosts))
            {
                //set the old payments as transfered
                String SQLUpdate3 = @"UPDATE Payment SET Transfered=1 WHERE PaymentID IN (SELECT p.PaymentID FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                             WHERE gs.StudentID=" + Request.QueryString["StudentID"] + " AND gs.GroupID=" + Request.QueryString["GroupID"] + ")";
                Functions.ExecuteCommand(SQLUpdate3);

                //insert new payment just for the old costs
                String GroupStudentID = Functions.ExecuteScalar("SELECT GroupStudentID FROM GroupStudent WHERE StudentID=" + Request.QueryString["StudentID"] + " AND GroupID=" + Request.QueryString["GroupID"]);
                String SQLUpdate4 = @"INSERT INTO Payment (PaymentNumber,Ammount,AmmountWords,DateOfPayment,GroupStudentID,UserId,CreatedBy) VALUES('" + Get_PaymentNumber() +
                               "'," + OldCosts + ",N'" + NumberToWords(Convert.ToInt32(OldCosts), "") +
                               "',getdate()," + GroupStudentID + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                Functions.ExecuteCommand(SQLUpdate4);

                //insert the rest as new payment:
                int Rest = Convert.ToInt32(Paid) - Convert.ToInt32(OldCosts);
                String GroupStudentID_NEW = Functions.ExecuteScalar("SELECT GroupStudentID FROM GroupStudent WHERE StudentID=" + Request.QueryString["StudentID"] + " AND GroupID=" + ddlCourse.SelectedValue);
                String SQLUpdate5 = @"INSERT INTO Payment (PaymentNumber,Ammount,AmmountWords,DateOfPayment,GroupStudentID,UserId,CreatedBy) VALUES('" + Get_PaymentNumber() +
                               "'," + Rest.ToString() + ",N'" + NumberToWords(Rest, "") +
                               "',getdate()," + GroupStudentID_NEW + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                Functions.ExecuteCommand(SQLUpdate5);
            }
            lblInfo.Text = "The transfer is made! Please open the Student again!";
            lblInfo.Visible = true;
            btnSave.Enabled = false;
        }
    }
    #endregion
}