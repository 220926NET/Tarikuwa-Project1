using System;
using System.Collections.Generic;

namespace Expense.Models
{
    public class User //class user
    {
        
        private string _address = "";
        private string _email = "";

        private string _fullname = "";

        private string _password = "";

        private string _phone = "";

        private Role
            _role = Models.Role
                .Employee; //go to models and then find method role and use the role manager and then assign to role role

        public User() //default constructor
        {
        }

        public User(string email, string password) //constructor with two parametrs
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public User(int id, string email, string password, string phone, string fullName, string address,
            string role) //constructor with lots of parametrs
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            FullName = fullName;
            Id = id;
            Address = address;
            Role = role;
        }

        public User(string email, string password, string phone, string fullName) //constructor with some parametrs
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        public int Id { get; set; }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value) || value == " ")
                    throw new ArgumentException("The email can't be a null value or empty space");

                _email = value;
            }
        }

        public string Password
        {
            get => _password;
            set //validating before assigning the value and it is used to set the value if it passed the validation 
            {
                if (string.IsNullOrEmpty(value) || value == " ")
                    throw new ArgumentException("The password can't be a null value or empty space");
                _password = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set //validating the value before assigning and it is setter if the value passed the validation
            {
                if (string.IsNullOrEmpty(value) || value == " ")
                    throw new ArgumentException("The phone can't be a null value or empty space");

                _phone = value;
            }
        }

        public string FullName { get; set; }

        public string Address
        {
            get => _address;
            set //validating the value before assigning and it is setter if the value passed the validation
            {
                if (value == null)
                    _address = "";
                else
                    _fullname = value;
            }
        }

        public virtual ICollection<Tickets>
            TicketsCollection { get; set; } //defines to manipulate generic collection and it is Tickets

        public string Role
        {
            get => _role
                .ToString(); // shorter way of method writing //override method which is Tostring and it does display what we pass
            set => _role =
                (Role)Enum.Parse(typeof(Role), value,
                    true); //=> other way of writing shorter method this set the role to the role selected by the user
        }

        public override string ToString() //override method which is Tostring and it does display what we pass
        {
            return
                $"Email: {Email}\nPassword: {Password}\nPhone Number: {Phone}\nFull Name: {FullName}\nAddress: {Address}\nRole: {Role}";
        }
    }
}