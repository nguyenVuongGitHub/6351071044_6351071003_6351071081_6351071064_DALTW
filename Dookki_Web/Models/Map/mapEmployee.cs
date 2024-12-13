using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.Models.Map
{
    public class mapEmployee
    {
        //DOOKKIEntities db = new DOOKKIEntities();
        ////1. List
        //public List<Employee> listEmployees()
        //{
        //    try
        //    {
        //        var listEmployee = db.Employees.OrderBy(m=>m.Name).ToList(); 
        //        return listEmployee;
        //    }
        //    catch 
        //    {
        //        return new List<Employee> ();
        //    }
        //}
        ////2. Details
        //public Employee detailsEmployees(int idEmployee)
        //{
        //    try
        //    {
        //        return db.Employees.Find(idEmployee);
        //    }
        //    catch
        //    {
        //        return new Employee();
        //    }
        //}
        ////3. Add: Add will retrun id
        //public int addNewEmployees(Employee newModel)
        //{
        //    try
        //    {
        //        db.Employees.Add(newModel);
        //        db.SaveChanges();
        //        return newModel.ID;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}
        ////4. Update
        //public bool updateEmployees(Employee upModel)
        //{
        //    try
        //    {
        //        //1. Tim doi tuong can cap nhat
        //        var employee = db.Employees.Find(upModel.ID);
        //        employee.Name = upModel.Name;
        //        employee.phone = upModel.phone;
        //        employee.email = upModel.email;
        //        employee.amountWage = upModel.amountWage;
        //        employee.position = upModel.position;
        //        db.SaveChanges ();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        ////5. Delete
        //public bool deleteEmployees(int idEmployee)
        //{
        //    try
        //    {
        //        var employee = db.Employees.Find(idEmployee);
        //        db.Employees.Remove(employee);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false ;
        //    }
        //}
    }
}