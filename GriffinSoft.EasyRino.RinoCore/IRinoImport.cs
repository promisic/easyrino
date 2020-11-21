/*
 * RINO importer interface
 * 
 * Copyright (C) 2016 - 2020 Dusan Misic <promisic@gmail.com>
 */

using System.Xml;

namespace GriffinSoft.EasyRino.RinoCore
{
    public interface IRinoImport
    {
        /// <summary>
        /// Imports RINO obligation XML files
        /// </summary>
        /// <param name="inPath"></param>
        /// <returns></returns>
        XmlDocument ImportRinoObligationXml(string inPath);

        /// <summary>
        /// Imports RINO reconcilement XML file
        /// </summary>
        /// <param name="inPath">Input file path</param>
        /// <returns>XmlDocument object</returns>
        XmlDocument ImportRinoReconcilementXml(string inPath);
    }
}