using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace is_kdch.TinyMCE
{
    public class TextEditor : TextBox
    {
        protected override void OnPreRender(EventArgs e)
        {
            string tinyMceIncludeKey = "TinyMCEInclude";
            string tinyMceIncludeScript = "<script type=\"text/javascript\" src=\"../tinymce/jscripts/tiny_mce/tiny_mce.js\"></script>";

            if (!Page.ClientScript.IsStartupScriptRegistered(tinyMceIncludeKey))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), tinyMceIncludeKey, tinyMceIncludeScript);
            }

            if (!Page.ClientScript.IsStartupScriptRegistered(GetInitKey()))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), GetInitKey(), GetInitScript());
            }

            if (!CssClass.Contains(GetEditorClass())) //probably this is not the best way how to add the css class but I do not know any beter way
            {
                if (CssClass.Length > 0)
                {
                    CssClass += " ";
                }
                CssClass += GetEditorClass();
            }
            base.OnPreRender(e);
        }

        private string GetInitKey()
        {
            string simpleKey = "TinyMCESimple";
            string fullKey = "TinyMCEFull";
            string readOnlyKey = "TinyMCEReadOnly";

            switch (Mode)
            {
                case TextEditorMode.Simple:
                    return simpleKey;
                case TextEditorMode.Full:
                    return fullKey;
                case TextEditorMode.ReadOnly:
                    return readOnlyKey;
                default:
                    goto case TextEditorMode.Simple;
            }
        }

        private string GetEditorClass()
        {
            return GetEditorClass(Mode);
        }

        private string GetEditorClass(TextEditorMode mode)
        {
            string simpleClass = "SimpleTextEditor";
            string fullClass = "FullTextEditor";
            string readOnlyClass = "ReadOnlyTextEditor";
            switch (mode)
            {
                case TextEditorMode.Simple:
                    return simpleClass;
                case TextEditorMode.Full:
                    return fullClass;
                case TextEditorMode.ReadOnly:
                    return readOnlyClass;
                default:
                    goto case TextEditorMode.Simple;
            }
        }

        private string GetInitScript()
        {
            string simpleScript =
                "<script language=\"javascript\" type=\"text/javascript\">tinyMCE.init({{mode : \"textareas\",theme : \"simple\",editor_selector : \"{0}\"}});</script>";
            string fullScript =
                "<script language=\"javascript\" type=\"text/javascript\">tinyMCE.init({{mode : \"textareas\",theme : \"advanced\",editor_selector : \"{0}\"}});</script>";

            string readOnlyScript =
              "<script language=\"javascript\" type=\"text/javascript\">tinyMCE.init({{mode : \"textareas\",theme : \"advanced\", readonly : \"true\" ,editor_selector : \"{0}\"}});</script>";

            switch (Mode)
            {
                case TextEditorMode.Simple:
                    return string.Format(simpleScript, GetEditorClass(TextEditorMode.Simple));
                case TextEditorMode.Full:
                    return string.Format(fullScript, GetEditorClass(TextEditorMode.Full));
                case TextEditorMode.ReadOnly:
                    return string.Format(readOnlyScript, GetEditorClass(TextEditorMode.ReadOnly));
                default:
                    goto case TextEditorMode.Simple;
            }
        }

        public override TextBoxMode TextMode
        {
            get
            {
                return TextBoxMode.MultiLine;
            }
        }

        public TextEditorMode Mode
        {
            get
            {
                Object obj = ViewState["Mode"];
                if (obj == null)
                {
                    return TextEditorMode.Simple;
                }
                return (TextEditorMode)obj;
            }
            set
            {
                ViewState["Mode"] = value;
            }
        }

        public enum TextEditorMode
        {
            Simple,
            Full,
            ReadOnly
        }
    }
}