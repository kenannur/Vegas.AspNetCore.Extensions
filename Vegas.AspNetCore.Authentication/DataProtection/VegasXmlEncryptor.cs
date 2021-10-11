using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace Vegas.AspNetCore.Authentication.DataProtection
{
    public class VegasXmlEncryptor : IXmlEncryptor
    {
        public EncryptedXmlInfo Encrypt(XElement plaintextElement)
        {
            var newElement = new XElement("unencryptedKey",
                                          new XComment(" This key is not encrypted. "),
                                          new XElement(plaintextElement));

            return new EncryptedXmlInfo(newElement, typeof(VegasXmlDecryptor));
        }
    }
}
