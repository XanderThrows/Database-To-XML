/*************************************************************************/
/* Program Name:     DB2XML.cs                                         */
/* Date:             11/27/2023                                      */
/* Programmer:       Alex Hernandez                                           */
/* Class:            CSCI 234                                            */
/*                                                                       */
/* Program Description: DB2XML will read from a database and 
 *                      create an XML file.           */
/* Input: N/A                                                               */
/*                                                                       */
/* Output: Fills Objects                               */
/*                                                                       */
/* Givens:	None                                                         */
/*                                                                       */
/* Testing Data: See the Simple database                               */
/*************************************************************************/
using System;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;
// Must add a reference to System.Data.SQLite.dll (browse)

public class Customer
{
    public string CustID;
    public string FirstName;
    public string LastName;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public string ZipExt;
    public string AreaCode;
    public string Phone;
    public string CellAreaCode;
    public string CellPhone;
    public string EMail;
}

class simple
{
    public Customer[] CustRecords;
}

class DB2Class
{
    static string connectString;
    static string commandString;
    static SQLiteConnection connection = null;
    static SQLiteCommand sqlCmd;
    static Customer[] Records; // Allocated after we know how many records
    static simple cusTomer = new simple();

    /*************************************/
    /* Method to Connect to the database */
    /* Global input: connectString       */
    /* Global output: connection         */
    /*************************************/
    static void DBConnect()
    {
        try
        {
            connection = new SQLiteConnection(connectString);
            connection.Open();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }

    /*************************************/
    /* Method to Close the database      */
    /* Global input: connection          */
    /*************************************/
    static void DBClose()
    {
        if (connection != null) connection.Close();
    }

    static void FillClass()
    {
        SQLiteDataReader reader = null;
        int recordcount = 0;
        int i = 0;

        commandString = "SELECT * FROM Customer ";
        commandString += "ORDER BY CustID";

        try
        {
            sqlCmd = new SQLiteCommand();
            sqlCmd.CommandText = commandString;
            sqlCmd.Connection = connection;

            // Count the records
            reader = sqlCmd.ExecuteReader();
            while (reader.Read()) recordcount++;
            reader.Close();
            reader = null;

           
            Records = new Customer[recordcount];

            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                Records[i] = new Customer();
                Records[i].CustID = reader.GetString(0);
                Records[i].FirstName = reader.GetString(1);
                Records[i].LastName = reader.GetString(2);
                Records[i].Address = reader.GetString(3);
                Records[i].City = reader.GetString(4);
                Records[i].State = reader.GetString(5);
                Records[i].Zip = reader.GetString(6);
                Records[i].ZipExt = reader.GetString(7);
                Records[i].AreaCode = reader.GetString(8);
                Records[i].Phone = reader.GetString(9);
                Records[i].CellAreaCode = reader.GetString(10);
                Records[i].CellPhone = reader.GetString(11);
                Records[i].EMail = reader.GetString(12);
                i++;
            }

            cusTomer.CustRecords = Records;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.Write("Press <Ener> to contine: ");
            Console.ReadLine();
        }

        finally
        {
            if (reader != null) reader.Close();
        }
    }

    static void DisplayClass()
    {
        int i;

        for (i = 0; i < cusTomer.CustRecords.Length; i++)
        {
            Console.WriteLine($"CustID = {cusTomer.CustRecords[i].CustID}");
            Console.WriteLine($"FirstName = {cusTomer.CustRecords[i].FirstName}");
            Console.WriteLine($"LastName = {cusTomer.CustRecords[i].LastName}");
            Console.WriteLine($"Address = {cusTomer.CustRecords[i].Address}");
            Console.WriteLine($"City = {cusTomer.CustRecords[i].City}");
            Console.WriteLine($"State = {cusTomer.CustRecords[i].State}");
            Console.WriteLine($"Zip = {cusTomer.CustRecords[i].Zip}");
            Console.WriteLine($"ZipExt = {cusTomer.CustRecords[i].ZipExt}");
            Console.WriteLine($"AreaCode = {cusTomer.CustRecords[i].AreaCode}");
            Console.WriteLine($"Phone = {cusTomer.CustRecords[i].Phone}");
            Console.WriteLine($"CellAreaCode = {cusTomer.CustRecords[i].CellAreaCode}");
            Console.WriteLine($"CellPhone = {cusTomer.CustRecords[i].CellPhone}");
            Console.WriteLine($"Email = {cusTomer.CustRecords[i].EMail:f}");
            Console.WriteLine();
        }
    }

    static private void SerializeObject(string filename)
    {
        int j;
        // Create an instance of the XmlSerializer specifying type.
        XmlSerializer serializer = new XmlSerializer(typeof(Customer));

        // Create a TextWriter to write the file. 
        TextWriter writer = new StreamWriter(filename);

        if (cusTomer.CustRecords != null)
        {
            for (j = 0; j < cusTomer.CustRecords.Length; j++)
            {
                Customer i = new Customer();

                i.CustID = cusTomer.CustRecords[j].CustID;
                i.FirstName = cusTomer.CustRecords[j].FirstName;
                i.LastName = cusTomer.CustRecords[j].LastName;
                i.Address = cusTomer.CustRecords[j].Address;
                i.City = cusTomer.CustRecords[j].City;
                i.State = cusTomer.CustRecords[j].State;
                i.Zip = cusTomer.CustRecords[j].Zip;
                i.ZipExt = cusTomer.CustRecords[j].ZipExt;
                i.AreaCode = cusTomer.CustRecords[j].AreaCode;
                i.Phone = cusTomer.CustRecords[j].Phone;
                i.CellAreaCode = cusTomer.CustRecords[j].CellAreaCode;
                i.CellPhone = cusTomer.CustRecords[j].CellPhone;
                i.EMail = cusTomer.CustRecords[j].EMail;

                serializer.Serialize(writer, i);
            }
        }

        

        writer.Close();
    }

    static void Main(string[] args)
    {
        connectString = "Data Source=Simple.db";
        SerializeObject("Customer.xml");

        DBConnect();
        FillClass();
        DisplayClass();
        DBClose();
    }
}