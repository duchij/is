﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="hlasko.aspx.cs" Inherits="hlasko" MaintainScrollPositionOnPostback="true"   %>
<%--<%@ Register TagPrefix="druhadk_hlasko" TagName="dk_hlasko" Src="~/Controls/druhadk_hlasko.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <asp:ScriptManager ID="ScriptManager1" runat="server" AllowCustomErrorsRedirect="true"></asp:ScriptManager>

    <%-- hlasko pre 2dk --%>
    <asp:PlaceHolder ID="hlasko_pl" runat="server" >
        <asp:HiddenField ID="newsDialogShow" runat="server" Value="" />
        <%--<druhadk_hlasko:dk_hlasko runat="server"></druhadk_hlasko:dk_hlasko>--%>
    </asp:PlaceHolder>


</asp:Content>

