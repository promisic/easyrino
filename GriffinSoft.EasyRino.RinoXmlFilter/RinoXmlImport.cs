/*
 * RINO importer API class implementation
 * 
 * Copyright (C) 2016 - 2020 Dusan Misic <promisic@gmail.com>
 */

using System.Xml;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.RinoXmlFilter
{
    public class RinoXmlImport : IRinoImport
    {
        public XmlDocument ImportRinoObligationXml(string inPath)
        {
            var rinoXmlDoc = new XmlDocument();
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }

        public XmlDocument ImportRinoReconcilementXml(string inPath)
        {
            var rinoXmlDoc = new XmlDocument();
            rinoXmlDoc.Load(inPath);

            return rinoXmlDoc;
        }
    }
}