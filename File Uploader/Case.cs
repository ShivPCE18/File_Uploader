using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace File_Uploader
{
    

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Attribute2
    {
        public string value { get; set; }
    }

    public class AttributeID
    {
        public string value { get; set; }
    }

    public class RefNoteID
    {
        public string value { get; set; }
    }

    public class Required
    {
    }

    public class Value
    {
        public string value { get; set; }
    }

    public class Custom
    {
    }

    public class _Attribute
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public string note { get; set; }
        public Attribute2 Attribute { get; set; }
        public AttributeID AttributeID { get; set; }
        public RefNoteID RefNoteID { get; set; }
        public Required Required { get; set; }
        public Value Value { get; set; }
        public Custom custom { get; set; }
    }

    public class Billable
    {
        public bool value { get; set; }
    }

    public class BillableOvertime
    {
        public int value { get; set; }
    }

    public class BillableTime
    {
        public int value { get; set; }
    }

    public class BusinessAccount
    {
        public string value { get; set; }
    }

    public class BusinessAccountName
    {
        public string value { get; set; }
    }

    public class CaseID
    {
        public string value { get; set; }
    }

    public class ClassID
    {
        public string value { get; set; }
    }

    public class ClosingDate
    {
    }

    public class ContactDisplayName
    {
        public string value { get; set; }
    }

    public class ContactID
    {
        public int value { get; set; }
    }

    public class Contract
    {
    }

    public class DateReported
    {
        public DateTime value { get; set; }
    }

    public class Description
    {
        public string value { get; set; }
    }

    public class InitialResponse
    {
        public string value { get; set; }
    }

    public class LastActivityDate
    {
    }

    public class LastIncomingActivity
    {
    }

    public class LastOutgoingActivity
    {
    }

    public class Location
    {
        public string value { get; set; }
    }

    public class ManualOverride
    {
        public bool value { get; set; }
    }

    public class OvertimeSpent
    {
        public string value { get; set; }
    }

    public class Owner
    {
    }

    public class OwnerEmployeeName
    {
    }

    public class Priority
    {
        public string value { get; set; }
    }

    public class Reason
    {
        public string value { get; set; }
    }

    public class ResolutionTime
    {
        public string value { get; set; }
    }

    public class Severity
    {
        public string value { get; set; }
    }

    public class SLA
    {
        public DateTime value { get; set; }
    }

    public class Status
    {
        public string value { get; set; }
    }

    public class Subject
    {
        public string value { get; set; }
    }

    public class TimeSpent
    {
        public string value { get; set; }
    }

    public class Workgroup
    {
    }

    public class WorkgroupDescription
    {
    }

    public class Case
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public string note { get; set; }
        public List<_Attribute> Attributes { get; set; }
        public Billable Billable { get; set; }
        public BillableOvertime BillableOvertime { get; set; }
        public BillableTime BillableTime { get; set; }
        public BusinessAccount BusinessAccount { get; set; }
        public BusinessAccountName BusinessAccountName { get; set; }
        public CaseID CaseID { get; set; }
        public ClassID ClassID { get; set; }
        public ClosingDate ClosingDate { get; set; }
        public ContactDisplayName ContactDisplayName { get; set; }
        public ContactID ContactID { get; set; }
        public Contract Contract { get; set; }
        public DateReported DateReported { get; set; }
        public Description Description { get; set; }
        public InitialResponse InitialResponse { get; set; }
        public LastActivityDate LastActivityDate { get; set; }
        public LastIncomingActivity LastIncomingActivity { get; set; }
        public LastOutgoingActivity LastOutgoingActivity { get; set; }
        public Location Location { get; set; }
        public ManualOverride ManualOverride { get; set; }
        public OvertimeSpent OvertimeSpent { get; set; }
        public Owner Owner { get; set; }
        public OwnerEmployeeName OwnerEmployeeName { get; set; }
        public Priority Priority { get; set; }
        public Reason Reason { get; set; }
        public ResolutionTime ResolutionTime { get; set; }
        public Severity Severity { get; set; }
        public SLA SLA { get; set; }
        public Status Status { get; set; }
        public Subject Subject { get; set; }
        public TimeSpent TimeSpent { get; set; }
        public Workgroup Workgroup { get; set; }
        public WorkgroupDescription WorkgroupDescription { get; set; }
        public Custom custom { get; set; }
    }



}

namespace version20
{
    public class Note
    {
        public string value { get; set; }
    }

    public class Attribute2
    {
        public string value { get; set; }
    }
    public class AttributeDescription
    {
        public string value { get; set; }
    }

    public class AttributeID
    {
        public string value { get; set; }
    }

    public class RefNoteID
    {
        public string value { get; set; }
    }

    public class Required
    {
    }

    public class Value
    {
        public string value { get; set; }
    }

    public class ValueDescription
    {
        public string value { get; set; }
    }

    public class Custom
    {
    }

    public class _Attribute
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public object note { get; set; }
        public Attribute2 Attribute { get; set; }
        public AttributeDescription AttributeDescription { get; set; }
        public AttributeID AttributeID { get; set; }
        public RefNoteID RefNoteID { get; set; }
        public Required Required { get; set; }
        public Value Value { get; set; }
        public ValueDescription ValueDescription { get; set; }
        public Custom custom { get; set; }
    }

    public class Billable
    {
        public bool value { get; set; }
    }

    public class BillableOvertime
    {
        public int value { get; set; }
    }

    public class BillableTime
    {
        public int value { get; set; }
    }

    public class BusinessAccount
    {
        public string value { get; set; }
    }

    public class BusinessAccountName
    {
        public string value { get; set; }
    }

    public class CaseID
    {
        public string value { get; set; }
    }

    public class ClassID
    {
        public string value { get; set; }
    }

    public class ClosingDate
    {
    }

    public class ContactDisplayName
    {
        public string value { get; set; }
    }

    public class ContactID
    {
        public int value { get; set; }
    }

    public class Contract
    {
    }

    public class DateReported
    {
        public DateTime value { get; set; }
    }

    public class Description
    {
        public string value { get; set; }
    }

    public class InitialResponse
    {
        public string value { get; set; }
    }

    public class LastActivityDate
    {
    }

    public class LastIncomingActivity
    {
    }

    public class LastModifiedDateTime
    {
        public DateTime value { get; set; }
    }

    public class LastOutgoingActivity
    {
    }

    public class Location
    {
        public string value { get; set; }
    }

    public class ManualOverride
    {
        public bool value { get; set; }
    }

    public class NoteID
    {
        public string value { get; set; }
    }

    public class OvertimeSpent
    {
        public string value { get; set; }
    }

    public class Owner
    {
    }

    public class OwnerEmployeeName
    {
    }

    public class Priority
    {
        public string value { get; set; }
    }

    public class Reason
    {
        public string value { get; set; }
    }

    public class ResolutionTime
    {
        public string value { get; set; }
    }

    public class Severity
    {
        public string value { get; set; }
    }

    public class SLA
    {
        public DateTime value { get; set; }
    }

    public class Status
    {
        public string value { get; set; }
    }

    public class Subject
    {
        public string value { get; set; }
    }

    public class TimeSpent
    {
        public string value { get; set; }
    }

    public class Workgroup
    {
    }

    public class WorkgroupDescription
    {
    }

    public class Case
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public Note note { get; set; }
        public List<_Attribute> Attributes { get; set; }
        public Billable Billable { get; set; }
        public BillableOvertime BillableOvertime { get; set; }
        public BillableTime BillableTime { get; set; }
        public BusinessAccount BusinessAccount { get; set; }
        public BusinessAccountName BusinessAccountName { get; set; }
        public CaseID CaseID { get; set; }
        public ClassID ClassID { get; set; }
        public ClosingDate ClosingDate { get; set; }
        public ContactDisplayName ContactDisplayName { get; set; }
        public ContactID ContactID { get; set; }
        public Contract Contract { get; set; }
        public DateReported DateReported { get; set; }
        public Description Description { get; set; }
        public InitialResponse InitialResponse { get; set; }
        public LastActivityDate LastActivityDate { get; set; }
        public LastIncomingActivity LastIncomingActivity { get; set; }
        public LastModifiedDateTime LastModifiedDateTime { get; set; }
        public LastOutgoingActivity LastOutgoingActivity { get; set; }
        public Location Location { get; set; }
        public ManualOverride ManualOverride { get; set; }
        public NoteID NoteID { get; set; }
        public OvertimeSpent OvertimeSpent { get; set; }
        public Owner Owner { get; set; }
        public OwnerEmployeeName OwnerEmployeeName { get; set; }
        public Priority Priority { get; set; }
        public Reason Reason { get; set; }
        public ResolutionTime ResolutionTime { get; set; }
        public Severity Severity { get; set; }
        public SLA SLA { get; set; }
        public Status Status { get; set; }
        public Subject Subject { get; set; }
        public TimeSpent TimeSpent { get; set; }
        public Workgroup Workgroup { get; set; }
        public WorkgroupDescription WorkgroupDescription { get; set; }
        public Custom custom { get; set; }
    }
}

