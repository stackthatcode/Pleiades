﻿<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome to Pleiades Commerce Engine v0.7</h1>
    <div style="clear:both; height:20px;"></div>

    <p>This is a temporary placeholder until more content is developed!</p>

    <%: Html.RouteLink("Manage Admin Users", OutboundNavigation.AdminManagerList()) %>

    <br />


</asp:Content>
