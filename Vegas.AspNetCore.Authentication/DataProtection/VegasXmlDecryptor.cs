using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace Vegas.AspNetCore.Authentication.DataProtection
{
    public class VegasXmlDecryptor : IXmlDecryptor
    {
        public XElement Decrypt(XElement encryptedElement)
        {
            return new XElement(encryptedElement.Elements().Single());
        }
    }
}
