/*
 * RINO importer API class implementation
 * 
 * Copyright: Dusan Misic 2016 - 2019 <promisic@outlook.com>
 */

using System.Xml;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.RinoXmlFilter
{
    public class RinoXmlImport : IRinoImport
    {
        public XmlDocument ImportRinoObligationXml(string inPath)
        {
            // Creating XmlDocument object
            var rinoXmlDoc = new XmlDocument();

            // Loading XML file into memory
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }

        public XmlDocument ImportRinoReconcilementXml(string inPath)
        {
            // Creating XmlDocument object
            var rinoXmlDoc = new XmlDocument();

            // Loading XML file into memory
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }
    }
}