using System.Configuration;
using System.Data;
using System.Data.SqlClient;//install from Nuget
using System.Drawing.Drawing2D;
using System.Xml.Linq;

///
///https://www.dotnetperls.com/sqlclient
///

string conStr = ConfigurationManager.ConnectionStrings["DefConnString"].ConnectionString; 

if (conStr != null)
{
    if (CheckConnection(conStr))
    {
        Console.WriteLine("Connection opened");
        ReadCustomers();
    }
    else 
    {
        Console.WriteLine("Connection error!");
    }
}

bool CheckConnection(string conStr)
{
    bool res = true;

    using (SqlConnection con = new(conStr))
    {
        try
        {
            con.Open();
            res = con.State == System.Data.ConnectionState.Open;
        }
        catch
        {
            res = false;
        }
    }
    return res;
}

void ReadCustomers()
{
    List<Customer> customers = new();

    using (SqlConnection con = new(conStr))
    {
        try
        {
            con.Open();
            using (SqlCommand command = new SqlCommand("SELECT Top 10 CustomerID, CompanyName, ContactName, Country FROM Customers", con))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string custID = reader["CustomerID"] == DBNull.Value ? "" : reader["CustomerID"].ToString();//reader.GetString(0);
                    string compName = reader["CompanyName"].ToString();//reader.GetString(1);
                    string contact = reader.GetString(2);
                    string country = reader.GetString(3); // Breed string
                    customers.Add(new Customer() { CustomerID = custID, CompanyName = compName, ContactName = contact, Country = country });
                }
                reader.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
    foreach (Customer cust in customers)
    {
        Console.WriteLine($"{cust.CustomerID} {cust.CompanyName} {cust.ContactName} {cust.Country}");
    }
}

void InsertCustomer()
{
    using (SqlConnection con = new(conStr))
    {
        try
        {
            con.Open();
            using (SqlCommand command = new SqlCommand("insert into Customers (CustomerID, CompanyName, ContactName, Country) " +
                                                        "values (@CustomerID, @CompanyName, @ContactName, @Country)", con))
            {
                command.Parameters.Add(new SqlParameter("CustomerID", "NewID"));
                command.Parameters.Add(new SqlParameter("CompanyName", "CareGo"));
                command.Parameters.Add(new SqlParameter("ContactName", "Vlad"));
                command.Parameters.Add(new SqlParameter("Country", "Canada"));
                //or
                //command.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.VarChar)).Value = "New Id";

                command.ExecuteNonQuery();
            }
        }
        catch (SqlException err)
        {
            Console.WriteLine("Sql error: " + err.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Generic error: " + e.Message);
        }
        finally
        {
            con.Close();
        }
    }
}