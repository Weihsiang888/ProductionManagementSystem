using System.Xml.Serialization;

namespace DxBlazorApplication7.Data
{
    public class WorkOrderList
    {
        [XmlRoot(ElementName = "Status")]
        public class Status
        {
            [XmlAttribute(AttributeName = "code")]
            public string Code { get; set; }
            [XmlAttribute(AttributeName = "sqlcode")]
            public string Sqlcode { get; set; }
            [XmlAttribute(AttributeName = "description")]
            public string Description { get; set; }
        }

        [XmlRoot(ElementName = "Execution")]
        public class Execution
        {
            [XmlElement(ElementName = "Status")]
            public Status Status { get; set; }
        }

        [XmlRoot(ElementName = "Field")]
        public class Field
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "value")]
            public string Value { get; set; }
        }

        [XmlRoot(ElementName = "Record")]
        public class Record
        {
            [XmlElement(ElementName = "Field")]
            public List<Field> Field { get; set; }
        }

        [XmlRoot(ElementName = "Master")]
        public class Master
        {
            [XmlElement(ElementName = "Record")]
            public Record Record { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "Detail")]
        public class Detail
        {
            [XmlElement(ElementName = "Record")]
            public List<Record> Record { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "RecordSet")]
        public class RecordSet
        {
            [XmlElement(ElementName = "Master")]
            public Master Master { get; set; }
            [XmlElement(ElementName = "Detail")]
            public List<Detail> Detail { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
        }

        [XmlRoot(ElementName = "Document")]
        public class Document
        {
            [XmlElement(ElementName = "RecordSet")]
            public List<RecordSet> RecordSet { get; set; }
        }

        [XmlRoot(ElementName = "ResponseContent")]
        public class ResponseContent
        {
            [XmlElement(ElementName = "Parameter")]
            public string Parameter { get; set; }
            [XmlElement(ElementName = "Document")]
            public Document Document { get; set; }
        }

        [XmlRoot(ElementName = "Response")]
        public class Response
        {
            [XmlElement(ElementName = "Execution")]
            public Execution Execution { get; set; }
            [XmlElement(ElementName = "ResponseContent")]
            public ResponseContent ResponseContent { get; set; }
        }
    }
}
