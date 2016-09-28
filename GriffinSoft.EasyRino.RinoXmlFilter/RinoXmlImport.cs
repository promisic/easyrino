/*
 * RINO importer API class implementation
 * 
 * Copyright: Dusan Misic 2016 <promisic@outlook.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.RinoXmlFilter
{
    public class RinoXmlImport : IRinoImport
    {
        public XmlDocument ImportRinoObligationXml(string inPath)
        {
            // Creating XmlDocument object
            XmlDocument rinoXmlDoc = new XmlDocument();

            // Loading XML file into memory
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }

        public XmlDocument ImportRinoReconcilementXml(string inPath)
        {
            // Creating XmlDocument object
            XmlDocument rinoXmlDoc = new XmlDocument();

            // Loading XML file into memory
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }
    }
}
