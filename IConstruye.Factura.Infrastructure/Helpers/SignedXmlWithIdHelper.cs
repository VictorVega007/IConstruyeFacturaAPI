using System.Security.Cryptography.Xml;
using System.Xml;

namespace IConstruye.Factura.Infrastructure.Helpers;

public class SignedXmlWithIdHelper(XmlDocument document) : SignedXml(document)
{
    public override XmlElement GetIdElement(XmlDocument document, string id)
    {
        var element = base.GetIdElement(document, id);
        if (element != null) return element;
        
        foreach (XmlElement xmlElement in document.GetElementsByTagName("*"))
        {
            if (xmlElement.HasAttribute("ID") && xmlElement.GetAttribute("ID") == id)
            {
                return xmlElement;
            }
        }
        
        return null;
    }
}