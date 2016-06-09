using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// required using statements to access EF DB 
using COMP2007_Lesson4B.Models;
using System.Web.ModelBinding; 

namespace COMP2007_Lesson4B
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetStudent();
            }
        }

        protected void GetStudent()
        {
            // populate the form with existing student record
            int StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            // connect the DB with EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // populate a student instance with the StudentID from the URL parameter
                Student updateStudent = (from student in db.Students
                                         where student.StudentID == StudentID
                                         select student).FirstOrDefault();

                // map the student properties to the form controls
                if (updateStudent != null)
                {
                    LastNameTextBox.Text = updateStudent.LastName;
                    FirstNameTextBox.Text = updateStudent.FirstMidName;
                    EnrollmentDateTextBox.Text = updateStudent.EnrollmentDate.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Students.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // connect to EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                // use the student model to save a new record
                Student newStudent = new Student();

                int StudentID = 0; 

                if(Request.QueryString.Count > 0)
                {
                    //get the id from URL 
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //get the current student from the EF DB
                    newStudent = (from student in db.Students
                                  where student.StudentID == StudentID
                                  select student).FirstOrDefault(); 
                }

                //Add form data to the new student record
                newStudent.LastName = LastNameTextBox.Text;
                newStudent.FirstMidName = FirstNameTextBox.Text;
                newStudent.EnrollmentDate = Convert.ToDateTime(EnrollmentDateTextBox.Text);

                // check to see if a new stuent is being added
                if(StudentID == 0)
                {
                    db.Students.Add(newStudent); 
                }

                // run insert in DB
                db.SaveChanges();

                // redirect to the updated students page
                Response.Redirect("~/Students.aspx");
            }
        }
    }
}