using System;

namespace Expense.Models
{
    public class Tickets //class
    {
        private string _name;
        private Status _status = Models.Status.Pending;

        public Tickets()
        {
        } //default constructor

        //assigned constructor
        public Tickets(int id, string name, int? managerId, int employeeId, double price, string date, string status)
        {
            Id = id;
            Name = name;
            ManagerId = managerId;
            EmployeeId = employeeId;
            Price = price;
            Date = date;
            Status = status;
        }

        //assigned constructor
        public Tickets(string name, double price, int employeeId, int? managerId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            Date = DateTime.Now.ToString();
            EmployeeId = employeeId;
            ManagerId = managerId;
        }

        //properties
        public int Id { get; set; }

        public string Name
        {
            get => _name; //because this get method don't do much action so //simple method
            set
            {
                if (string.IsNullOrEmpty(value) || value == " ")
                    throw new ArgumentException("The name of tickets can't be a null value or empty space");

                _name = value;
            }
        }
//the short way of writing getter and setter
        public int? ManagerId { get; set; }
        public int EmployeeId { get; set; }

        public double Price { get; set; }
        public string Date { get; set; }

        public string Status
        {
            get => _status.ToString();
            set => _status = (Status)Enum.Parse(typeof(Status), value, true);
        }

        public override string ToString() //override method to display the properties
        {
            return $"Name: {Name}\nTicket Price: {Price}\nDate: {Date}\nStatus: {Status}";
        }
    }
}