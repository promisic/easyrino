/*
 *  RINO reconcilement manager class
 *  Copyright (C) 2016 - 2019 Dusan Misic <promisic@outlook.com>
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
using System.Linq;
using System.Globalization;
using System.Xml;

using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.Core
{
    internal class RinoReconcilementManager
    {
        #region Internal DataTable and properties region

        /// <summary>
        /// Constructor
        /// </summary>
        internal RinoReconcilementManager()
        {
            // Filling datatable with columns
            FillDataTableWithColumns();
        }

        /// <summary>
        /// Property to get RINO data table.
        /// </summary>
        public DataTable RinoDataTable { get; private set; }

        /// <summary>
        /// Property to get or set JBBK ID.
        /// </summary>
        public string Jbbk { get; set; }

        /// <summary>
        /// Property to get or set valid obligation field.
        /// </summary>
        public bool ValidReconcilement { get; private set; }

        #endregion

        #region Convert XML to Rino List region

        /// <summary>
        /// Converts XmlDocument object to RinoReconcilementItem object.
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <returns>List of RinoReconcilementItem objects.</returns>
        internal List<RinoReconcilementItem> ConvertXmlToRinoList(XmlDocument rinoXmlDoc)
        {
            // Creating RinoReconcilementItem list object
            List<RinoReconcilementItem> rri = new List<RinoReconcilementItem>();

            // Getting RINO header nodes
            XmlNodeList rinoXmlHeaderNodes = rinoXmlDoc.SelectNodes("//RINO");

            if (rinoXmlHeaderNodes != null)
                foreach (XmlNode xmlNode in rinoXmlHeaderNodes)
                {
                    XmlNode jbbkNode = xmlNode.SelectSingleNode("JBBK");
                    XmlNode typeNode = xmlNode.SelectSingleNode("Tip");

                    // Setting JBBK
                    if (jbbkNode != null) Jbbk = jbbkNode.InnerText;

                    if (typeNode != null) ValidReconcilement = typeNode.InnerText == "Izmirenje";
                }

            // Execute parsing stage ONLY if it is valid obligation XML file
            if (ValidReconcilement)
            {
                // Doing XPath search query
                XmlNodeList xmlNodes = rinoXmlDoc.SelectNodes("//RINO/Izmirenja/Izmirenje");

                // Iterating each XmlNode
                if (xmlNodes != null)
                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        // Creating rri object to add to existing list
                        RinoReconcilementItem rriObj = new RinoReconcilementItem();

                        // Checking type of current XmlNode
                        if (xmlNode.Attributes != null)
                        {
                            string xmlNodeValue = xmlNode.Attributes["VrstaPosla"].Value;

                            switch (xmlNodeValue)
                            {
                                case "U":
                                    // Inserting new item
                                    rriObj.Action = RinoActionType.Unos;
                                    break;
                                case "I":
                                    // Changing existing item
                                    rriObj.Action = RinoActionType.Izmena;
                                    break;
                                case "O":
                                    // Deleting existing item
                                    rriObj.Action = RinoActionType.Otkazivanje;
                                    break;
                            }
                        }

                        // Getting RINO ID
                        XmlNode idNode = xmlNode.SelectSingleNode("Id");

                        // Setting RINO ID
                        if (idNode != null) rriObj.RinoId = Convert.ToInt64(idNode.InnerText);

                        // Getting document number
                        XmlNode bdNode = xmlNode.SelectSingleNode("BrojDokumenta");

                        // Setting document number
                        if (bdNode != null) rriObj.BrojDokumenta = bdNode.InnerText;

                        // Getting PIB ID
                        XmlNode pibNode = xmlNode.SelectSingleNode("PIBPoverioca");

                        // Setting PIB ID
                        if (pibNode != null) rriObj.PibPoverioca = pibNode.InnerText;

                        // Getting bank info
                        XmlNode bankNode = xmlNode.SelectSingleNode("Banka");

                        // Setting bank info
                        if (bankNode != null)
                        {
                            rriObj.Banka = bankNode.InnerText;

                            // Getting ReklPodZaRek node
                            XmlNode rpzrNode = xmlNode.SelectSingleNode("ReklPodZaRek");

                            // Setting ReklPodZaRek value
                            if (rpzrNode != null) rriObj.ReklPodZaRek = rpzrNode.InnerText;

                            // Setting bank info
                            rriObj.Banka = bankNode.InnerText;
                        }

                        // Getting document creation date
                        XmlNode diNode = xmlNode.SelectSingleNode("DatumIzmirenja");

                        // Checking if DatumIzmirenja is empty
                        rriObj.DatumIzmirenja = diNode != null && diNode.InnerText.Length > 0
                            ? DateTime.ParseExact(diNode.InnerText, "yyyy-MM-dd", null)
                            : new DateTime();

                        // Getting ammount
                        XmlNode iznosNode = xmlNode.SelectSingleNode("Iznos");

                        // Setting amount
                        if (iznosNode != null)
                            rriObj.Iznos = decimal.Parse(iznosNode.InnerText, CultureInfo.GetCultureInfo("en-US"));

                        // This node may be missing / is optional.
                        if (xmlNode.SelectSingleNode("RazlogIzmene") != null)
                        {
                            // Getting reason for change
                            XmlNode riNode = xmlNode.SelectSingleNode("RazlogIzmene");

                            // Setting reason for change
                            if (riNode != null) rriObj.RazlogIzmene = riNode.InnerText;
                        }

                        // Adding rriObj object to rri
                        rri.Add(rriObj);
                    }
            }

            return rri;
        }

        #endregion

        #region Convert Rino List to XML region

        /// <summary>
        /// Converts list of RinoReconcilementItem's to RINO XmlDocument object
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's object</param>
        /// <returns>RINO XmlDocument object</returns>
        internal XmlDocument ConvertRinoListToXml(List<RinoReconcilementItem> rriList)
        {
            // Creating RINO XML object
            XmlDocument rinoXml = new XmlDocument();

            // Creating XML declaration
            XmlDeclaration xmlOrderDecl = rinoXml.CreateXmlDeclaration("1.0", "UTF-8", "yes");

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
            rinoTipNode.InnerText = "Izmirenje";
            // Append node
            rinoRootNode.AppendChild(rinoTipNode);

            // Creating Izmirenja node
            XmlNode rinoIzmirenjaNode = rinoXml.CreateElement("Izmirenja");
            // Append node
            rinoRootNode.AppendChild(rinoIzmirenjaNode);

            // Iterating through rriList object
            foreach (RinoReconcilementItem rriItem in rriList)
            {
                // Creating Izmirenje node
                XmlNode rinoIzmirenjeNode = rinoXml.CreateElement("Izmirenje");
                // Creating attribute
                XmlAttribute rinoIzmirenjeAttr = rinoXml.CreateAttribute("VrstaPosla");
                // Setting attribute value
                string vrstaPosla = null;

                switch (rriItem.Action)
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

                rinoIzmirenjeAttr.Value = vrstaPosla;
                // Appending attribute
                rinoIzmirenjeNode.Attributes?.Append(rinoIzmirenjeAttr);
                // Append node
                rinoIzmirenjaNode.AppendChild(rinoIzmirenjeNode);

                // Creating RINO ID node
                XmlNode rinoIdNode = rinoXml.CreateElement("Id");
                // Setting RINO ID node
                rinoIdNode.InnerText = rriItem.RinoId.ToString();
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoIdNode);

                // Creating BrojDokumenta node
                XmlNode rinoBdNode = rinoXml.CreateElement("BrojDokumenta");
                // Setting BrojDokumenta value
                rinoBdNode.InnerText = rriItem.BrojDokumenta;
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoBdNode);

                // Creating PIBPoverioca node
                XmlNode rinoPibNode = rinoXml.CreateElement("PIBPoverioca");
                // Setting PIBPoverica value
                rinoPibNode.InnerText = rriItem.PibPoverioca;
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoPibNode);

                // Creating Banka node
                XmlNode rinoBankaNode = rinoXml.CreateElement("Banka");
                // Setting Banka value
                rinoBankaNode.InnerText = rriItem.Banka;
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoBankaNode);

                // Creating ReklPodZaRekl node
                XmlNode rinoRpzrNode = rinoXml.CreateElement("ReklPodZaRek");
                // Setting ReklPodZaRekl value
                rinoRpzrNode.InnerText = rriItem.ReklPodZaRek;
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoRpzrNode);

                // Creating DatumIzmirenja node
                XmlNode rinoDiNode = rinoXml.CreateElement("DatumIzmirenja");
                // Setting DatumDokumenta value
                rinoDiNode.InnerText = rriItem.DatumIzmirenja.ToString("yyyy-MM-dd");
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoDiNode);

                // Creating Iznos node
                XmlNode rinoIznosNode = rinoXml.CreateElement("Iznos");
                // Setting Iznos value
                rinoIznosNode.InnerText = rriItem.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                // Append node
                rinoIzmirenjeNode.AppendChild(rinoIznosNode);

                // Checking for optional RazlogIzmene
                if (!string.IsNullOrEmpty(rriItem.RazlogIzmene))
                {
                    // Creating RazlogIzmene node
                    XmlNode rinoRiNode = rinoXml.CreateElement("RazlogIzmene");
                    // Setting RazlogIzmene value
                    rinoRiNode.InnerText = rriItem.RazlogIzmene;
                    // Append node
                    rinoIzmirenjeNode.AppendChild(rinoRiNode);
                }
            }

            return rinoXml;
        }

        #endregion

        #region DataTable manipulation methods region

        /// <summary>
        /// Clears all rows inside internal DataTable object.
        /// </summary>
        private void ClearDataTableRows()
        {
            RinoDataTable.Rows.Clear();
        }

        /// <summary>
        /// Fill's interal DataTable object with columns.
        /// </summary>
        private void FillDataTableWithColumns()
        {
            // Adding columns to DataTable object
            RinoDataTable.Columns.Add("rinoAction", typeof(string));
            RinoDataTable.Columns.Add("rinoId", typeof(long));
            RinoDataTable.Columns.Add("rinoBrojDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoPIB", typeof(string));
            RinoDataTable.Columns.Add("rinoBanka", typeof(string));
            RinoDataTable.Columns.Add("rinoReklPodZaRek", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumIzmirenja", typeof(string));
            RinoDataTable.Columns.Add("rinoIznos", typeof(decimal));
            RinoDataTable.Columns.Add("rinoRazlogIzmene", typeof(string));
        }

        /// <summary>
        /// Converts list of RinoReconcilementItem's objects to DataTable rows.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        public void ConvertRinoListToDataTable(List<RinoReconcilementItem> rriList)
        {
            // Clearing RINO DataTable object
            ClearDataTableRows();

            foreach (RinoReconcilementItem listItem in rriList)
            {
                // RINO action
                string rinoAction = "";

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

                // RINO due date validity check
                var rinoPaidDate = !CheckUninitializedDate(listItem.DatumIzmirenja) ? listItem.DatumIzmirenja.ToString("dd.MM.yyyy") : null;

                // RINO reason for change variable
                var rinoReasonForChange = listItem.RazlogIzmene;

                // Adding new row to DataTable object
                RinoDataTable.Rows.Add(
                    rinoAction,
                    listItem.RinoId,
                    listItem.BrojDokumenta,
                    listItem.PibPoverioca,
                    listItem.Banka,
                    listItem.ReklPodZaRek,
                    rinoPaidDate,
                    listItem.Iznos,
                    rinoReasonForChange);
            }
        }

        #endregion

        #region Utility method region

        /// <summary>
        /// Checks for uninitialized date.
        /// </summary>
        /// <param name="dateToCheck">DateTime object to check.</param>
        /// <returns>True if date is "default uninitialized one", fasle if otherwise.</returns>
        public bool CheckUninitializedDate(DateTime dateToCheck)
        {
            // Creating DateTime object similar to uninitialized datetime
            DateTime uninDateTime = new DateTime(0001, 1, 1, 0, 0, 0); // Target date is: 1.1.0001. 00.00.00

            // Comparing dates
            int dateCompResult = DateTime.Compare(dateToCheck, uninDateTime);

            if (dateCompResult == 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Rino List manipulation methods region

        /// <summary>
        /// Get's RinoReconcilementItem at selected index.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects.</param>
        /// <param name="index">Index position in the list.</param>
        /// <returns>RinoReconcilementItem object at requested index position.</returns>
        public RinoReconcilementItem GetRriItemAt(List<RinoReconcilementItem> rriList, int index)
        {
            return rriList.ElementAt(index);
        }

        /// <summary>
        /// Remove RinoReconcilementItem object from list at selected index.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoReconcilementItem's objects.</returns>
        public List<RinoReconcilementItem> RemoveItemAt(List<RinoReconcilementItem> rriList, int index)
        {
            // Removing item from list at specific index
            rriList.RemoveAt(index);

            return rriList;
        }

        /// <summary>
        /// Inserts RinoReconcilementItem object to list.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="rriItem">RinoReconcilementItem object to insert</param>
        /// <returns>New lkist of RinoReconcilementItem's objects.</returns>
        public List<RinoReconcilementItem> InsertNewItem(List<RinoReconcilementItem> rriList, RinoReconcilementItem rriItem)
        {
            rriList.Add(rriItem);

            return rriList;
        }

        /// <summary>
        /// Modifies existing list of RinoReconcilementItem's objects. Removes object at selected index and inserts a new 
        /// one in its place.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="rriItem">RinoReconcilementItem object</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoReconcilementItem's objects</returns>
        public List<RinoReconcilementItem> ModifyExistingItem(List<RinoReconcilementItem> rriList, RinoReconcilementItem rriItem,
            int index)
        {
            rriList.RemoveAt(index);

            rriList.Insert(index, rriItem);

            return rriList;
        }

        #endregion
    }
}
