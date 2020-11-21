/*
 *  RINO exporter interface
 *  Copyright (C) 2016 - 2020 Dusan Misic <promisic@gmail.com>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Xml;

namespace GriffinSoft.EasyRino.RinoCore
{
    public interface IRinoExporter
    {
        /// <summary>
        /// Exports RINO obligation XML file
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <param name="outPath">Output file path</param>
        void ExportRinoObligationXml(XmlDocument rinoXmlDoc, string outPath);

        /// <summary>
        /// Exports RINO reconcilement XML file
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <param name="outPath">Output file path</param>
        void ExportRinoReconcilementXml(XmlDocument rinoXmlDoc, string outPath);
    }
}