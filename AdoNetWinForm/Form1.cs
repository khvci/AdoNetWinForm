using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using AdoNetWinForm.Entities;


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
            cbUserType.Items.Add("Personal User");
            cbUserType.Items.Add("Business User");
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            User user = CreateUser();
            conn = new SqlConnection(connectionString);
            AddUserToDatabase(user);

            lbResultViewer.Items.Add(ViewUser(user));
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
    }
}
