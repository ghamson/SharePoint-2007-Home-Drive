using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;

namespace homeDrive
{
    [Guid("124e74e4-fee0-403c-adb9-f7230cf32acf")]
    public class homeDrive : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private bool _error = false;
        private string _UNCPath = null;
        private int _height = 0;
        private bool _locationDetails = true;
        private string _UNCPathExtended = null;

        //SPUser currentUser = SPContext.Current.Web.CurrentUser;
        string userName = SPContext.Current.Web.CurrentUser.Name;
        string userLogin = SPContext.Current.Web.CurrentUser.LoginName;
        string userEmail = SPContext.Current.Web.CurrentUser.Email;

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Home Drive Configuration")]
        [WebDisplayName("UNC Path (I.E: \\\\fileserver\\users\\)")]
        [WebDescription("The current logged in Windows User will be appended to the supplied UNC path.")]
        public string UNCPath
        {
            get
            {
                if (_UNCPath == null)
                {
                    _UNCPath = "";
                }
                return _UNCPath;
            }
            set { _UNCPath = value; }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Home Drive Configuration")]
        [WebDisplayName("After UNC Path string (I.E: $)")]
        [WebDescription("This will appended after the UNC Path and the username strings")]
        public string UNCPathExtended
        {
            get
            {
                if (_UNCPathExtended == null)
                {
                    _UNCPathExtended = "";
                }
                return _UNCPathExtended;
            }
            set { _UNCPathExtended = value; }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Home Drive Configuration")]
        [WebDisplayName("iFrame Height")]
        [WebDescription("Integer value representing pixels.  Default: 700 = 700px")]
        public int height
        {
            get
            {
                if (_height == 0)
                {
                    _height = 700;
                }
                return _height;
            }
            set { _height = value; }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Home Drive Configuration")]
        [WebDisplayName("Show Path Details")]
        [WebDescription("Show's the UNC Path to the end user above the explorer window")]
        public bool LocationDetails
        {
            get
            {
                return _locationDetails;
            }
            set
            {
                _locationDetails = value;
            }
        }

        public homeDrive()
        {
            this.ExportMode = WebPartExportMode.All;
        }

        /// <summary>
        /// Create all your controls here for rendering.
        /// Try to avoid using the RenderWebPart() method.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!_error)
            {
                try
                {

                    base.CreateChildControls();

                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            string outHTML = "";
            //string currentWindowsUser = System.Environment.UserName; //Do a comparison on this.
            
            

            if (UNCPath == "")
            {
                outHTML = outHTML + "<br /><span style='font-family: MS Sans Serif; font-size: 8pt; color: black;'>To edit the configuration, <a href=\"javascript:MSOTlPn_ShowToolPane2Wrapper('Edit', this, '" + this.ID + "')\">open tool pane</a>.  UNC Path required.</span><br />&nbsp;<br />";
            }
            else
            {
                int test = userLogin.IndexOf("\\", 1);
                if (LocationDetails == true)
                {
                    outHTML = outHTML + "<span style='color: black; font-family: Arial; font-size: 9pt;'><b>Location:</b>&nbsp;" + UNCPath + userLogin.Substring(test + 1, userLogin.Length - (test + 1)) + UNCPathExtended + "</span><br />";
                }
                
                outHTML = outHTML + "<iframe src='" + UNCPath + userLogin.Substring(test + 1, userLogin.Length - (test + 1)) + UNCPathExtended + "' width='100%' height='" + height + "px'></iframe>";
                //outHTML = outHTML + UNCPath + userLogin.Substring(test + 1, userLogin.Length - (test + 1));
            }
            
            writer.Write(outHTML);
        }

        /// <summary>
        /// Ensures that the CreateChildControls() is called before events.
        /// Use CreateChildControls() to create your controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();
                
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Clear all child controls and add an error message for display.
        /// </summary>
        /// <param name="ex"></param>
        private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }
    }
}
