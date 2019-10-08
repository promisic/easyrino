/*
 *  RINO obligation manager class
 *  Copyright (C) 2016 - 2019  Dusan Misic <promisic@outlook.com>
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.Core
{
    internal class RinoObligationManager
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        internal RinoObligationManager()
        {
            // Filling datatable with columns
            FillDataTableWithColumns();
        }

        #region Convert XML to Rino List region

        /// <summary>
        ///     Converts XmlDocument object to RinoObligationItem object.
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <returns>List of RinoObligationItem objects.</returns>
        internal List<RinoObligationItem> ConvertXmlToRinoList(XmlDocument rinoXmlDoc)
        {
            // Creating RinoObligationItem list object
            var roi = new List<RinoObligationItem>();

            // Getting RINO header nodes
            var rinoXmlHeaderNodes = rinoXmlDoc.SelectNodes("//RINO");

            if (rinoXmlHeaderNodes != null)
                foreach (XmlNode xmlNode in rinoXmlHeaderNodes)
                {
                    var jbbkNode = xmlNode.SelectSingleNode("JBBK");
                    var typeNode = xmlNode.SelectSingleNode("Tip");

                    // Setting JBBK
                    if (jbbkNode != null) Jbbk = jbbkNode.InnerText;

                    if (typeNode != null) ValidObligation = typeNode.InnerText == "Obaveza";
                }

            // Execute parsing stage ONLY if it is valid obligation XML file
            if (ValidObligation)
            {
                // Doing XPath search query
                var xmlNodes = rinoXmlDoc.SelectNodes("//RINO/Obaveze/Obaveza");

                // Iterating each XmlNode
                if (xmlNodes != null)
                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        // Creating ROI object to add to existing list
                        var roiObj = new RinoObligationItem();

                        // Checking type of current XmlNode
                        if (xmlNode.Attributes != null)
                        {
                            var xmlNodeValue = xmlNode.Attributes["VrstaPosla"].Value;

                            switch (xmlNodeValue)
                            {
                                case "U":
                                    // Inserting new item
                                    roiObj.Action = RinoActionType.Unos;
                                    break;
                                case "I":
                                    // Changing existing item
                                    roiObj.Action = RinoActionType.Izmena;
                                    break;
                                case "O":
                                    // Deleting existing item
                                    roiObj.Action = RinoActionType.Otkazivanje;
                                    break;
                            }
                        }

                        // Getting ammount
                        var iznosNode = xmlNode.SelectSingleNode("Iznos");

                        // Setting amount
                        if (iznosNode != null)
                            roiObj.Iznos = decimal.Parse(iznosNode.InnerText,
                                CultureInfo.CreateSpecificCulture("en-US"));

                        // Getting name
                        var nazivNode = xmlNode.SelectSingleNode("NazivPoverioca");

                        // Setting name
                        if (nazivNode != null) roiObj.NazivPoverioca = nazivNode.InnerText;

                        // Getting PIB ID
                        var pibNode = xmlNode.SelectSingleNode("PIBPoverioca");

                        // Setting PIB ID
                        if (pibNode != null) roiObj.PibPoverioca = pibNode.InnerText;

                        // Getting MB ID
                        var mbNode = xmlNode.SelectSingleNode("MBPoverioca");

                        // Setting MB ID
                        if (mbNode != null) roiObj.MbPoverioca = mbNode.InnerText;

                        // Getting VrstaPoverioca
                        var vpNode = xmlNode.SelectSingleNode("VrstaPoverioca");

                        // Parsing and setting VrstaPoverioca
                        if (vpNode != null)
                            switch (vpNode.InnerText)
                            {
                                case "0":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                                    break;
                                case "1":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
                                    break;
                                case "8":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
                                    break;
                                case "9":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
                                    break;
                                default:
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                                    break;
                            }

                        // Getting document name
                        var ndNode = xmlNode.SelectSingleNode("NazivDokumenta");

                        // Setting document name
                        if (ndNode != null) roiObj.NazivDokumenta = ndNode.InnerText;

                        // Getting document number
                        var bdNode = xmlNode.SelectSingleNode("BrojDokumenta");

                        // Setting document number
                        if (bdNode != null) roiObj.BrojDokumenta = bdNode.InnerText;

                        // Getting document creation date
                        var ddNode = xmlNode.SelectSingleNode("DatumDokumenta");

                        // Setting document creation date
                        roiObj.DatumDokumenta = DateTime.ParseExact(ddNode.InnerText, "yyyy-MM-dd", null);

                        // Getting document obligation start date
                        var dnNode = xmlNode.SelectSingleNode("DatumNastanka");

                        // Setting document obligation start date
                        roiObj.DatumNastanka = DateTime.ParseExact(dnNode.InnerText, "yyyy-MM-dd", null);

                        // This node may be missing / is optional.
                        if (xmlNode.SelectSingleNode("DatumRokaZaIzmirenje") != null)
                        {
                            // Getting document payment due date
                            var driNode = xmlNode.SelectSingleNode("DatumRokaZaIzmirenje");

                            // Setting document payment due date
                            roiObj.DatumRokaZaIzmirenje = DateTime.ParseExact(driNode.InnerText, "yyyy-MM-dd", null);
                        }

                        // This node may be missing / is optional.
                        if (xmlNode.SelectSingleNode("RazlogIzmene") != null)
                        {
                            // Getting reason for change
                            var riNode = xmlNode.SelectSingleNode("RazlogIzmene");

                            // Setting reason for change
                            roiObj.RazlogIzmene = riNode.InnerText;
                        }

                        // Adding roiObj object to roi
                        roi.Add(roiObj);
                    }
            }

            return roi;
        }

        #endregion

        #region Convert Rino List to XML region

        /// <summary>
        ///     Converts list of RinoObligationItem's to RINO XmlDocument object
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's object</param>
        /// <returns>RINO XmlDocument object</returns>
        internal XmlDocument ConvertRinoListToXml(List<RinoObligationItem> roiList)
        {
            // Creating RINO XML object
            var rinoXml = new XmlDocument();

            // Creating XML declaration
            var xmlOrderDecl = rinoXml.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            // Creating RINO root node
            XmlNode rinoRootNode = rinoXml.CreateElement("RINO");
            // Append node
            rinoXml.AppendChild(rinoRootNode);

            // Inserting XML declaration
            rinoXml.InsertBefore(xmlOrderDecl, rinoRootNode);

            // Creating JBBK node
            XmlNode rinoJbbkNode = rinoXml.CreateElement("JBBK");
            // Setting JBBK value
            rinoJbbkNode.InnerText = Jbbk;

            // Append node
            rinoRootNode.AppendChild(rinoJbbkNode);

            // Creating Tip node
            XmlNode rinoTipNode = rinoXml.CreateElement("Tip");
            // Setting Tip value
            rinoTipNode.InnerText = "Obaveza";
            // Append node
            rinoRootNode.AppendChild(rinoTipNode);

            // Creating Obaveze node
            XmlNode rinoObavezeNode = rinoXml.CreateElement("Obaveze");
            // Append node
            rinoRootNode.AppendChild(rinoObavezeNode);

            // Iterating through roiList object
            foreach (var roiItem in roiList)
            {
                // Creating Obaveza node
                XmlNode rinoObavezaNode = rinoXml.CreateElement("Obaveza");
                // Creating attribute
                var rinoObavezaAttr = rinoXml.CreateAttribute("VrstaPosla");
                // Setting attribute value
                string vrstaPosla = null;

                switch (roiItem.Action)
                {
                    case RinoActionType.Unos:
                        vrstaPosla = "U";
                        break;
                    case RinoActionType.Izmena:
                        vrstaPosla = "I";
                        break;
                    case RinoActionType.Otkazivanje:
                        vrstaPosla = "O";
                        break;
                }

                rinoObavezaAttr.Value = vrstaPosla;
                // Appending attribute
                rinoObavezaNode.Attributes?.Append(rinoObavezaAttr);
                // Append node
                rinoObavezeNode.AppendChild(rinoObavezaNode);

                // Creating Iznos node
                XmlNode rinoIznosNode = rinoXml.CreateElement("Iznos");
                // Setting Iznos value
                rinoIznosNode.InnerText = roiItem.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                // Append node
                rinoObavezaNode.AppendChild(rinoIznosNode);

                // Creating NazivPoverioca
                XmlNode rinoNpNode = rinoXml.CreateElement("NazivPoverioca");
                // Setting NazivPoverioca value
                rinoNpNode.InnerText = roiItem.NazivPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoNpNode);

                // Creating PIBPoverioca node
                XmlNode rinoPibNode = rinoXml.CreateElement("PIBPoverioca");
                // Setting PIBPoverica value
                rinoPibNode.InnerText = roiItem.PibPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoPibNode);

                // Creating MBPoverioca node
                XmlNode rinoMbNode = rinoXml.CreateElement("MBPoverioca");
                // Setting MBPoverioca value
                rinoMbNode.InnerText = roiItem.MbPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoMbNode);

                // Creating VrstaPoverioca node
                XmlNode rinoVpNode = rinoXml.CreateElement("VrstaPoverioca");
                // Setting VrstaPosla value
                string vrstaPoverioca = null;

                switch (roiItem.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        vrstaPoverioca = "0";
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        vrstaPoverioca = "1";
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        vrstaPoverioca = "8";
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        vrstaPoverioca = "9";
                        break;
                }

                rinoVpNode.InnerText = vrstaPoverioca ?? throw new InvalidOperationException();
                // Append node
                rinoObavezaNode.AppendChild(rinoVpNode);

                // Creating NazivDokumenta node
                XmlNode rinoNdNode = rinoXml.CreateElement("NazivDokumenta");
                // Setting NazivDokumenta value
                rinoNdNode.InnerText = roiItem.NazivDokumenta;
                // Append node
                rinoObavezaNode.AppendChild(rinoNdNode);

                // Creating BrojDokumenta node
                XmlNode rinoBdNode = rinoXml.CreateElement("BrojDokumenta");
                // Setting BrojDokumenta value
                rinoBdNode.InnerText = roiItem.BrojDokumenta;
                // Append node
                rinoObavezaNode.AppendChild(rinoBdNode);

                // Creating DatumDokumenta node
                XmlNode rinoDdNode = rinoXml.CreateElement("DatumDokumenta");
                // Setting DatumDokumenta value
                rinoDdNode.InnerText = roiItem.DatumDokumenta.ToString("yyyy-MM-dd");
                // Append node
                rinoObavezaNode.AppendChild(rinoDdNode);

                // Creating DatumNastanka node
                XmlNode rinoDnNode = rinoXml.CreateElement("DatumNastanka");
                // Setting DatumNastanka value
                rinoDnNode.InnerText = roiItem.DatumNastanka.ToString("yyyy-MM-dd");
                // Append node
                rinoObavezaNode.AppendChild(rinoDnNode);

                // Checking for optional DatumRokaZaIzmirenje
                if (CheckUninitializedDate(roiItem.DatumRokaZaIzmirenje) != true)
                {
                    // Creating DatumRokaZaIzmirenje node
                    XmlNode rinoDrziNode = rinoXml.CreateElement("DatumRokaZaIzmirenje");
                    // Setting DatumRokaZaIzmirenje value
                    rinoDrziNode.InnerText = roiItem.DatumRokaZaIzmirenje.ToString("yyyy-MM-dd");
                    // Append node
                    rinoObavezaNode.AppendChild(rinoDrziNode);
                }

                // Checking for optional RazlogIzmene
                if (roiItem.RazlogIzmene != null)
                    if (roiItem.RazlogIzmene.Length > 0)
                    {
                        // Creating RazlogIzmene node
                        XmlNode rinoRiNode = rinoXml.CreateElement("RazlogIzmene");
                        // Setting RazlogIzmene value
                        rinoRiNode.InnerText = roiItem.RazlogIzmene;
                        // Append node
                        rinoObavezaNode.AppendChild(rinoRiNode);
                    }
            }

            return rinoXml;
        }

        #endregion

        #region Utility method region

        /// <summary>
        ///     Checks for uninitialized date.
        /// </summary>
        /// <param name="dateToCheck">DateTime object to check.</param>
        /// <returns>True if date is "default uninitialized one", fasle if otherwise.</returns>
        public bool CheckUninitializedDate(DateTime dateToCheck)
        {
            // Creating DateTime object similar to uninitialized datetime
            var uninDateTime = new DateTime(0001, 1, 1, 0, 0, 0); // Target date is: 1.1.0001. 00.00.00

            // Comparing dates
            var dateCompResult = DateTime.Compare(dateToCheck, uninDateTime);

            if (dateCompResult == 0) return true;

            return false;
        }

        #endregion

        #region Internal DataTable and properties region

        /// <summary>
        ///     Property to get RINO data table.
        /// </summary>
        public DataTable RinoDataTable { get; private set; }

        /// <summary>
        ///     Property to get or set JBBK ID.
        /// </summary>
        public string Jbbk { get; set; }

        /// <summary>
        ///     Property to get or set valid obligation field.
        /// </summary>
        public bool ValidObligation { get; private set; }

        #endregion

        #region DataTable manipulation methods region

        /// <summary>
        ///     Clears all rows inside internal DataTable object.
        /// </summary>
        private void ClearDataTableRows()
        {
            RinoDataTable.Rows.Clear();
        }

        /// <summary>
        ///     Fill's interal DataTable object with columns.
        /// </summary>
        private void FillDataTableWithColumns()
        {
            // Adding columns to DataTable object
            RinoDataTable.Columns.Add("rinoAction", typeof(string));
            RinoDataTable.Columns.Add("rinoIznos", typeof(decimal));
            RinoDataTable.Columns.Add("rinoNazivPoverioca", typeof(string));
            RinoDataTable.Columns.Add("rinoPIB", typeof(string));
            RinoDataTable.Columns.Add("rinoMB", typeof(string));
            RinoDataTable.Columns.Add("rinoVrstaPoverioca", typeof(string));
            RinoDataTable.Columns.Add("rinoNazivDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoBrojDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumNastanka", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumRokaZaIzmirenje", typeof(string));
            RinoDataTable.Columns.Add("rinoRazlogIzmene", typeof(string));
        }

        /// <summary>
        ///     Converts list of RinoObligationItem's objects to DataTable rows.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        public void ConvertRinoListToDataTable(List<RinoObligationItem> roiList)
        {
            // Clearing RINO DataTable object
            ClearDataTableRows();

            foreach (var listItem in roiList)
            {
                // RINO action
                var rinoAction = "";

                switch (listItem.Action)
                {
                    case RinoActionType.Unos:
                        rinoAction = "Unos";
                        break;
                    case RinoActionType.Izmena:
                        rinoAction = "Izmena";
                        break;
                    case RinoActionType.Otkazivanje:
                        rinoAction = "Otkazivanje";
                        break;
                }

                // RINO vrsta poverioca
                var rinoVrstaPoverica = "";

                switch (listItem.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        rinoVrstaPoverica = "Pravno lice";
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        rinoVrstaPoverica = "Javni sektor";
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        rinoVrstaPoverica = "Polj. gazdinstva";
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        rinoVrstaPoverica = "Kompenzacija";
                        break;
                }

                // RINO due date validity check
                var rinoDueDate = !CheckUninitializedDate(listItem.DatumRokaZaIzmirenje)
                    ? listItem.DatumRokaZaIzmirenje.ToString("dd.MM.yyyy")
                    : null;

                // RINO reason for change variable
                var rinoReasonForChange = listItem.RazlogIzmene;

                // Adding new row to DataTable object
                RinoDataTable.Rows.Add(
                    rinoAction,
                    listItem.Iznos,
                    listItem.NazivPoverioca,
                    listItem.PibPoverioca,
                    listItem.MbPoverioca,
                    rinoVrstaPoverica,
                    listItem.NazivDokumenta,
                    listItem.BrojDokumenta,
                    listItem.DatumDokumenta.ToString("dd.MM.yyyy"),
                    listItem.DatumNastanka.ToString("dd.MM.yyyy"),
                    rinoDueDate,
                    rinoReasonForChange);
            }
        }

        #endregion

        #region Rino List manipulation methods region

        /// <summary>
        ///     Get's RinoObligationItem at selected index.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects.</param>
        /// <param name="index">Index position in the list.</param>
        /// <returns>RinoObligationItem object at requested index position.</returns>
        public RinoObligationItem GetRoiItemAt(List<RinoObligationItem> roiList, int index)
        {
            return roiList.ElementAt(index);
        }

        /// <summary>
        ///     Remove RinoObligationItem object from list at selected index.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoObligationItem's objects.</returns>
        public List<RinoObligationItem> RemoveItemAt(List<RinoObligationItem> roiList, int index)
        {
            // Removing item from list at specific index
            roiList.RemoveAt(index);

            return roiList;
        }

        /// <summary>
        ///     Inserts RinoObligationItem object to list.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="roiItem">RinoObligationItem object to insert</param>
        /// <returns>New lkist of RinoObligationItem's objects.</returns>
        public List<RinoObligationItem> InsertNewItem(List<RinoObligationItem> roiList, RinoObligationItem roiItem)
        {
            roiList.Add(roiItem);

            return roiList;
        }

        /// <summary>
        ///     Modifies existing list of RinoObligationItem's objects. Removes object at selected index and inserts a new
        ///     one in its place.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="roiItem">RinoObligationItem object</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoObligationItem's objects</returns>
        public List<RinoObligationItem> ModifyExistingItem(List<RinoObligationItem> roiList, RinoObligationItem roiItem,
            int index)
        {
            roiList.RemoveAt(index);

            roiList.Insert(index, roiItem);

            return roiList;
        }

        #endregion
    }
}