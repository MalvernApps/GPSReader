// --------------------------------
// <copyright file="RegistryAccess.cs" company="Sam and Glenn Murray">
//     All rights reserved
// </copyright>
// <author> Sam Murray and Glenn Murray</author>
// ---------------------------------
#region Configuration Control
// - 
// $History: RegistryAccess.cs $
//	
//	*****************  Version 1  *****************
//	User: Glenn        Date: 8/06/09    Time: 20:28
//	Created in $/GUIs/GPS_Reader.root/GPS_Reader/GPS_Reader
//	Added in saving to database
//	
//	*****************  Version 2  *****************
//	User: Glenn        Date: 2/01/09    Time: 0:16
//	Updated in $/GUIs/InfoViewer.root/InfoViewer/ApplicationSDK/ApplicationSDK
//	I have updated the commenting and headers in a lot of the code to up
//	comment quality level and be more stylecop compliant
// - 
#endregion

namespace GPS_Reader
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Win32;              // needed for registry

    /// <summary>
    /// Access the registry for common application settings
    /// </summary>
    public class RegistryAccess
    {
        public string GetDatabaseConenctionString()
        {
            // save something to the registry
            RegistryKey softkey = Registry.LocalMachine.OpenSubKey("Software", false );
            RegistryKey appKey = softkey.OpenSubKey("Infoviewer");

            // now get values
            return  (string)appKey.GetValue("Database Connection Sting");
        }

        /// <summary>
        /// Get a specific registry string forthe application
        /// </summary>
        /// <param name="value">string in the registry to get</param>
        /// <returns>value of the string</returns>
        public string GetRegistryString( string value )
        {
            // save something to the registry
            RegistryKey softkey = Registry.LocalMachine.OpenSubKey("Software", true);
            RegistryKey appKey = softkey.CreateSubKey("Infoviewer");

            // now get values
            return (string)appKey.GetValue( value );
        }

        /// <summary>
        /// Set a specific registry string
        /// </summary>
        /// <param name="value">string in registry</param>
        /// <param name="newValue">the value</param>
        public void SetRegistryString(string value , string newValue )
        {
            // save something to the registry
            RegistryKey softkey = Registry.LocalMachine.OpenSubKey("Software", true);
            RegistryKey appKey = softkey.CreateSubKey("Infoviewer");

            // now get values
            appKey.SetValue(value, newValue , RegistryValueKind.String );
        }
    }
}
