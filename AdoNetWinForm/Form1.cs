using AdoNetWinForm.Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;


namespace AdoNetWinForm
{
    public partial class UserForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AdoNetWinFormDB"].ConnectionString;
        SqlConnection conn;

        public UserForm()
        {
            InitializeComponent();
            this.Load += UserForm_Load;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            lblTotalCounter.Text = CountEntries().ToString();
            cbUserType.Items.Add("Personal User");
            cbUserType.Items.Add("Business User");
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            User user = CreateUser();
            conn = new SqlConnection(connectionString);
            AddUserToDatabase(user);
            lblTotalCounter.Text = CountEntries().ToString();
            txtResultViewer.Text = ViewUser(user);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtResultViewer.Text = string.Empty;
        }

        private string ViewUser(User user)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(user.FirstName);
            builder.Append(" ");
            builder.Append(user.LastName);
            builder.Append(" ");
            builder.Append(user.UserName);
            builder.Append(" ");
            builder.Append(user.UserType);
            builder.Append(" ");
            builder.Append(user.PhoneNumber);
            builder.Append(" ");
            builder.Append(user.Gender);

            return builder.ToString();
        }

        private User CreateUser()
        {
            User user = new User()
            {
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                UserName = txtUserName.Text,
                UserType = (UserType)cbUserType.SelectedIndex,
                PhoneNumber = txtPhone.Text,
                Gender = rbMale.Checked ? Gender.Male : Gender.Female
            };
            return user;
        }

        private void AddUserToDatabase(User user)
        {
            using (conn)
            {
                conn.Open();

                string query = "INSERT INTO Users " +
                               "VALUES (@firstname, @lastname, @username, @usertype, @phonenumber, @gender)";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@firstname", user.FirstName);
                    command.Parameters.AddWithValue("@lastname", user.LastName);
                    command.Parameters.AddWithValue("@username", user.UserName);
                    command.Parameters.AddWithValue("@usertype", user.UserType);
                    command.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                    command.Parameters.AddWithValue("@gender", user.Gender);

                    command.ExecuteNonQuery();
                }
            }
        }

        private string CountEntries()
        {
            conn = new SqlConnection(connectionString);
            string _count;
            using (conn)
            {
                conn.Open();

                string query = "SELECT COUNT(Id) FROM Users";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    _count = command.ExecuteScalar().ToString();
                }
            }
            return _count;
           
        }
        
        private void ShowAll()
        {
            conn = new SqlConnection(connectionString);
            using (conn)
            {
                conn.Open();

                string query = "SELECT * FROM Users";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    StringBuilder builder = new StringBuilder();
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        builder.Append("*********");
                        builder.Append(Environment.NewLine);
                        builder.Append("Id: ");
                        builder.Append(reader[0]);
                        builder.Append(Environment.NewLine);
                        builder.Append("First Name: ");
                        builder.Append(reader[1]);
                        builder.Append(Environment.NewLine);
                        builder.Append("Last Name: ");
                        builder.Append(reader[2]);
                        builder.Append(Environment.NewLine);
                        builder.Append("User Name: ");
                        builder.Append(reader[3]);
                        builder.Append(Environment.NewLine);
                        builder.Append("User Type: ");
                        builder.Append((UserType)reader[4]);
                        builder.Append(Environment.NewLine);
                        builder.Append("Phone: ");
                        builder.Append(reader[5]);
                        builder.Append(Environment.NewLine);
                        builder.Append("Gender: ");
                        builder.Append((Gender)reader[6]);
                        builder.Append(Environment.NewLine);
                        
                    }

                    txtResultViewer.Text = builder.ToString();
                }
            }
        }
    }
}
