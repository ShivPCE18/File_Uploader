using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace File_Uploader.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Approved
    {
        public bool value { get; set; }
    }

    public class Currency
    {
        public string value { get; set; }
    }

    public class CuryViewState
    {
        public bool value { get; set; }
    }

    public class Date
    {
        public DateTime value { get; set; }
    }

    public class Department
    {
        public string value { get; set; }
    }

    public class Description
    {
        public string value { get; set; }
    }

    public class Account
    {
        public string value { get; set; }
    }

    public class AlternateID
    {
        public string value { get; set; }
    }

    public class Canceled
    {
        public bool value { get; set; }
    }

    public class EstExtCost
    {
        public double value { get; set; }
    }

    public class EstUnitCost
    {
        public double value { get; set; }
    }

    public class Inventory
    {
        public string value { get; set; }
    }

    public class IssueStatus
    {
    }

    public class LineNbr
    {
        public int value { get; set; }
    }

    public class Location
    {
        public string value { get; set; }
    }

    public class ManualCost
    {
        public bool value { get; set; }
    }

    public class OrderQty
    {
        public double value { get; set; }
    }

    public class PromisedDate
    {
        public DateTime value { get; set; }
    }

    public class RefNbr
    {
        public string value { get; set; }
    }

    public class RequiredDate
    {
        public DateTime value { get; set; }
    }

    public class Sub
    {
        public string value { get; set; }
    }

    public class UOM
    {
        public string value { get; set; }
    }

    public class Vendor
    {
        public string value { get; set; }
    }

    public class VendorDescription
    {
    }

    public class VendorName
    {
        public string value { get; set; }
    }

    public class VendorRef
    {
    }

    public class Custom
    {
    }

    public class Details
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public string note { get; set; }
        public Account Account { get; set; }
        public AlternateID AlternateID { get; set; }
        public Canceled Canceled { get; set; }
        public Description Description { get; set; }
        public EstExtCost EstExtCost { get; set; }
        public EstUnitCost EstUnitCost { get; set; }
        public Inventory Inventory { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public LineNbr LineNbr { get; set; }
        public Location Location { get; set; }
        public ManualCost ManualCost { get; set; }
        public OrderQty OrderQty { get; set; }
        public PromisedDate PromisedDate { get; set; }
        public RefNbr RefNbr { get; set; }
        public RequiredDate RequiredDate { get; set; }
        public Sub Sub { get; set; }
        public UOM UOM { get; set; }
        public Vendor Vendor { get; set; }
        public VendorDescription VendorDescription { get; set; }
        public VendorName VendorName { get; set; }
        public VendorRef VendorRef { get; set; }
        public Custom custom { get; set; }
    }

    public class OpenQty
    {
        public double value { get; set; }
    }

    public class Priority
    {
        public string value { get; set; }
    }

    public class RequestClass
    {
        public string value { get; set; }
    }

    public class RequestedBy
    {
        public string value { get; set; }
    }

    public class Status
    {
        public string value { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public string note { get; set; }
        public Approved Approved { get; set; }
        public Currency Currency { get; set; }
        public CuryViewState CuryViewState { get; set; }
        public Date Date { get; set; }
        public Department Department { get; set; }
        public Description Description { get; set; }
        public Details Details { get; set; }
        public EstExtCost EstExtCost { get; set; }
        public Location Location { get; set; }
        public OpenQty OpenQty { get; set; }
        public Priority Priority { get; set; }
        public RefNbr RefNbr { get; set; }
        public RequestClass RequestClass { get; set; }
        public RequestedBy RequestedBy { get; set; }
        public Status Status { get; set; }
        public Custom custom { get; set; }
    }


}